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

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for ManageCustomers.xaml
    /// </summary>
    public partial class ManageCustomers : Window
    {
        public ManageCustomers()
        {
            InitializeComponent();
        }

       

        private void btBackToDashClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddClientDialog addNewClientDlg = new AddClientDialog();
            addNewClientDlg.Owner = this;
            addNewClientDlg.ShowDialog();

        }

        private void btManageExistingCustomers_Click(object sender, RoutedEventArgs e)
        {
            ManageExistingCustomers dlgManageExistCust = new ManageExistingCustomers();
            dlgManageExistCust.Owner = this;
            dlgManageExistCust.ShowDialog();
        }
    }
}
