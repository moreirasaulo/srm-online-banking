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
using System.Drawing;

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
                lblPreviousBalance.Content = "$ " + oldBalance;
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
            lblAmount.Content = string.Format("$ {0:0.00}", transaction.Amount);
            lblNewBalance.Content = "$ " + account.Balance;
            lblDate.Content = transaction.Date;
            lblPrintDate.Content = DateTime.Now;
        }

        private void EmailReceipt(string toEmail)
        {
            try
            {
                string fileName = "receipt" + DateTime.Now.ToString("yyyyMMddhhmmss");
                
                //create bmp
                int Width = (int)receiptPanel.RenderSize.Width;
                int Height = (int)receiptPanel.RenderSize.Height;
                string bmpFileName = fileName + ".bmp";
                //string fileName = "receipt.bmp";
                RenderTargetBitmap renderTargetBitmap =
                new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(receiptPanel);
                PngBitmapEncoder pngImage = new PngBitmapEncoder();
                pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                using (Stream fileStream = File.Create(bmpFileName))
                {
                    pngImage.Save(fileStream);
                }

                //create pdf
                string pdfFileName = fileName + ".pdf";
                PdfDocument doc = new PdfDocument();
                PdfPage oPage = new PdfPage();
                doc.Pages.Add(oPage);
                XGraphics xgr = XGraphics.FromPdfPage(oPage);
                XImage img = XImage.FromFile(bmpFileName);
                xgr.DrawImage(img, 0, 0);
                using (Stream fileStream1 = File.Create(pdfFileName))
                {
                    doc.Save(fileStream1);
                }

                //email
               
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
                MailAddress ToEmail = new MailAddress(toEmail, "Customer");

                MailMessage mess = null;
                if (Utils.login.User.Gender == "male")
                {
                    mess = new MailMessage(
                    "johnabbottbank@gmail.com",
                    toEmail,
                    "Transaction receipt from " + currentTans.Date.ToShortDateString(),
                    "Dear Mr " + Utils.login.User.LastName + ",\n\nPlease see the attached receipt.\n\nThank you,\n\nJohn Abbott Bank");
                }
                else if (Utils.login.User.Gender == "female")
                {
                    mess = new MailMessage(
                    "johnabbottbank@gmail.com",
                    toEmail,
                    "Transaction receipt from " + currentTans.Date.ToShortDateString(),
                    "Dear Mrs " + Utils.login.User.LastName + ",\n\nPlease see the attached receipt.\n\nThank you,\n\nJohn Abbott Bank");
                }
                else
                {
                    mess = new MailMessage(
                    "johnabbottbank@gmail.com",
                    toEmail,
                    "Transaction receipt from " + currentTans.Date.ToShortDateString(),
                    "Dear Mr/Mrs " + Utils.login.User.LastName + ",\n\nPlease see the attached receipt.\n\nThank you,\n\nJohn Abbott Bank");
                }

                Attachment data = new Attachment(pdfFileName, MediaTypeNames.Application.Octet);

                mess.Attachments.Add(data);

                client.Send(mess);
              /*  var image = System.Drawing.Image.FromFile("receipt.bmp");
                pngImage.Dispose();
                File.Delete("receipt.bmp");

                doc.Dispose();
                File.Delete("receipt.pdf"); */
                MessageBox.Show("Receipt was sent successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

              /*  System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(bmpFileName);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
               // doc.Close();
               // doc.Dispose();
                File.Delete(pdfFileName);*/

            }
            catch (IOException ex)
            {
                MessageBox.Show("Error sending receipt: " + ex.Message, "Internal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SmtpException ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message, "Internal error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBoxResult answer = CustomMessageBox.ShowYesNo("Would you like to send this receipt to " + Utils.login.User.Email + "?", "Confirmation required",
                "Yes", "Enter another email", MessageBoxImage.Question);
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
