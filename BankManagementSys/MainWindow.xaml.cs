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

            try
            {
                Utilities.login = EFData.context.Logins.FirstOrDefault(l => l.Username == username && l.Password == password); //FIX exception
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error loading from Database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            /* same:
            Login login1 = (from l in EFData.context.Logins
                           where l.Username == username && l.Password == password
                           select l).FirstOrDefault();
            */

            if (Utilities.login != null)
            {
                if (Utilities.login.UserTypeId == 2)
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

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
