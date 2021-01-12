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
    /// Interaction logic for ManageAccounts.xaml
    /// </summary>
    public partial class ManageAccounts : UserControl
    {
        List<User> customers = null;
        User currentClient;
        Account currentAccount;
        public ManageAccounts()
        {
            InitializeComponent();
           
            /*  operation = action;
              if(operation == "New account")
              {
                  lvAccounts.Visibility = Visibility.Hidden;
                  HideButtons();
                  btAdd.Visibility = Visibility.Visible;
              } */
        }

        private void HideButtons()
        {
            btViewAccInfo.Visibility = Visibility.Hidden;
            btCloseAcct.Visibility = Visibility.Hidden;
            btAdd.Visibility = Visibility.Hidden;
            btDeposit.Visibility = Visibility.Hidden;
            btWithdrawal.Visibility = Visibility.Hidden;
            btTransfer.Visibility = Visibility.Hidden;
            btPayment.Visibility = Visibility.Hidden;
            btStatement.Visibility = Visibility.Hidden;
            lblCustAccounts.Visibility = Visibility.Hidden;
        }

        private void ShowButtons()
        {
            btViewAccInfo.Visibility = Visibility.Visible;
            btCloseAcct.Visibility = Visibility.Visible;
            btAdd.Visibility = Visibility.Visible;
            btDeposit.Visibility = Visibility.Visible;
            btWithdrawal.Visibility = Visibility.Visible;
            btTransfer.Visibility = Visibility.Visible;
            btPayment.Visibility = Visibility.Visible;
            btStatement.Visibility = Visibility.Visible;
            lblCustAccounts.Visibility = Visibility.Visible;
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
            List<Account> accounts = EFData.context.Accounts.Where(a => a.UserId.Equals(currentClient.Id)).ToList();
            lvAccounts.ItemsSource = accounts;
        }

        private void btFind_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearchCustBy.Text.Length == 0)
            {
                MessageBox.Show("The search field cannot be empty", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadFoundCustomers();
            tbSearchCustBy.Text = "";
            lblCustNotFound.Content = "";

            if (lvCustomers.Items.Count == 0)
            {
                lblCustNotFound.Content = "No customers that satisfy your search criteria was found";
                return;
            }
        }

        private void lvCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvCustomers.Items.Count == 0 || lvCustomers.SelectedIndex == -1)
            {
                lvAccounts.ItemsSource = new List<User>();
                return;
            }
            currentClient = (User)lvCustomers.SelectedItem;
            LoadFoundAccounts();
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
            ViewAccountInfo viewAccInfoDlg = new ViewAccountInfo(currentClient, currentAccount);
            // viewAccInfoDlg.Owner = this;
            bool? result = viewAccInfoDlg.ShowDialog();
            if (result == true)
            {
                LoadFoundAccounts();
            }
        }

        private void lvAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvAccounts.Items.Count != 0 && lvAccounts.SelectedIndex != -1)
            {
                currentAccount = (Account)lvAccounts.SelectedItem;
                btViewAccInfo.IsEnabled = true;
                btCloseAcct.IsEnabled = true;
                btStatement.IsEnabled = true;
                btDeposit.IsEnabled = true;
                btWithdrawal.IsEnabled = true;
                btTransfer.IsEnabled = true;
                btPayment.IsEnabled = true;
                if (currentAccount.AccountType.Description == "Savings")
                {
                    btPayment.IsEnabled = false;
                }
                if (currentAccount.AccountType.Description == "Investment")
                {
                    btTransfer.IsEnabled = false;
                    btPayment.IsEnabled = false;
                }
                if (currentAccount.IsActive == false)
                {
                    btViewAccInfo.IsEnabled = false;
                    btCloseAcct.IsEnabled = false;
                    btStatement.IsEnabled = false;
                    if (currentAccount != null && currentAccount.IsActive == false)
                    {
                        btStatement.IsEnabled = true;
                        btTransfer.IsEnabled = false;
                        btPayment.IsEnabled = false;
                        btDeposit.IsEnabled = false;
                        btWithdrawal.IsEnabled = false;
                    }
                }
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
            GenerateStatement statementWindow = new GenerateStatement(currentClient, currentAccount);
            // statementWindow.Owner = this;
            statementWindow.ShowDialog();
        }

        private void AddAccount()
        {
            
        }


        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lvCustomers.Items.Count == 0 || lvCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("A customer must be selected first.", "Action required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AddNewAccount addNewAcct = new AddNewAccount(currentClient);
            //addNewAcct.Owner = this;
            bool? result = addNewAcct.ShowDialog();
            if (result == true)
            {
                lvAccounts.Visibility = Visibility.Visible;
                ShowButtons();
                LoadFoundAccounts();
            }
        }

        private void btCloseAcct_Click(object sender, RoutedEventArgs e)
        {
            if (lvAccounts.Items.Count != 0 && lvAccounts.SelectedIndex != -1)
            {
                string closingAcctMessage = null;
                if (currentAccount.Balance > 0)
                {
                    closingAcctMessage = "Before closing account, confirm withdrawal of remaining balance of $" + currentAccount.Balance;
                    MessageBoxResult res = MessageBox.Show(closingAcctMessage, "Withdrawal of funds required", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    Transaction transac;
                    if (res == MessageBoxResult.Yes)
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
                        Receipt withdrawalReceipt = new Receipt(currentAccount, previousBalance, transac, currentClient, true);
                        //   withdrawalReceipt.Owner = this;
                        bool? dlgResult = withdrawalReceipt.ShowDialog();
                    }
                    if (res == MessageBoxResult.No)
                    {
                        MessageBox.Show("Account cannot be closed before withdrawal of remaining funds", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                string message = string.Format("Confirm closure of account number {0} with remaining balance of {1} $ ?", currentAccount.Id, currentAccount.Balance);
                MessageBoxResult result = MessageBox.Show(message, "Confirmation of account closure required", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    currentAccount.IsActive = false;
                    currentAccount.CloseDate = DateTime.Today;
                    AccountClosureStatement closureStatementDlg = new AccountClosureStatement(currentAccount);
                    //  closureStatementDlg.Owner = this;
                    closureStatementDlg.ShowDialog();
                }

                try
                {
                    EFData.context.SaveChanges();
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //FIX : confirm closing account
                //FIX: generate statement about closing account
                LoadFoundAccounts();

            }
        }

        private void lvAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvCustomers.Items.Count != 0 && lvCustomers.SelectedIndex != -1 && lvAccounts.Items.Count != 0 && lvAccounts.SelectedIndex != -1)
            {
                if (currentAccount.IsActive == false)
                {
                    MessageBox.Show("This account is closed");
                    return;
                }
                ViewAccountInfo viewAccInfoDlg = new ViewAccountInfo(currentClient, currentAccount);
                // viewAccInfoDlg.Owner = this;
                bool? result = viewAccInfoDlg.ShowDialog();
                if (result == true)
                {
                    LoadFoundAccounts();
                }
            }
        }

        private void CallTransactionDialog(string type)
        {
            TransactionDialog transacDlg = new TransactionDialog(currentClient, currentAccount, type);
            // transacDlg.Owner = this;
            transacDlg.ShowDialog();
             /*  if (result == true)
               {
                   SortTransactionsByTypeAndDate();
                   LoadInfoToFileds();
               } */
        }

        private bool IsBalanceSufficient()
        {
            if (currentAccount.Balance <= 0)
            {
                string message = string.Format("Account No {0} has insufficient balance ({1} $) to proceed with operation", currentAccount.Id, currentAccount.Balance);
                MessageBox.Show(message, "Warning: impossible operation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }


        //deposit
        private void btDeposit_Click(object sender, RoutedEventArgs e)
        {
            CallTransactionDialog("Deposit");
        }

       
        //withdrawal
        private void btWithdrawal_Click(object sender, RoutedEventArgs e)
        {
            if (IsBalanceSufficient() == false) { return; }
            CallTransactionDialog("Withdrawal");
        }

        //transfer
        private void btTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (IsBalanceSufficient() == false) { return; }
            CallTransactionDialog("Transfer");
        }

        //payment
        private void btPayment_Click(object sender, RoutedEventArgs e)
        {
            if (IsBalanceSufficient() == false) { return; }
            CallTransactionDialog("Payment");
        }
    }
}
