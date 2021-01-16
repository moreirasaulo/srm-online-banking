using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SharedCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerUI
{
    /// <summary>
    /// Interaction logic for ManageAccounts.xaml
    /// </summary>
    public partial class ManageAccounts : UserControl
    {
        public ManageAccounts()
        {
            InitializeComponent();
            comboHistory.ItemsSource = Utils.transactionHistoryDays;


            comboAccountType.ItemsSource = Utils.login.User.Accounts;
            comboAccountType.DisplayMemberPath = "AccounNoDescription";

            if (Utils.login.User.Accounts.Count == 0)
            {
                lblError.Content = "There is no bank account linked to your profile yet.";
                return;
            }
        }

       

        private void LoadTransactions()
        {
            try
            {
                Utils.userTransactions = EFData.context.Transactions.Where(t => t.AccountId ==
                Utils.selectedAcc.Id).ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void SortTransactionsByTypeAndDate()
        {
            List<Transaction> sortedTransactions = new List<Transaction>();
            //by type
            if (rbTransactAll.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions;
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else if (rbTransacDeposits.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Deposit").ToList();
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else if (rbTransacWithdrawals.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Withdrawal").ToList();
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else if (rbTransacTransfers.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Transfer").ToList();
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else if (rbTransacPayments.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Payment").ToList();
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else
            {
                sortedTransactions = Utils.userTransactions;
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            lvTransactions.ItemsSource = sortedTransactions;
        }

        private List<Transaction> SortTransactionsByDate(List<Transaction> list)
        {
            if (comboHistory.SelectedIndex == 0)
            {
                try
                {
                    return list = list.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 7);
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return list;
                }
            }
            else if (comboHistory.SelectedIndex == 1)
            {
                try
                {
                    return list = list.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 30);
                }
                catch (ArgumentNullException ex) 
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return list;
                }               
            }
            else if (comboHistory.SelectedIndex == 2)
            {
                try
                {
                    return list = list.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 60);
                }
                catch (ArgumentNullException ex) 
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return list;
                }
            }
            else
            {
                return list;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SortTransactionsByTypeAndDate();
        }



        private void comboHistory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            SortTransactionsByTypeAndDate();
        }

        private void btMakeTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.login.User.Accounts == null || comboAccountType.SelectedIndex == -1)
            {
                MessageBox.Show("Select an account to make a transfer", "Action required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Utils.selectedAcc.Balance <= 0)
            {
                MessageBox.Show("Your account balance is insufficient to make a transfer", "Insufficient balance", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TransferPaymentDialog transferDlg = new TransferPaymentDialog("Transfer");
            transferDlg.Owner = Utils.clientDashboard;
            bool? result = transferDlg.ShowDialog();

            if (result == true)
            {
                LoadTransactions();
                SortTransactionsByTypeAndDate();
                lblBalance.Content = "$ " + Utils.selectedAcc.Balance;
            }
        }

        private void comboAccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboHistory.IsEnabled = true;
            rbTransactAll.IsEnabled = true;
            rbTransacDeposits.IsEnabled = true;
            rbTransacWithdrawals.IsEnabled = true;
            rbTransacTransfers.IsEnabled = true;
            rbTransacPayments.IsEnabled = true;
            Utils.selectedAcc = (Account)comboAccountType.SelectedItem;
            if (Utils.selectedAcc == null)
            {
                MessageBox.Show("Please choose an account to view transactions");
                return;
            }
            lblBalance.Content = "$ " + Utils.selectedAcc.Balance;

            LoadTransactions();
            SortTransactionsByTypeAndDate();
            comboHistory.SelectedIndex = 0;
        }

        private void btMakePayment_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.login.User.Accounts == null || comboAccountType.SelectedIndex == -1)
            {
                MessageBox.Show("Select an account to make a payment", "Action required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Account selectedAcc = (Account)comboAccountType.SelectedItem;
            if (selectedAcc.Balance <= 0)
            {
                MessageBox.Show("Your account balance is insufficient to make a payment", "Insufficient balance", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TransferPaymentDialog paymentDlg = new TransferPaymentDialog("Payment");
            paymentDlg.Owner = Utils.clientDashboard;
            bool? result = paymentDlg.ShowDialog();
            if (result == true)
            {
                LoadTransactions();
                SortTransactionsByTypeAndDate();
                lblBalance.Content = "$ " + Utils.selectedAcc.Balance;
            }
        }

        private void AddLogo(XGraphics gfx, PdfPage page, string imagePath, int xPosition, int yPosition)
        {
            try
            {
                XImage xImage = XImage.FromFile(imagePath);
                gfx.DrawImage(xImage, xPosition, yPosition, xImage.PixelWidth / 3, xImage.PixelWidth / 3);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error reading logo from file: " + ex.Message, "Internal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btPDF_Click(object sender, RoutedEventArgs e)
        {
            Account selectedAcc = (Account)comboAccountType.SelectedItem;
            if (selectedAcc == null)
            {
                MessageBox.Show("You should select an account first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                PdfDocument doc = new PdfDocument();
                doc.Info.Title = "Banking history";
                PdfPage page = doc.AddPage();
                XImage logo = XImage.FromFile("johnabbottbank.png");
                XGraphics graphics = XGraphics.FromPdfPage(page);

                XFont fontReg = new XFont("Arial", 10, XFontStyle.Regular);
                XFont fontBold = new XFont("Arial", 10, XFontStyle.Bold);
                XFont fontItalic = new XFont("Arial", 10, XFontStyle.Italic);

                // graphics.DrawString("John Abbott Bank®", fontItalic, XBrushes.Black, 480, 30);
                graphics.DrawString("Account Holder: " + Utils.login.User.FirstName + " " + Utils.login.User.LastName, fontBold, XBrushes.Black, 20, 30);
                graphics.DrawString("Account Number: " + selectedAcc.Id, fontBold, XBrushes.Black, 20, 45);
                graphics.DrawString("Current Balance: $ " + selectedAcc.Balance, fontBold, XBrushes.Black, 20, 60);
                graphics.DrawString(DateTime.Now.ToString(), fontBold, XBrushes.Black, 20, 75);
                XPen lineRed = new XPen(XColors.Green, 5);
                XPoint pt1 = new XPoint(0, 90);
                XPoint pt2 = new XPoint(page.Width, 90);
                graphics.DrawLine(lineRed, pt1, pt2);
                graphics.DrawString("TRANSACTION TYPE", fontBold, XBrushes.Black, 20, 105);
                graphics.DrawString("DATE", fontBold, XBrushes.Black, 250, 105);
                graphics.DrawString("AMOUNT", fontBold, XBrushes.Black, 450, 105);
                AddLogo(graphics, page, "johnabbottbank.png", 500, 0);

                List<Transaction> tr = new List<Transaction>();
                foreach (Transaction item in lvTransactions.Items)
                {
                    tr.Add(item);
                }

                int ind = 120;
                for (int i = 0; i < tr.Count; i++)
                {
                    Transaction t = tr[i];
                    graphics.DrawString(t.Type, fontReg, XBrushes.Black, 20, ind);
                    graphics.DrawString(t.Date.ToShortDateString(), fontReg, XBrushes.Black, 250, ind);
                    graphics.DrawString(t.Amount.ToString(), fontReg, XBrushes.Black, 450, ind);
                    ind = ind + 15;
                }

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "PDF Files (*.pdf)|*.pdf|All files(*.*)|*.*";
                saveFile.InitialDirectory = @"C:\Documents\";
                saveFile.Title = "Save your banking history to file";
                if (saveFile.ShowDialog() == true)
                {
                    doc.Save(saveFile.FileName);
                    Process.Start(saveFile.FileName);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error saving to file: " + ex.Message, "Error saving to file", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void lvTransactions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (comboAccountType.Items.Count == 0 || comboAccountType.SelectedIndex == -1)
            {
                return;
            }
            Account currentAcc = (Account)comboAccountType.SelectedItem;
            if (lvTransactions.Items.Count == 0 || lvTransactions.SelectedIndex == -1)
            {
                return;
            }
            Transaction currTrans = (Transaction)lvTransactions.SelectedItem;

            Receipt receiptDlg = new Receipt(currentAcc, 0, currTrans, false);
            receiptDlg.Owner = Utils.clientDashboard;
            receiptDlg.ShowDialog();
        }

        private void btViewReport_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.selectedAcc == null)
            {
                MessageBox.Show("First select an account to view spending reports");
                return;
            }
            if (Utils.selectedAcc.AccountType.Description != "Checking")
            {
                MessageBox.Show("Spending reports are available only for checking accounts");
                return;
            }
            SpendingReport spendReportDlg = new SpendingReport(Utils.selectedAcc);
            spendReportDlg.Owner = Utils.clientDashboard;
            spendReportDlg.ShowDialog();
        }
    }
}
