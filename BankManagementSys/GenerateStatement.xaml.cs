using Microsoft.VisualBasic;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SharedCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
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
    /// Interaction logic for GenerateStatement.xaml
    /// </summary>
    public partial class GenerateStatement : Window
    {
        User currentUser;
        Account currentAccount;
        DateTime finishDate = DateTime.Today;

        public GenerateStatement(User user, Account account)
        {
            InitializeComponent();
            currentUser = user;
            currentAccount = account;
            if (currentAccount.IsActive == false)
            {
                finishDate = (DateTime)currentAccount.CloseDate;
            }
            LoadYears();
            //calendarMonthYear.SelectedDate = new DateTime(1983, 01, 01);
        }
        private void LoadYears()
        {
            List<int> years = new List<int>();

            try
            {
                for (int i = currentAccount.OpenDate.Year; i <= finishDate.Year; i++)
                {
                    years.Add(i);
                }

                comboStatementYears.ItemsSource = years;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<string> LoadMonths()
        {
            List<string> months = new List<string>();
            int currentSelectedYear = (int)comboStatementYears.SelectedItem;


            if (currentSelectedYear == currentAccount.OpenDate.Year && currentSelectedYear == DateTime.Now.Year)
            {
                months.Clear();
                // select months only from month of opeing and today's(or closing date) month
                for (int i = currentAccount.OpenDate.Month; i <= finishDate.Month; i++)
                {
                    months.Add(DateAndTime.MonthName(i));
                }
            }
            else if (currentSelectedYear == finishDate.Year)
            {
                months.Clear();
                // need to select from january to current month (or to month of closing account)
                for (int i = 1; i <= finishDate.Month; i++)
                {
                    months.Add(DateAndTime.MonthName(i));
                }
            }
            else if (currentSelectedYear == currentAccount.OpenDate.Year)
            {
                months.Clear();
                // only from date when person open acct to december
                for (int i = currentAccount.OpenDate.Month; i <= 12; i++)
                {
                    months.Add(DateAndTime.MonthName(i));
                }
            }
            else
            {
                months.Clear();
                for (int i = 1; i <= 12; i++)
                {
                    // select all months
                    months.Add(DateAndTime.MonthName(i));
                }
            }
            return months;
        }

        private void comboStatementYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboStatementYears.Items.Count == 0 || comboStatementYears.SelectedIndex == -1)
            {
                MessageBox.Show("You should select a year first.", "Action required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            comboStatementMonths.IsEnabled = true;
            comboStatementMonths.ItemsSource = LoadMonths();
        }

        private void comboStatementMonths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentAccount == null)
            {
                MessageBox.Show("It was not possible to define an account to generate a statement.");
                return;
            }

            int selectedYear = (int)comboStatementYears.SelectedItem;
            if (comboStatementMonths.Items.Count == 0 || comboStatementMonths.SelectedIndex == -1)
            {
                Utilities.Transactions = null;
                lvMonthStatement.ItemsSource = Utilities.Transactions;
                return;
            }
            string selectedMonthStr = comboStatementMonths.SelectedItem.ToString();
            int selectedMonth = DateTime.ParseExact(selectedMonthStr, "MMMM", CultureInfo.CurrentCulture).Month;

            try
            {
                Utilities.Transactions = EFData.context.Transactions.Where(t => t.AccountId ==
            currentAccount.Id && t.Date.Year == selectedYear && t.Date.Month == selectedMonth).ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Error fetching from Database: " + ex.Message);
            }

            if (Utilities.Transactions != null)
            {
                lvMonthStatement.ItemsSource = Utilities.Transactions;
            }

            if (lvMonthStatement.Items.Count != 0)
            {
                btExport.IsEnabled = true;
                if (currentAccount.User.Email != null)
                {
                    btByEmail.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("No transaction was registered during the selected period");
                btExport.IsEnabled = false;
            }
        }

        private void btByEmail_Click(object sender, RoutedEventArgs e)
        {
            PdfDocument doc = CreatePdf();

            try
            {
                doc.Save("Statement.pdf");

                string file = "Statement.pdf";
                SmtpClient client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential()
                    {
                        UserName = "johnabbottbank@gmail.com",
                        Password = "querty123!"
                    }
                };
                MailAddress FromEmail = new MailAddress("johnabbottbank@gmail.com", "John Abbott Bank");
                MailAddress ToEmail = new MailAddress(currentAccount.User.Email, "Customer");

                MailMessage mess = new MailMessage(
                    "johnabbottbank@gmail.com",
                    currentAccount.User.Email,
                    "Transaction receipt from " + DateTime.Now.ToShortDateString(),
                    "Please see the attached statement.\nThank you,\n John Abbott Bank");

                Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);

                mess.Attachments.Add(data);


                client.Send(mess);
                MessageBox.Show("The statement was sent", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                    ex.ToString());
            }
            catch (IOException ex)
            {
                MessageBox.Show("Attachment error: " + ex.Message, "Attachment error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private PdfDocument CreatePdf()
        {
            PdfDocument doc = null;
            try
            {
                string year = comboStatementYears.SelectedItem.ToString();
                string month = comboStatementMonths.SelectedItem.ToString();
                XImage logo = XImage.FromFile("johnabbottbank.png");

                doc = new PdfDocument();
                doc.Info.Title = "Banking history";
                PdfPage page = doc.AddPage();

                XGraphics graphics = XGraphics.FromPdfPage(page);

                XFont fontReg = new XFont("Arial", 10, XFontStyle.Regular);
                XFont fontBold = new XFont("Arial", 10, XFontStyle.Bold);
                XFont fontItalic = new XFont("Arial", 10, XFontStyle.Italic);
                XFont fontBoldItalic = new XFont("Arial", 15, XFontStyle.BoldItalic);


                //graphics.DrawString("John Abbott Bank®", fontItalic, XBrushes.Black, 480, 30);
                graphics.DrawString("Account Holder: " + Utilities.login.User.FirstName + " " + Utilities.login.User.LastName, fontBold, XBrushes.Black, 20, 30);
                graphics.DrawString("Account Number: " + currentAccount.Id, fontBold, XBrushes.Black, 20, 45);
                graphics.DrawString("Current Balance: $ " + currentAccount.Balance, fontBold, XBrushes.Black, 20, 60);
                graphics.DrawString(DateTime.Now.ToString(), fontBold, XBrushes.Black, 20, 75);
                graphics.DrawString(month + " " + year + " Statement", fontBoldItalic, XBrushes.Black, 250, 60);
                XPen lineRed = new XPen(XColors.Green, 5);
                XPoint pt1 = new XPoint(0, 90);
                XPoint pt2 = new XPoint(page.Width, 90);
                graphics.DrawLine(lineRed, pt1, pt2);
                graphics.DrawString("TRANSACTION TYPE", fontBold, XBrushes.Black, 20, 105);
                graphics.DrawString("DATE", fontBold, XBrushes.Black, 250, 105);
                graphics.DrawString("AMOUNT", fontBold, XBrushes.Black, 450, 105);
                AddLogo(graphics, page, "johnabbottbank.png", 500, 0);

                List<Transaction> tr = new List<Transaction>();
                foreach (Transaction item in lvMonthStatement.Items)
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
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message + "Error");
            }
            return doc;
        }

        private void AddLogo(XGraphics gfx, PdfPage page, string imagePath, int xPosition, int yPosition)
        {
            try
            {
                XImage xImage = XImage.FromFile(imagePath);
                gfx.DrawImage(xImage, xPosition, yPosition, xImage.PixelWidth / 3, xImage.PixelWidth / 3);
            }
            catch(IOException ex)
            {
                MessageBox.Show("Error reading logo from file: " + ex.Message, "Internal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btExport_Click(object sender, RoutedEventArgs e)
        {
            PdfDocument doc = CreatePdf();
            string year = comboStatementYears.SelectedItem.ToString();
            string month = comboStatementMonths.SelectedItem.ToString();

            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "PDF Files (*.pdf)|*.pdf|All files(*.*)|*.*";
                saveFile.InitialDirectory = @"C:\Documents\";
                saveFile.Title = month + " " + year + " Statement";
                if (saveFile.ShowDialog() == true)
                {
                    doc.Save(saveFile.FileName);
                    Process.Start(saveFile.FileName);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message + "Error");
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvMonthStatement_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvMonthStatement.Items.Count == 0 || lvMonthStatement.SelectedIndex == -1)
            {
                return;
            }
            Transaction currTrans = (Transaction)lvMonthStatement.SelectedItem;

            Receipt receiptDlg = new Receipt(currentAccount, 0, currTrans, currentUser, false);
            receiptDlg.Owner = this;
            receiptDlg.ShowDialog();
        }
    }
}
