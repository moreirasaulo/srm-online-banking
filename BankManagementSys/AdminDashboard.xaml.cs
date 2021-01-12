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
using System.Windows.Shapes;

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        ManageAccounts manageAccs;
        public AdminDashboard()
        {
            InitializeComponent();
            lblUserName.Content = Utilities.login.User.FirstName + " (admin)";
            manageAccs = new ManageAccounts();
        }

        private void btManageCustomers_Click(object sender, RoutedEventArgs e)
        {
            ManageCustomers manageCustsDlg = new ManageCustomers();
            manageCustsDlg.Owner = this;
            manageCustsDlg.ShowDialog();
        }

        private void btManageAccounts_Click(object sender, RoutedEventArgs e)
        {
            ManageAccounts manageAccsDlg = new ManageAccounts();
            manageAccsDlg.Owner = this;
            manageAccsDlg.ShowDialog();
        }

        private void btValidationTest_Click(object sender, RoutedEventArgs e)
        {
            UserTypeValidationTest validationTestDlg = new UserTypeValidationTest();
            validationTestDlg.Owner = this;
            validationTestDlg.ShowDialog();
        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure you would like to logout?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                Utilities.login = null;
                Close();
            }
        }
    }
}
