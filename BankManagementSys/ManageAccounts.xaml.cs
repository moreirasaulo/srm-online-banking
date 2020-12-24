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
    /// Interaction logic for ManageAccounts.xaml
    /// </summary>
    public partial class ManageAccounts : Window
    {
        List<User> customers = null;

        public ManageAccounts()
        {
            InitializeComponent();
        }

        private void LoadFoundCustomers()
        {
            string searchInfo = tbSearchCustBy.Text;

            try
            {
                if (rbNatId.IsChecked == true)
                {
                    customers = EFData.context.Users.Include("Accounts").Where(cust => cust.NationalId == searchInfo).ToList();  //FIX EXCEPTIONS
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
                        MessageBox.Show("Please enter correct account number (just digits)");
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
                if (lvCustomers.Items.Count == 0)
                {
                    lblCustNotFound.Content = "No customers that satisfy search criteria found";
                    return;
                }
               
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Error fetching from database: " + ex.Message);
            }

        }

        private void btFind_Click(object sender, RoutedEventArgs e)
        {
            LoadFoundCustomers();
            tbSearchCustBy.Text = "";
            lblCustNotFound.Content = "";
        }

        private void lvCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvCustomers.Items.Count != 0 && lvCustomers.SelectedIndex != -1)
            {
                User selectedUser = (User)lvCustomers.SelectedItem;
                lvAccounts.ItemsSource = selectedUser.Accounts;
            }
        }

        private void btBackToDash_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btViewAccInfo_Click(object sender, RoutedEventArgs e)
        {
            if (lvCustomers.Items.Count == 0 && lvCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("First choose a customer to view accounts");
                return;
            }
            if (lvAccounts.Items.Count == 0 || lvAccounts.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose an account to view");
                return;
            }
            User selectedUser = (User)lvCustomers.SelectedItem;
            Account selectedAcc = (Account)lvAccounts.SelectedItem;
            ViewAccountInfo viewAccInfoDlg = new ViewAccountInfo(selectedUser,selectedAcc);
            viewAccInfoDlg.Owner = this;
            viewAccInfoDlg.ShowDialog();
        }
    }
}
