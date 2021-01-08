using SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                    catch (FormatException)
                    {
                        MessageBox.Show("Please enter correct account number (digits only).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            }
            catch (SystemException ex)
            {
                MessageBox.Show("Error fetching from database: " + ex.Message);
            }

        }

        private void LoadFoundAccounts()
        {
            User selectedUser = (User)lvCustomers.SelectedItem;
            List<Account> accounts = EFData.context.Accounts.Where(a => a.UserId.Equals(selectedUser.Id)).ToList();
            lvAccounts.ItemsSource = accounts;
        }

        private void btFind_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearchCustBy.Text == "")
            {
                MessageBox.Show("The search input cannot be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadFoundCustomers();
            tbSearchCustBy.Text = "";
            lblCustNotFound.Content = "";

            if (lvCustomers.Items.Count == 0)
            {
                lblCustNotFound.Content = "No customers that satisfy your search criteria was found.";
                return;
            }
        }

        private void lvCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvCustomers.Items.Count != 0 && lvCustomers.SelectedIndex != -1)
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
            ViewAccountInfo viewAccInfoDlg = new ViewAccountInfo(selectedUser, selectedAcc);
            viewAccInfoDlg.Owner = this;
            viewAccInfoDlg.ShowDialog();
        }

        private void lvAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btViewAccInfo.IsEnabled = true;
            btCloseAcct.IsEnabled = true;
            btStatement.IsEnabled = true;
            Account selectedAcc = (Account)lvAccounts.SelectedItem;
            if (lvAccounts.Items.Count == 0 || lvAccounts.SelectedIndex == -1 || selectedAcc.IsActive == false) 
            {
                btViewAccInfo.IsEnabled = false;
                btCloseAcct.IsEnabled = false;
                btStatement.IsEnabled = false;
            }
        }

        private void NumbersOnly(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.Enter:
                    break;
                default:
                    e.Handled = true;
                    lblErrorMsg.Content = "Only numbers are accepted for an account.";
                    break;
            }
        }

        private void tbSearchCustBy_KeyDown(object sender, KeyEventArgs e)
        {
            if (rbAccNo.IsChecked == true) 
            {
                NumbersOnly(e);
            }
            if (tbSearchCustBy.Text.Length == 0) 
            {
                lblErrorMsg.Content = "";
            }
        }

        private void btStatement_Click(object sender, RoutedEventArgs e)
        {
            Account currentAccount = (Account)lvAccounts.SelectedItem;
            GenerateStatement statementWindow = new GenerateStatement(currentAccount);
            statementWindow.Owner = this;
            statementWindow.ShowDialog();
        }


        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lvCustomers.Items.Count == 0 || lvCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("A customer must be selected first.", "Action required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            User selectedCust = (User)lvCustomers.SelectedItem;

            AddNewAccount addNewAcct = new AddNewAccount(selectedCust);
            addNewAcct.Owner = this;
            bool? result = addNewAcct.ShowDialog();
            if (result == true)
            {
                LoadFoundAccounts();
            }
        }

        private void btCloseAcct_Click(object sender, RoutedEventArgs e)
        {
            if(lvAccounts.Items.Count != 0 && lvAccounts.SelectedIndex != -1)
            {
                User selectedCust = (User)lvCustomers.SelectedItem;
                Account currentAccount = (Account)lvAccounts.SelectedItem;
                string closingAcctMessage = null;
                if(currentAccount.Balance > 0)
                {
                    closingAcctMessage = "Before closing account, withdraw remaining balance of $" + currentAccount.Balance +" Withdraw funds?";
                    MessageBoxResult result = MessageBox.Show(closingAcctMessage, "Withdrawal of funds required", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    Transaction transac;
                    if(result == MessageBoxResult.Yes)
                    {
                        transac = new Transaction();
                        transac.Date = DateTime.Now;
                        transac.Amount = currentAccount.Balance;
                        transac.Type = "Withdrawal";
                        transac.AccountId = currentAccount.Id;
                        decimal previousBalance = currentAccount.Balance; //balance before transaction
                        try
                        {
                            EFData.context.Transactions.Add(transac);
                            currentAccount.Balance = 0;  //new balance
                            EFData.context.SaveChanges();
                        }
                        catch (SystemException ex)
                        {
                            MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        Receipt withdrawalReceipt = new Receipt(currentAccount, previousBalance, transac, selectedCust, true);
                        withdrawalReceipt.Owner = this;
                        bool? dlgResult = withdrawalReceipt.ShowDialog();
                        if(dlgResult == true)
                        {
                            currentAccount.IsActive = false;
                            currentAccount.CloseDate = DateTime.Today;
                            try
                            {
                                EFData.context.SaveChanges();
                            }
                            catch (SystemException ex)
                            {
                                MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            //FIX: generate statement about closing account
                        }

                    }
                }
                
            }
        }
    }
}
