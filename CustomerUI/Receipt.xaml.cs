using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SharedCode;
using System;
using System.Collections.Generic;
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
using WPFCustomMessageBox;

namespace CustomerUI
{
    /// <summary>
    /// Interaction logic for Receipt.xaml
    /// </summary>
    public partial class Receipt : Window
    {
        Transaction currentTans;
        public Receipt(Account account, decimal oldBalance, Transaction transaction, bool needOldBalance)
        {
            InitializeComponent();
            currentTans = transaction;
            this.Title = transaction.Type;
            lblTransType.Content = transaction.Type;
            lblTransTypeAmount.Content = transaction.Type + " amount";

            if (transaction.Type == "Transfer")
            {
                lblBenefAcc.Content = "Beneficiary account:";
                lblBenefAccNo.Content = transaction.ToAccount;
            }
            if (transaction.Type == "Payment")
            {
                lblBenefAcc.Content = "Payee:";
                try
                {
                    lblBenefAccNo.Content = (from u in EFData.context.Users
                                             join acc in EFData.context.Accounts on u.Id equals acc.UserId
                                             where acc.Id == transaction.ToAccount
                                             select u.CompanyName).FirstOrDefault();
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (needOldBalance == true)
            {
                lblPreviousBalance.Content = oldBalance + " $";
            }
            if (needOldBalance == false)
            {
                lblOldBalance.Content = "";
                lblPreviousBalance.Content = "";
                lblNewOrCurrentBalance.Content = "Current balance:";
            }
            lblAccNo.Content = account.Id;
            lblAccHolder.Content = Utils.login.User.FullName;
            lblTransId.Content = transaction.Id;
            lblAmount.Content = string.Format("{0:0.00} $", transaction.Amount);
            lblNewBalance.Content = account.Balance + " $";
            lblDate.Content = transaction.Date;
            lblPrintDate.Content = DateTime.Now;
        }

        private void EmailReceipt(string toEmail)
        {
            //create bmp
            int Width = (int)receiptPanel.RenderSize.Width;
            int Height = (int)receiptPanel.RenderSize.Height;
            string fileName = "receipt.bmp";
            RenderTargetBitmap renderTargetBitmap =
            new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(receiptPanel);
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (Stream fileStream = File.Create(fileName))
            {
                pngImage.Save(fileStream);
            }

            //create pdf
            string pdfFileName = "receipt.pdf";
            PdfDocument doc = new PdfDocument();
            PdfPage oPage = new PdfPage();
            doc.Pages.Add(oPage);
            XGraphics xgr = XGraphics.FromPdfPage(oPage);
            XImage img = XImage.FromFile("receipt.bmp");
            xgr.DrawImage(img, 0, 0);
            using (Stream fileStream = File.Create(pdfFileName))
            {
                doc.Save(fileStream);
            }


            //email
            string file = "receipt.pdf";
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "ks.studilina@gmail.com",
                    Password = "1112522kO"
                }
            };
            MailAddress FromEmail = new MailAddress("ks.studilina@gmail.com", "Bank");
            MailAddress ToEmail = new MailAddress(toEmail, "Customer"); //change cust email

            MailMessage mess = new MailMessage(
                "ks.studilina@gmail.com",
                toEmail, //change cust email
                "Transaction receipt from " + currentTans.Date.ToShortDateString(),
                "Please see the attached receipt.\nThank you,\nBank");

            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);

            mess.Attachments.Add(data);

            try
            {
                client.Send(mess);
                MessageBox.Show("Receipt was sent", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                    ex.ToString());
            }
        }

        private void btSendByEmail_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.login.User.Email == null)
            {
                EmailDialog emailDlg = new EmailDialog();
                emailDlg.Owner = this;
                bool? result = emailDlg.ShowDialog();

                if (result == true)
                {
                    EmailReceipt(Utils.emailForReceipts);
                }
            }
            else
            {
                MessageBoxResult answer = CustomMessageBox.ShowYesNo("Send receipt to " + Utils.login.User.Email, "Confirmation required",
                "Yes", "Enter another e-mail", MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    EmailReceipt(Utils.login.User.Email);
                }
                if (answer == MessageBoxResult.No)
                {
                    EmailDialog emailDlg = new EmailDialog();
                    emailDlg.Owner = this;
                    bool? result = emailDlg.ShowDialog();

                    if (result == true)
                    {
                        EmailReceipt(Utils.emailForReceipts);
                    }
                }
            }
        }

        private void btPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog().GetValueOrDefault(false))
            {
                this.btPrint.Visibility = Visibility.Hidden;
                this.btSendByEmail.Visibility = Visibility.Hidden;
                this.btOk.Visibility = Visibility.Hidden;
                printDialog.PrintVisual(this, this.Title);
            }
            this.btPrint.Visibility = Visibility.Visible;
            this.btSendByEmail.Visibility = Visibility.Visible;
            this.btOk.Visibility = Visibility.Visible;
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }
    }
}
