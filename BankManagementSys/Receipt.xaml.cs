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

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for Receipt.xaml
    /// </summary>
    public partial class Receipt : Window
    {
        User currentCust;
        Transaction currentTans;
        public Receipt(Account account, decimal oldBalance, Transaction transaction, User user, bool needOldBalance)
        {
            InitializeComponent();
            currentCust = user;
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
            if (user.Email == null || user.Email == "")
            {
                btSendByEmail.IsEnabled = false;
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
            lblTransId.Content = transaction.Id;
            lblAccHolder.Content = currentCust.FullName;
            lblAmount.Content = string.Format("$ {0:0.00}", transaction.Amount);
            lblNewBalance.Content = "$ " + account.Balance;
            lblAgentNo.Content = Utilities.login.User.Id;
            lblDate.Content = transaction.Date;
            lblPrintDate.Content = DateTime.Now;


        }

        private void btSendByEmail_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Send receipt to " + currentCust.Email + " ?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                try
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
                            UserName = "johnabbottbank@gmail.com",
                            Password = "querty123!"
                        }
                    };
                    MailAddress FromEmail = new MailAddress("johnabbottbank@gmail.com", "John Abbott Bank");
                    MailAddress ToEmail = new MailAddress(currentCust.Email, "Customer");

                    MailMessage mess = null;
                    if (currentCust.Gender == "male")
                    {
                        mess = new MailMessage(
                        "johnabbottbank@gmail.com",
                        currentCust.Email,
                        "Transaction receipt from " + currentTans.Date.ToShortDateString(),
                        "Dear Mr " + currentCust.LastName + ",\n\nPlease see the attached receipt.\n\nThank you,\n\nJohn Abbott Bank");
                    }
                    else if (currentCust.Gender == "female")
                    {
                        mess = new MailMessage(
                        "johnabbottbank@gmail.com",
                        currentCust.Email,
                        "Transaction receipt from " + currentTans.Date.ToShortDateString(),
                        "Dear Mrs " + currentCust.LastName + ",\n\nPlease see the attached receipt.\n\nThank you,\n\nJohn Abbott Bank");
                    }
                    else
                    {
                        mess = new MailMessage(
                        "johnabbottbank@gmail.com",
                        currentCust.Email,
                        "Transaction receipt from " + currentTans.Date.ToShortDateString(),
                        "Dear Mr/Mrs " + currentCust.LastName + ",\n\nPlease see the attached receipt.\n\nThank you,\n\nJohn Abbott Bank");
                    }

                    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);

                    mess.Attachments.Add(data);


                    client.Send(mess);
                    MessageBox.Show("Receipt was sent", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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
