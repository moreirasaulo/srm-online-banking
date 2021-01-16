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
            Utilities.mainWindow = this;
        }

        private void btLoginClicked(object sender, RoutedEventArgs e)
        {
            string username = tbAdminUsername.Text;
            string password = pbAdminPassword.Password;

            if (tbAdminUsername.Text.Length == 0 || pbAdminPassword.Password.Length == 0)
            {
                MessageBox.Show("'Username' and 'password' fields cannot be empty.", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                HightlightFields();
                return;
            }

            try
            {
                Utilities.login = EFData.context.Logins.FirstOrDefault(l => l.Username == username && l.Password == password);
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error loading from Database", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (Utilities.login != null && Utilities.login.UserTypeId == 2)
            {
                MessageBox.Show("Login successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                AdminDashboard adminDashDlg = new AdminDashboard();
                Hide(); // this window
                adminDashDlg.Owner = this;
                bool? result = adminDashDlg.ShowDialog();
                if (result == true)
                {
                    Show(); //this window
                    tbAdminUsername.Text = "";
                    pbAdminPassword.Password = "";
                }
            }
            else if (Utilities.login != null && Utilities.login.UserTypeId !=2)
            {
                MessageBox.Show("Login failed, you do not have rights to log in as an agent.", "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Login failed, incorrect username or password.", "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);
                HightlightFields();

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

        private void HightlightFields()
        {
            tbAdminUsername.BorderBrush = System.Windows.Media.Brushes.Red;
            pbAdminPassword.BorderBrush = System.Windows.Media.Brushes.Red;
            tbAdminUsername.BorderThickness = new Thickness(1, 1, 1, 3);
            pbAdminPassword.BorderThickness = new Thickness(1, 1, 1, 3);
        }

        private void RemoveHightlightFromFields()
        {
            tbAdminUsername.BorderBrush = null;
            pbAdminPassword.BorderBrush = null;
            tbAdminUsername.BorderThickness = new Thickness(0, 0, 0, 0);
            pbAdminPassword.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void tbAdminUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbAdminUsername.Text.Length > 0)
            {
                RemoveHightlightFromFields();
            }
        }

        private void pbAdminPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pbAdminPassword.Password.Length > 0)
            {
                RemoveHightlightFromFields();
            }
        }
    }
}
