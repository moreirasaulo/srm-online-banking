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
        public AdminDashboard()
        {
            InitializeComponent();
           
        }

        private void btManageCustomers_Click(object sender, RoutedEventArgs e)
        {
            ManageCustomers manageCustsDlg = new ManageCustomers();
            manageCustsDlg.Owner = this;
            manageCustsDlg.ShowDialog();
        }

        private void btManageAccounts_Click(object sender, RoutedEventArgs e)
        {
            ManageAccounts manageAccsDlg = new ManageAccounts();
            manageAccsDlg.Owner = this;
            manageAccsDlg.ShowDialog();
        }

    }
}
