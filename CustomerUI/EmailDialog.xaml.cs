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
    /// Interaction logic for EmailDialog.xaml
    /// </summary>
    public partial class EmailDialog : Window
    {
        public EmailDialog()
        {
            InitializeComponent();
        }

        private void btSendReceipt_Click(object sender, RoutedEventArgs e)
        {
            if(!tbEmail.Text.Contains('@'))
            {
                MessageBox.Show("Please enter valid email address", "Invalid e-mail format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (tbEmail.Text.Length < 5)
            {
                MessageBox.Show("Email address is too short, it must containt at least 5 characters", "Invalid e-mail format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Utils.emailForReceipts = tbEmail.Text;
            DialogResult = true;
        }
    }
}
