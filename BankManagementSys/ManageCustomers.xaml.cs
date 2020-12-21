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

            List<User> customers = new List<User>();

            string searchInfo = tbSearchCustBy.Text;

            if (rbNatId.IsChecked == true)
            {
                customers = EFData.context.Users.Where(cust => cust.NationalId == searchInfo).ToList();
            }
            else if (rbAccNo.IsChecked == true)
            {
                int searchInfoAccNo;

                try
                {
                    searchInfoAccNo = Int32.Parse(searchInfo);
                }
                catch(FormatException ex)
                {
                    MessageBox.Show("Please eneter correct account number (just digits)");
                    return;
                }
                customers = (from cust in EFData.context.Users
                             join acc in EFData.context.Accounts on cust.Id equals acc.UserId
                             where acc.Id == searchInfoAccNo
                             select cust).ToList();

            }
            else if(rbLastName.IsChecked == true)
            {
                customers = EFData.context.Users.Where(cust => cust.LastName == searchInfo).ToList();
            }
            else
            {
                MessageBox.Show("Please select one search criteria");
                return;
            }

            tbSearchCustBy.Text = "";
            lvCustomers.ItemsSource = customers;
            lblEmptyResult.Content = "";
            if (customers.Count() == 0)
            {
                lblEmptyResult.Content = "No customers that satisfy search criteria found";
               
            }
        }   
    }
}
