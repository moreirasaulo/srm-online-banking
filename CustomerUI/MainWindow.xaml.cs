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
            string username = tbUsername.Text;
            string password = pbPassword.Password;

            var user = EFData.context.Logins.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                if ((user.UserType).ToLower() == "admin")
                {
                    MessageBox.Show("Login successful");
                }
                else
                {
                    MessageBox.Show("Login failed, incorrect login or password");
                    tbUsername.SelectAll();
                    tbUsername.BorderBrush = System.Windows.Media.Brushes.Red;
                    pbPassword.Password = "";
                    pbPassword.BorderBrush = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                MessageBox.Show("Login failed, this username does not exist");
                tbUsername.SelectAll();
                tbUsername.BorderBrush = System.Windows.Media.Brushes.Red;
                pbPassword.Password = "";
                pbPassword.BorderBrush = System.Windows.Media.Brushes.Red;
            }
        }

        
    }
}
