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
    /// Interaction logic for ManageExistingCustomers.xaml
    /// </summary>
    public partial class ManageExistingCustomers : Window
    {
        List<User> customers = new List<User>();

        public ManageExistingCustomers()
        {
            InitializeComponent();
        }
        private void btFind_Click(object sender, RoutedEventArgs e)
        {
            LoadFoundCustomers();
            tbSearchCustBy.Text = "";
            lblEmptyResult.Content = "";
           
        }

        private void LoadFoundCustomers()
        {
            string searchInfo = tbSearchCustBy.Text;

            try
            {
                if (rbNatId.IsChecked == true)
                {
                    customers = EFData.context.Users.Where(cust => cust.NationalId == searchInfo).ToList();  //FIX EXCEPTIONS
                }
                else if (rbAccNo.IsChecked == true)
                {
                    int searchInfoAccNo;

                    try
                    {
                        searchInfoAccNo = Int32.Parse(searchInfo);
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show("Please eneter correct account number (just digits)");
                        return;
                    }
                    customers = (from cust in EFData.context.Users
                                 join acc in EFData.context.Accounts on cust.Id equals acc.UserId
                                 where acc.Id == searchInfoAccNo
                                 select cust).ToList();

                }
                else if (rbLastName.IsChecked == true)
                {
                    customers = EFData.context.Users.Where(cust => cust.LastName.Contains(searchInfo)).ToList();
                }
                else
                {
                    MessageBox.Show("Please select one search criteria");
                    return;
                }
                lvCustomers.ItemsSource = customers;
                if (customers.Count() == 0)
                {
                    lblEmptyResult.Content = "No customers that satisfy search criteria found";

                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Error fetching from database: " + ex.Message);
            }
            
        }

        private void btBackToDashClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btViewCustProfileClicked(object sender, RoutedEventArgs e)
        {
            if(lvCustomers.Items.Count == 0 || lvCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("No customer is selected to view profile");
                return;
            }
            User selectedUser = (User)lvCustomers.SelectedItem;
            ViewCustomerProfile viewCustProfileDlg = new ViewCustomerProfile(selectedUser);
            viewCustProfileDlg.Owner = this;
            bool? result = viewCustProfileDlg.ShowDialog();
            if(result == true)
            {
               //FIX   LoadFoundCustomers();  //reload customers after updating
            }
        }
    }
}
