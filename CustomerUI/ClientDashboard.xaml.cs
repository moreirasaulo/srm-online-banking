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
                miManageAccounts.BorderBrush = System.Windows.Media.Brushes.DarkGreen;
                miManageAccounts.BorderThickness = new Thickness(0, 4, 0, 4);
            }
            if (selectedItem == "Profile")
            {
                rectProfile.Visibility = Visibility.Visible;
                miProfile.BorderBrush = System.Windows.Media.Brushes.DarkGreen;
                miProfile.BorderThickness = new Thickness(0, 4, 0, 4);
            }
            if (selectedItem == "LogOut")
            {
                rectLogOut.Visibility = Visibility.Visible;
                miLogOut.BorderBrush = System.Windows.Media.Brushes.DarkGreen;
                miLogOut.BorderThickness = new Thickness(0, 4, 0, 4);
            }
            if (selectedItem == "Exit")
            {
                rectExit.Visibility = Visibility.Visible;
                miExit.BorderBrush = System.Windows.Media.Brushes.DarkGreen;
                miExit.BorderThickness = new Thickness(0, 4, 0, 4);
            }

            /*  miManageAccounts.Foreground = System.Windows.Media.Brushes.White;
              miProfile.Foreground = System.Windows.Media.Brushes.White;
              miLogOut.Foreground = System.Windows.Media.Brushes.White;
              miExit.Foreground = System.Windows.Media.Brushes.White;
              if (selectedItem != null)
              {
                  selectedItem.Background = System.Windows.Media.Brushes.White;
                  selectedItem.Foreground = System.Windows.Media.Brushes.MediumSeaGreen;
              } */
        }


        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem("Exit");
            MessageBoxResult result = MessageBox.Show("Exit program?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
            MessageBoxResult result = MessageBox.Show("Log out of program?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
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
