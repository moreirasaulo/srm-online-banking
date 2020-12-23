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
        public ClientDashboard()
        {
            InitializeComponent();
            lblLoggedAs.Content = string.Format("Logged as {0} {1}", Utils.login.User.FirstName,
                Utils.login.User.LastName);

        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure you would like to logout?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes) 
            {
                Utils.login = null;
                Close();
            }            
        }

        private void btViewTransactions_Click(object sender, RoutedEventArgs e)
        {
          /*  string client = lblLoggedAs.Content as string;
            try
            {
                string client1 = client.Substring(10, client.Length - 1);// not working, client1 = null
            }
            catch (ArgumentOutOfRangeException ex) 
            {
                MessageBox.Show(ex.Message);
            }  */
            
            ViewTransactions transactions = new ViewTransactions();
            transactions.ShowDialog();
        }
    }
}
