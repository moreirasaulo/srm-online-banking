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

        private void btFind_Click(object sender, RoutedEventArgs e)
        {
            string natId = tbSearchByNatId.Text;

            var customers = from cust in EFData.context.Users
                            where cust.City == "London"
                            select cust;
            if (user != null)
            {
            }
    }
}
