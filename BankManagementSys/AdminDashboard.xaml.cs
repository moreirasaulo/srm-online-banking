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
        ManageAccounts manAccscontrol;

        public AdminDashboard()
        {
            InitializeComponent();
            Utilities.adminDashboard = this;
            lblAgenName.Content = Utilities.login.User.FullName;
            SetStartingWinodw();
        }

        public void SetStartingWinodw()
        {
            this.contentControl.Content = new JABMSImage();
        }

        private void btValidationTest_Click(object sender, RoutedEventArgs e)
        {
            UserTypeValidationTest validationTestDlg = new UserTypeValidationTest();
            validationTestDlg.Owner = this;
            validationTestDlg.ShowDialog();
        }

        private void HightlightSelectedMenuItem(MenuItem selectedItem)
        {
            miAddClient.Background = null;
            miUpdateClient.Background = null;
            miManageAccounts.Background = null;
            miLogOut.Background = null;
            miExit.Background = null;
            if (selectedItem != null)
            {
                selectedItem.Background = SystemColors.ControlDarkBrush;
            }
        }


        private void miManageAccounts_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem(miManageAccounts);
            manAccscontrol = new ManageAccounts();
            this.contentControl.Content = manAccscontrol;
            mainMenu.IsEnabled = false;
            mainMenu.Visibility = Visibility.Hidden;
            accountsMenu.IsEnabled = true;
            accountsMenu.Visibility = Visibility.Visible;
        }

        //add client
        private void miAddClient_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem(miAddClient);
            this.contentControl.Content = new AddNewClient();
           /* if (result == true)
            {
                HightlightSelectedMenuItem(null);
                this.contentControl.Content = new JABMSImage();
            } */
        }

        private void miUpdateClient_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem(miUpdateClient);
            this.contentControl.Content = new UpdateCustomer();
        }

        private void miBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem(null);
            this.contentControl.Content = new JABMSImage();
            mainMenu.IsEnabled = true;
            mainMenu.Visibility = Visibility.Visible;
            accountsMenu.IsEnabled = false;
            accountsMenu.Visibility = Visibility.Hidden;
        }

        private void miCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            manAccscontrol.AddAccount();
           
        }

        private void miViewUpdateAccount_Click(object sender, RoutedEventArgs e)
        {
            manAccscontrol.ViewAccountIinfo();
        }

        private void miCloseAccount_Click(object sender, RoutedEventArgs e)
        {
            manAccscontrol.CloseAccount();
        }

        private void miStatement_Click(object sender, RoutedEventArgs e)
        {
            manAccscontrol.CreateStatement();
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem(miExit);
            MessageBoxResult result = MessageBox.Show("Exit program?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
            if(result == MessageBoxResult.No)
            {
                HightlightSelectedMenuItem(null);
                this.contentControl.Content = new JABMSImage();
            }
        }

        private void miLogOut_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem(miLogOut);
            MessageBoxResult result = MessageBox.Show("Log out of program?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Utilities.login = null;
                DialogResult = true;
            }
            if (result == MessageBoxResult.No)
            {
                HightlightSelectedMenuItem(null);
                this.contentControl.Content = new JABMSImage();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Utilities.mainWindow.Show();
        }
    }
}
