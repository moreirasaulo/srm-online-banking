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
        public ClientDashboard()
        {
            InitializeComponent();
            lblLoggedAs.Content = string.Format("Logged as {0}", Utils.login.User.FullName);
            manageAccs = new ManageAccounts();
            this.contentControl.Content = manageAccs;

        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure you would like to logout?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes) 
            {
                Utils.login = null;
                Close();
                Utils.mainWindow.Show();
            }            
        }

        private void btViewSpendRep_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.selectedAcc == null)
            {
                MessageBox.Show("First select an account to view spending reports");
                return;
            }
            if (Utils.selectedAcc.AccountType.Description != "Checking")
            {
                MessageBox.Show("Spending reports are available only for checking accounts");
                return;
            }
            this.contentControl.Content = new SpendReport();
            btBack.Visibility = Visibility.Visible;
            btViewSpendRep.Visibility = Visibility.Hidden;
            btMyProfile.Visibility = Visibility.Hidden;
        }

        private void btMyProfile_Click(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new CustProfile();
            btBack.Visibility = Visibility.Visible;
            btViewSpendRep.Visibility = Visibility.Hidden;
            btMyProfile.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = manageAccs;
            btBack.Visibility = Visibility.Hidden;
            btViewSpendRep.Visibility = Visibility.Visible;
            btMyProfile.Visibility = Visibility.Visible;
        }
    }
}
