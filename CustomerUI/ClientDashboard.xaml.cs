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
using System.Windows.Shapes;

namespace CustomerUI
{
    /// <summary>
    /// Interaction logic for ClientDashboard.xaml
    /// </summary>
    public partial class ClientDashboard : Window
    {
        ManageAccounts manageAccs;
        string selectedMenuItem;
        public ClientDashboard()
        {
            InitializeComponent();
            Utils.clientDashboard = this;
            lblLoggedAs.Content = Utils.login.User.FullName;
            manageAccs = new ManageAccounts();
            this.contentControl.Content = manageAccs;
            selectedMenuItem = "ManageAccounts";
            HightlightSelectedMenuItem(selectedMenuItem);

        }

        private void HightlightSelectedMenuItem(string selectedItem)
        {
            rectManageAccs.Visibility = Visibility.Hidden;
            rectProfile.Visibility = Visibility.Hidden;
            rectLogOut.Visibility = Visibility.Hidden;
            rectExit.Visibility = Visibility.Hidden;
            miManageAccounts.BorderBrush = null;
            miManageAccounts.BorderThickness = new Thickness(0, 0, 0, 0);
            miProfile.BorderBrush = null;
            miProfile.BorderThickness = new Thickness(0, 0, 0, 0);
            miLogOut.BorderBrush = null;
            miLogOut.BorderThickness = new Thickness(0, 0, 0, 0);
            miExit.BorderBrush = null;
            miExit.BorderThickness = new Thickness(0, 0, 0, 0);

            
            if (selectedItem == "ManageAccounts")
            {
                rectManageAccs.Visibility = Visibility.Visible;
                miManageAccounts.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miManageAccounts.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
            }
            if (selectedItem == "Profile")
            {
                rectProfile.Visibility = Visibility.Visible;
                miProfile.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miProfile.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
            }
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


        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem("Exit");
            MessageBoxResult result = MessageBox.Show("Are you sure you would like to exit?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
            if (result == MessageBoxResult.No)
            {
                if (this.contentControl.Content == null)
                {
                    this.contentControl.Content = new ManageAccounts();
                    selectedMenuItem = "ManageAccounts";
                    HightlightSelectedMenuItem(selectedMenuItem);
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
            MessageBoxResult result = MessageBox.Show("Are you sure you would like to log out?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Utils.clientDashboard = null;
                Utils.login = null;
                Utils.selectedAcc = null;
                Utils.userTransactions = null;
                DialogResult = true;
            }
            if (result == MessageBoxResult.No)
            {
                if (this.contentControl.Content == null)
                {
                    this.contentControl.Content = new ManageAccounts();
                    selectedMenuItem = "ManageAccounts";
                    HightlightSelectedMenuItem(selectedMenuItem);
                }
                else
                {
                    HightlightSelectedMenuItem(selectedMenuItem);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Utils.mainWindow.Show();
        }

        private void miManageAccounts_Click(object sender, RoutedEventArgs e)
        {
            Utils.selectedAcc = null;
            Utils.userTransactions = null;
            manageAccs = new ManageAccounts();
            this.contentControl.Content = manageAccs;
            selectedMenuItem = "ManageAccounts";
            HightlightSelectedMenuItem(selectedMenuItem);
        }

        private void miProfile_Click(object sender, RoutedEventArgs e)
        {
            Utils.selectedAcc = null;
            Utils.userTransactions = null;
            this.contentControl.Content = new CustProfile();
            selectedMenuItem = "Profile";
            HightlightSelectedMenuItem(selectedMenuItem);
        }

       
    }
}
