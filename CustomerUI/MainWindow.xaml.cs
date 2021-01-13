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

namespace CustomerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Utils.mainWindow = this;
        }

        private void btLoginClicked(object sender, RoutedEventArgs e)
        {
            string username = tbClientUsername.Text;
            string password = pbClientPassword.Password;

            if (tbClientUsername.Text.Length == 0 || pbClientPassword.Password.Length == 0)
            {
                MessageBox.Show("Username and password cannot be empty", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                HightlightFields();
                return;
            }

            try
            {
                Utils.login = EFData.context.Logins.Include("User").SingleOrDefault(l => l.Username == username && l.Password == password);
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error loading from Database", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Utils.login != null && (Utils.login.UserTypeId == 3 || Utils.login.UserTypeId == 2))
            {
                MessageBox.Show("Login successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClientDashboard clientDlg = new ClientDashboard();
                Hide(); // this window
                clientDlg.Owner = this;
                bool? result = clientDlg.ShowDialog();
                if (result == true)
                {
                    tbClientUsername.Text = "";
                    pbClientPassword.Password = "";
                    Show(); //this window
                }

            }
            else
            {
                MessageBox.Show("Login failed, incorrect username or password", "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);
                HightlightFields();

            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);           
        }

        private void HightlightFields()
        {
            tbClientUsername.BorderBrush = System.Windows.Media.Brushes.Red;
            pbClientPassword.BorderBrush = System.Windows.Media.Brushes.Red;
            tbClientUsername.BorderThickness = new Thickness(1, 1, 1, 3);
            pbClientPassword.BorderThickness = new Thickness(1, 1, 1, 3);
        }

        private void RemoveHightlightFromFields()
        {
            tbClientUsername.BorderBrush = null;
            pbClientPassword.BorderBrush = null;
            tbClientUsername.BorderThickness = new Thickness(0, 0, 0, 0);
            pbClientPassword.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void tbClientUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbClientUsername.Text.Length > 0)
            {
                RemoveHightlightFromFields();
            }
        }

        private void pbClientPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pbClientPassword.Password.Length > 0)
            {
                RemoveHightlightFromFields();
            }
        }
    }
}
