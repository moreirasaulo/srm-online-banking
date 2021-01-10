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
using System.Xaml;

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
            Utils.mainWindow = this;
        }

        private void btLoginClicked(object sender, RoutedEventArgs e)
        {
            string username = tbClientUsername.Text;
            string password = pbClientPassword.Password;
            
            Utils.login = EFData.context.Logins.Include("User").SingleOrDefault(l => l.Username == username && l.Password == password);     //FIX exception     
            
            if (Utils.login != null)
            {
                if (Utils.login.UserTypeId == 3)
                {
                    MessageBox.Show("Login successful");
                    ClientDashboard client = new ClientDashboard();
                    Hide(); // this window
                    tbClientUsername.Text = "";
                    pbClientPassword.Password = "";
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

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();            
        }
    }
}
