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
        UserControl currentControl;
        public Action<string> OnTitleChanged;

        public AdminDashboard()
        {
            InitializeComponent();
        }




        private void btValidationTest_Click(object sender, RoutedEventArgs e)
        {
            UserTypeValidationTest validationTestDlg = new UserTypeValidationTest();
            validationTestDlg.Owner = this;
            validationTestDlg.ShowDialog();
        }

        private void miManageAccounts_Click(object sender, RoutedEventArgs e)
        {
            currentControl = new ManageAccounts();
            this.contentControl.Content = currentControl;
            mainMenu.IsEnabled = false;
            mainMenu.Visibility = Visibility.Hidden;
            accountsMenu.IsEnabled = true;
            accountsMenu.Visibility = Visibility.Visible;
        }

        private void miAddClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miUpdateClient_Click(object sender, RoutedEventArgs e)
        {

        }



        private void miAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddClientDialog addNewClientDlg = new AddClientDialog();
            addNewClientDlg.Owner = this;
            addNewClientDlg.ShowDialog();
        }



        private void miViewUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new UpdateCustomer();
        }

        private void miNewAccount_Click(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new ManageAccounts();
        }


        //verified
        private void miBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = null;
            mainMenu.IsEnabled = true;
            mainMenu.Visibility = Visibility.Visible;
            accountsMenu.IsEnabled = false;
            accountsMenu.Visibility = Visibility.Hidden;
        }

        private void miCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
