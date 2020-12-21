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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void btLoginClicked(object sender, RoutedEventArgs e)
        {
            string username = tbAdminUsername.Text;
            string password = pbAdminPassword.Password;

            var user = EFData.context.Logins.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                if ((user.UserType).ToLower() == "admin")
                {
                    MessageBox.Show("Login successful");
                    AdminDashboard adminDashDlg = new AdminDashboard();
                    adminDashDlg.Owner = this;
                    adminDashDlg.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Login failed, incorrect login or password");
                }
            }
            else
            {
                MessageBox.Show("Login failed, this username does not exist");
            }
        }







        /*   private bool VerifyUser(string username, string password)
           {
              string query =  "SELECT UserType FROM Users WHERE Username ='" + username + "' AND Password = '" + password + "'";
               string reader;
               if(reader )
           }  */
    }
}
