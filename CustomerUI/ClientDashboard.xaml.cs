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
        public ClientDashboard(string client)
        {
            InitializeComponent();
            lblLoggedAs.Content = "Logged as " + client;
        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure you would like to logout?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes) 
            {
                Close();
            }            
        }

        private void btViewTransactions_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
