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
            selectedMenuItem = "Home";
            HightlightSelectedMenuItem(selectedMenuItem);
        }

        private void HightlightSelectedMenuItem(string selectedItem)
        {
            rectAddClient.Visibility = Visibility.Hidden;
            rectViewUpdateClient.Visibility = Visibility.Hidden;
            rectManageAccounts.Visibility = Visibility.Hidden;
            rectLogOut.Visibility = Visibility.Hidden;
            rectExit.Visibility = Visibility.Hidden;
            miHome.BorderBrush = null;
            miHome.BorderThickness = new Thickness(0, 0, 0, 0);
            miManageAccounts.BorderBrush = null;
            miManageAccounts.BorderThickness = new Thickness(0, 0, 0, 0);
            miUpdateClient.BorderBrush = null;
            miUpdateClient.BorderThickness = new Thickness(0, 0, 0, 0);
            miLogOut.BorderBrush = null;
            miLogOut.BorderThickness = new Thickness(0, 0, 0, 0);
            miExit.BorderBrush = null;
            miExit.BorderThickness = new Thickness(0, 0, 0, 0);


            if (selectedItem == "Home")
            {
                rectAddClient.Visibility = Visibility.Visible;
                miHome.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miHome.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
            }
            if (selectedItem == "ManageClients")
            {
                rectViewUpdateClient.Visibility = Visibility.Visible;
                miUpdateClient.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miUpdateClient.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
            }
            if (selectedItem == "ManageAccounts")
            {
                rectManageAccounts.Visibility = Visibility.Visible;
                miManageAccounts.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00695C"));
                miManageAccounts.BorderThickness = new Thickness(0, 2.5, 0, 2.5);
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




        private void miManageAccounts_Click(object sender, RoutedEventArgs e)
        {
            selectedMenuItem = "ManageAccounts";
            HightlightSelectedMenuItem(selectedMenuItem);
            manAccscontrol = new ManageAccounts();
            this.contentControl.Content = manAccscontrol;
        }


        private void miUpdateClient_Click(object sender, RoutedEventArgs e)
        {
            selectedMenuItem = "ManageClients";
            HightlightSelectedMenuItem(selectedMenuItem);
            this.contentControl.Content = new UpdateCustomer();
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
                    selectedMenuItem = "Home";
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
                    selectedMenuItem = "Home";
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
            Utilities.mainWindow.Show();
        }

        private void miHome_Click(object sender, RoutedEventArgs e)
        {
            HightlightSelectedMenuItem("Home");
            this.contentControl.Content = new JABMSImage();
        }
    }
}
