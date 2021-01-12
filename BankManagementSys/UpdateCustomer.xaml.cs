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

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : UserControl
    {
        List<User> customers = new List<User>();
        public UpdateCustomer()
        {
            InitializeComponent();
        }

        private void btFind_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearchCustBy.Text.Length == 0)
            {
                MessageBox.Show("The search field cannot be empty.", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            LoadFoundCustomers();
            tbSearchCustBy.Text = "";
            lblEmptyResult.Content = "";
            if (customers.Count == 0)
            {
                lblEmptyResult.Content = "No customers that satisfy your search criteria was found.";
                return;
            }

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
                    catch (FormatException)
                    {
                        MessageBox.Show("Please eneter correct account number (only digits are allowed)");
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

       
            
           // viewCustProfileDlg.Owner = this;
           // bool? result = viewCustProfileDlg.ShowDialog();
           /* if (result == true)
            {
                //FIX   LoadFoundCustomers();  //reload customers after updating
            } */
        

        private void lvCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvCustomers.Items.Count == 0 || lvCustomers.SelectedIndex == -1)
            {
                this.contentControl.Content = null;
                return;
            }
            User selectedUser = (User)lvCustomers.SelectedItem;
            this.contentControl.Content = new ViewUpdateClientProfile(selectedUser);
        }
    }
}
