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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void btLoginClicked(object sender, RoutedEventArgs e)
        {
            string username = tbClientUsername.Text;
            string password = pbClientPassword.Password;

            
            Login login = EFData.context.Logins.SingleOrDefault(u => u.Username == username && u.Password == password);

            
            if (login != null)
            {
                if ((login.UserType).ToLower() == "client")
                {
                    MessageBox.Show("Login successful");
                    ClientDashboard client = new ClientDashboard(username);
                    client.Show();                   
                }
                else
                {
                    MessageBox.Show("Login failed, incorrect login or password");
                    tbClientUsername.SelectAll();
                    tbClientUsername.BorderBrush = System.Windows.Media.Brushes.Red;
                    pbClientPassword.Password = "";
                    pbClientPassword.BorderBrush = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                MessageBox.Show("Login failed, this username does not exist");
                tbClientUsername.SelectAll();
                tbClientUsername.BorderBrush = System.Windows.Media.Brushes.Red;
                pbClientPassword.Password = "";
                pbClientPassword.BorderBrush = System.Windows.Media.Brushes.Red;
            } 
        }
    }
}
