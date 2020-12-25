using PdfSharp;
using PdfSharp.Pdf;
using SharedCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for DepositReceipt.xaml
    /// </summary>
    public partial class DepositReceipt : Window
    {
        User currentCust;
        public DepositReceipt(Account account, decimal oldBalance, Transaction deposit, User user)
        {
            InitializeComponent();
            currentCust = user;
            if(user.Email == null)
            {
              //  btSendByEmail.IsEnabled = false;
            }
            lblAccNo.Content = account.Id;
            lblTransId.Content = deposit.Id;
            lblPreviousBalance.Content = oldBalance + " $";
            lblAmount.Content = deposit.Amount + " $";
            lblNewBalance.Content = account.Balance + " $";
            lblAgentNo.Content = Utilities.login.User.Id;
            lblDate.Content = deposit.Date;


        }

        private void btSendByEmail_Click(object sender, RoutedEventArgs e)
        {

            /*PrintDialog pd = new PrintDialog();
            pd.PrintQueue = new PrintQueue(new PrintServer(), "Adobe PDF");
            pd.PrintVisual(this, this.Title); */
            // create a document
            /*      FixedDocument document = new FixedDocument();
                  document.DocumentPaginator.PageSize = new Size(96 * 8.5, 96 * 11);
                  // create a page
                  FixedPage page1 = new FixedPage();
                  page1.Width = document.DocumentPaginator.PageSize.Width;
                  page1.Height = document.DocumentPaginator.PageSize.Height;
                  // add some text to the page
                  page1.Children.Add(grid);
                  // add the page to the document
                  PageContent page1Content = new PageContent();
                  ((IAddChild)page1Content).AddChild(page1);
                  document.Pages.Add(page1Content);

               /*   //to pdf
                  var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);
                  PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, d.FileName, 0);  */
            int Width = (int)grid.RenderSize.Width;
            int Height = (int)grid.RenderSize.Height;
            string fileName = "myCapture.bmp";
            RenderTargetBitmap renderTargetBitmap =
            new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(grid);
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (Stream fileStream = File.Create(fileName))
            {
                pngImage.Save(fileStream);
            }

            // Specify the file to be attached and sent.
            // This example assumes that a file named Data.xls exists in the
            // current working directory.
            string file = "myCapture.bmp";
        

            

            //Send the message.
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
            MailAddress FromEmail = new MailAddress("ks.studilina@gmail.com", "Viri");
            MailAddress ToEmail = new MailAddress("ks.studilina@gmail.com", "Someone");

            MailMessage mess = new MailMessage(
                "ks.studilina@gmail.com",
                "ks.studilina@gmail.com",
                "Quarterly data report.",
                "See the attached spreadsheet.");

            // Create  the file attachment for this email message.
            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);

            // Add the file attachment to this email message.
            mess.Attachments.Add(data);

            try
            {
                client.Send(mess);
                MessageBox.Show("Done");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                    ex.ToString());
            }






        }

           



        private void btPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();  //not win forms
            if (printDialog.ShowDialog().GetValueOrDefault(false))
            {
                this.btPrint.Visibility = Visibility.Hidden;
                this.btSendByEmail.Visibility = Visibility.Hidden;
                printDialog.PrintVisual(this, this.Title);
            }
            if(printDialog.ShowDialog() == false)
            {
                this.btPrint.Visibility = Visibility.Visible;
                this.btSendByEmail.Visibility = Visibility.Visible;
            }
        }
    }
}
