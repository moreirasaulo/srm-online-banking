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
        string selectedMenuItem;

        public AdminDashboard()
        {
            InitializeComponent();
            Utilities.adminDashboard = this;
            lblAgenName.Content = Utilities.login.User.FullName;
            this.contentControl.Content = new JABMSImage();
            selectedMenuItem = "";
            HightlightSelectedMenuItem(selectedMenuItem);
        }

        public void SetStartingWindow()
        {
            this.contentControl.Content = new JABMSImage();
            HightlightSelectedMenuItem("");
        }

        private void HightlightSelectedMenuItem(string selectedItem)
        {
            rectAddClient.Visibility = Visibility.Hidden;
            rectViewUpdateClient.Visibility = Visibility.Hidden;
            rectLogOut.Visibility = Visibility.Hidden;
            rectExit.Visibility = Visibility.Hidden;
            miAddClient.BorderBrush = null;
            miAddClient.BorderThickness = new Thickness(0, 0, 0, 0);
            miUpdateClient.BorderBrush = null;
            miUpdateClient.BorderThickness = new Thickness(0, 0, 0, 0);
            miLogOut.BorderBrush = null;
            miLogOut.BorderThickness = new Thickness(0, 0, 0, 0);
            miExit.BorderBrush = null;
            miExit.BorderThickness = new Thickness(0, 0, 0, 0);


            if (selectedItem == "AddClient")
            {
                rectAddClient.Visibility = Visibility.Visible;
                miAddClient.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miAddClient.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
            }
            if (selectedItem == "ViewUpdateClient")
            {
                rectViewUpdateClient.Visibility = Visibility.Visible;
                miUpdateClient.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miUpdateClient.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
            }
          /*  if (selectedItem == "ManageAccounts")
            {
                rectManageAccs.Visibility = Visibility.Visible;
                miManageAccounts.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miManageAccounts.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
            } */
            if (selectedItem == "LogOut")
            {
                rectLogOut.Visibility = Visibility.Visible;
                miLogOut.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miLogOut.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
            }
            if (selectedItem == "Exit")
            {
                rectExit.Visibility = Visibility.Visible;
                miExit.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miExit.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
            }
        }


        private void btValidationTest_Click(object sender, RoutedEventArgs e)
        {
            UserTypeValidationTest validationTestDlg = new UserTypeValidationTest();
            validationTestDlg.Owner = this;
            validationTestDlg.ShowDialog();
        }


        private void miManageAccounts_Click(object sender, RoutedEventArgs e)
        {
            selectedMenuItem = "ManageAccounts";
            HightlightSelectedMenuItem(selectedMenuItem);
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
            selectedMenuItem = "AddClient";
            HightlightSelectedMenuItem(selectedMenuItem);
            this.contentControl.Content = new AddNewClient();
           /* if (result == true)
            {
                HightlightSelectedMenuItem(null);
                this.contentControl.Content = new JABMSImage();
            } */
        }

        private void miUpdateClient_Click(object sender, RoutedEventArgs e)
        {
            selectedMenuItem = "ViewUpdateClient";
            HightlightSelectedMenuItem(selectedMenuItem);
            this.contentControl.Content = new UpdateCustomer();
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
            HightlightSelectedMenuItem("Exit");
            MessageBoxResult result = MessageBox.Show("Exit program?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
            if(result == MessageBoxResult.No)
            {
                if (this.contentControl.Content == null)
                {
                    this.contentControl.Content = new JABMSImage();
                    HightlightSelectedMenuItem("");
                }
                else
                {
                    HightlightSelectedMenuItem(selectedMenuItem);
                }
            }
        }

        private void miLogOut_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem("LogOut");
            MessageBoxResult result = MessageBox.Show("Are you sure you wish to log out?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Utilities.login = null;
                DialogResult = true;
            }
            if (result == MessageBoxResult.No)
            {
                if (this.contentControl.Content == null)
                {
                    this.contentControl.Content = new JABMSImage();
                    HightlightSelectedMenuItem("");
                }
                else
                {
                    HightlightSelectedMenuItem(selectedMenuItem);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Utilities.mainWindow.Show();
        }

        private void miBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new JABMSImage();
            mainMenu.IsEnabled = true;
            mainMenu.Visibility = Visibility.Visible;
            accountsMenu.IsEnabled = false;
            accountsMenu.Visibility = Visibility.Hidden;
            HightlightSelectedMenuItem("");
        }
    }
}
