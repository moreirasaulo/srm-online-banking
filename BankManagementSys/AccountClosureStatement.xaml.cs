using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SharedCode;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for AccountClosureStatement.xaml
    /// </summary>
    public partial class AccountClosureStatement : Window
    {
        User currentUser;
        Account currentAccount;
        public AccountClosureStatement(Account account)
        {
            InitializeComponent();
            currentAccount = account;
            try
            {
                currentUser = EFData.context.Users.FirstOrDefault(u => u.Id == currentAccount.UserId);
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error loading from Database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (currentUser.Email != null && currentUser.Email.Length != 0)
            {
                btEmailStatement.IsEnabled = true;
            }
            lblAccNumber.Content = currentAccount.Id;
            lblAccHolder.Content = currentUser.FullName;
            lblAccType.Content = currentAccount.AccountType.Description;
            lblOpenDate.Content = currentAccount.OpenDate;
            lblClosedDate.Content = currentAccount.CloseDate;
            lblBalance.Content = currentAccount.Balance.ToString("0.00") + " $";
            lblPrintDate.Content = DateTime.Now.ToShortDateString();

        }

        private void btEmailStatement_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Would you like to send this receipt to " + currentUser.Email + " ?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                try
                {
                    //create bmp
                    int Width = (int)statementPanel.RenderSize.Width;
                    int Height = (int)statementPanel.RenderSize.Height;
                    string fileName = "receipt.bmp";
                    RenderTargetBitmap renderTargetBitmap =
                    new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32);
                    renderTargetBitmap.Render(statementPanel);
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
                    MailAddress FromEmail = new MailAddress("johnabbottbank@gmail.com", "Bank");
                    MailAddress ToEmail = new MailAddress(currentUser.Email, "Customer");

                    MailMessage mess = null;
                    if (currentUser.Gender == "male")
                    {
                        mess = new MailMessage(
                        "johnabbottbank@gmail.com",
                        currentUser.Email,
                        "Account closure statement",
                        "Dear Mr " + currentUser.LastName + ",\n\nPlease see the attached statement.\n\nThank you,\n\nJohn Abbott Bank");
                    }
                    else 
                    {
                        mess = new MailMessage(
                        "johnabbottbank@gmail.com",
                        currentUser.Email,
                        "Account closure statement",
                        "Dear Mrs " + currentUser.LastName + ",\n\nPlease see the attached statement.\n\nThank you,\n\nJohn Abbott Bank");
                    }                   

                    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);

                    mess.Attachments.Add(data);


                    client.Send(mess);
                    MessageBox.Show("The receipt was sent successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void btPrintStatement_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog(); 
            if (printDialog.ShowDialog().GetValueOrDefault(false))
            {
                this.btPrintStatement.Visibility = Visibility.Hidden;
                this.btEmailStatement.Visibility = Visibility.Hidden;
                this.btOk.Visibility = Visibility.Hidden;
                printDialog.PrintVisual(this, this.Title);
            }
            this.btPrintStatement.Visibility = Visibility.Visible;
            this.btEmailStatement.Visibility = Visibility.Visible;
            this.btOk.Visibility = Visibility.Visible;
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
