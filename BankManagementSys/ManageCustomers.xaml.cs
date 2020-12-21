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
            string searchCriteria = "";

            List<User> customers = new List<User>();

            if (rbNatId.IsChecked == true)
            {
                searchCriteria = "NationalId";
              /*  customers = (from cust in EFData.context.Users
                             where cust.NationalId == searchCriteria
                             select cust).ToList(); */

                customers = EFData.context.Users.Where(cust => cust.NationalId == searchCriteria).ToList();
            }
            else if (rbAccNo.IsChecked == true)
            {
                searchCriteria = "Id";
                customers = (from cust in EFData.context.Users
                             join acc in EFData.context.Accounts on acc.UserId equals cust.Id
                             where acc.Id == searchCriteria
                             select cust).ToList();
            }
            else if(rbLastName.IsChecked == true)
            {
                searchCriteria = "LastName";
                customers = (from cust in EFData.context.Users
                             where cust.LastName == searchCriteria
                             select cust).ToList();
            }
            else
            {
                MessageBox.Show("Please select one search criteria");
                return;
            }

            string searchInfo = tbSearchCustBy.Text;

            List<User> 
            lvCustomers.ItemsSource = customers;
            if(customers.Count() == 0)
            {
                lblEmptyResult.Content = "No customers that satisfy search criteria found";
            }
        }   
    }
}
