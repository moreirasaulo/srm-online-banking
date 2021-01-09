using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SharedCode;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for ViewAccountInfo.xaml
    /// </summary>
    public partial class ViewAccountInfo : Window
    {
        User currentUser;
        Account currentAccount;
        public ViewAccountInfo(User user,Account account)
        {
            InitializeComponent();
            currentUser = user;
            currentAccount = account;
            LoadInfoToFileds();
            comboHistory.ItemsSource = Utilities.transactionHistoryDays;
            comboHistory.SelectedIndex = 0;
           
        }

        private void LoadInfoToFileds()
        {
            if (currentUser.CompanyName != null)
            {
                lblDate.Content = "Company registration date:";
                lblId.Content = "Company registration number:";
                lblCompanyName.Content = "Company name:";
                lblCompanyNameValue.Content = currentUser.CompanyName;
            }
            lblFullName.Content = currentUser.FullName;
            lblDateOfBirth.Content = currentUser.DateOfBirth.ToShortDateString();
            lblNatId.Content = currentUser.NationalId;
            lblAccNo.Content = currentAccount.Id;
            lblOpenDate.Content = currentAccount.OpenDate.ToShortDateString();
            lblBalance.Content = "$ " + currentAccount.Balance.ToString("0.00");
            lblAccType.Content = currentAccount.AccountType.Description;
            lblInterestFeeDate.Content = currentAccount.InterestFeeDate.ToShortDateString();

            if (currentAccount.AccountType.Description == "Checking")
            {
                lblMonthlyFeeAst.Content = "Monthly fee: *";
                decimal monthlyFee = currentAccount.MonthlyFee.HasValue ? Decimal.Round(currentAccount.MonthlyFee.Value, 2) : 0;
                lblMonthlyFee.Content = "$ " + monthlyFee.ToString("0.00");
                lblInterest.Content = "0 %";
                lblInterestDivid.Content = "Interest:";
                lblInterestOrFee.Content = "Next fee date:";
            }
            else if (currentAccount.AccountType.Description == "Savings")
            {
                lblMonthlyFee.Content = "$ 0";
                lblInterestDivid.Content = "Interest *:";
                lblInterest.Content = currentAccount.Interest + " %";
                lblInterestOrFee.Content = "Next interest date:";
            }
            else if (currentAccount.AccountType.Description == "Investment")
            {
                lblMonthlyFee.Content = "$ 0";
                lblInterestDivid.Content = "Monthly dividends:";
                lblInterest.Content = currentAccount.Interest + " %";
                lblInterestOrFee.Content = "Next dividends date:";
            }
            else if (currentAccount.AccountType.Description == "Business")
            {
                lblMonthlyFee.Content = "$ " + currentAccount.MonthlyFee;
                lblInterest.Content = "0 %";
                lblInterestDivid.Content = "Interest:";
                lblInterestOrFee.Content = "Next fee date:";
            }
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void SortTransactionsByTypeAndDate()
        {
            if(currentAccount == null)
            {
                return;
            }
            List<Transaction> accTransactions = currentAccount.Transactions.ToList();
            if (comboHistory.SelectedIndex == 0)
            {
                accTransactions = accTransactions.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 7);
                accTransactions = SortTransactionsByType(accTransactions);
            }
            else if (comboHistory.SelectedIndex == 1)
            {
                accTransactions = accTransactions.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 30);
                accTransactions = SortTransactionsByType(accTransactions);
            }
            else if (comboHistory.SelectedIndex == 2)
            {
                accTransactions = accTransactions.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 60);
                accTransactions = SortTransactionsByType(accTransactions);
            }
            else
            {
                accTransactions = accTransactions.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 7);
                accTransactions = SortTransactionsByType(accTransactions);
            }
            lvTransactions.ItemsSource = accTransactions;
        }

        private List<Transaction> SortTransactionsByType(List<Transaction> list)
        {
            //by type
            if (rbTransactAll.IsChecked == true)
            {
                return list;
            }
            else if (rbTransacDeposits.IsChecked == true)
            {
                return list = list.Where(t => t.Type == "Deposit").ToList();
            }
            else if (rbTransacWithdrawals.IsChecked == true)
            {
                return list = list.Where(t => t.Type == "Withdrawal").ToList();
            }
            else if (rbTransacPayments.IsChecked == true)
            {
                return list = list.Where(t => t.Type == "Payment").ToList();
            }
            else if (rbTransacTransfers .IsChecked== true)
            {
                return list = list.Where(t => t.Type == "Transfer").ToList();
            }
            else
            {
                return list;
            }
        }

        private void comboHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortTransactionsByTypeAndDate();
            
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SortTransactionsByTypeAndDate();
        }

        private void CallTransactionDialog(string type)
        {
            TransactionDialog transacDlg = new TransactionDialog(currentUser, currentAccount, type);
            transacDlg.Owner = this;
            bool? result = transacDlg.ShowDialog();
            if (result == true)
            {
                SortTransactionsByTypeAndDate();
            }
        }

        //deposit
        private void btDeposit_Click(object sender, RoutedEventArgs e)
        {
            CallTransactionDialog("Deposit");
        }

        private bool IsBalanceSufficient()
        {
            if (currentAccount.Balance <= 0)
            {
                string message = string.Format("Account No {0} has insufficient balance ({1} $) to proceed with operation", currentAccount.Id, currentAccount.Balance);
                MessageBox.Show(message, "Warning: impossible operation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        

        //withdrawal
        private void btWithdrawal_Click(object sender, RoutedEventArgs e)
        {
            if(IsBalanceSufficient() == false) { return; }
            CallTransactionDialog("Withdrawal");
        }

        //transfer
        private void btTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (IsBalanceSufficient() == false) { return; }
            CallTransactionDialog("Transfer");
        }

        //payment
        private void btPayment_Click(object sender, RoutedEventArgs e)
        {
            if (IsBalanceSufficient() == false) { return; }
            CallTransactionDialog("Payment");
        }



        private void lvTransactions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(lvTransactions.Items.Count == 0 || lvTransactions.SelectedIndex == -1)
            {
                return;
            }
            Transaction currTrans = (Transaction)lvTransactions.SelectedItem;
           
                Receipt receiptDlg = new Receipt(currentAccount, 0, currTrans, currentUser, false);
                receiptDlg.Owner = this;
                receiptDlg.ShowDialog();
        }

        private void lblMonthlyFee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (currentAccount.AccountType.Description == "Checking") 
            {
                SetupNewRate setupDlg = new SetupNewRate(currentAccount);
                setupDlg.Owner = this;
                bool? result = setupDlg.ShowDialog();
                if(result == true)
                {
                    LoadInfoToFileds();
                }
            }
        }

        private void lblInterest_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (currentAccount.AccountType.Description == "Savings")
            {
                SetupNewRate setupDlg = new SetupNewRate(currentAccount);
                setupDlg.Owner = this;
                bool? result = setupDlg.ShowDialog();
                if (result == true)
                {
                    LoadInfoToFileds();
                }
            }
        }
    }
}
