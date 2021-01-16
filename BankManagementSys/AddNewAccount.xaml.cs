using SharedCode;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
    /// Interaction logic for AddNewAccount.xaml
    /// </summary>
    public partial class AddNewAccount : Window
    {
        User currentCust;
        AccountType selectedAccType;
        public AddNewAccount(User user)
        {
            InitializeComponent();
            currentCust = user;
            lblNewAcctFor.Content = "New account for " + user.FirstName + " " + user.LastName;

            List<AccountType> acctTypes = null;
            if (currentCust.CompanyName == null)
            {
                try
                {
                    acctTypes = EFData.context.AccountTypes.Where(a => a.Description != "Business").ToList();
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error loading from Database", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (currentCust.CompanyName != null)
            {
                try
                {
                    acctTypes = new List<AccountType>();
                    acctTypes.Add(EFData.context.AccountTypes.FirstOrDefault(a => a.Description == "Business"));
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error loading from Database", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            comboAcctTypes.ItemsSource = acctTypes;
            comboAcctTypes.DisplayMemberPath = "Description";
        }

        private void comboAcctTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboAcctTypes.Items.Count != 0 && comboAcctTypes.SelectedIndex != -1)
            {
                selectedAccType = (AccountType)comboAcctTypes.SelectedItem;
                if (selectedAccType.Description.Equals("Checking") || selectedAccType.Description.Equals("Business"))
                {
                    lblFeeOrInterest.Content = "Monthly fee:";
                }
                if (selectedAccType.Description.Equals("Savings") || selectedAccType.Description.Equals("Investment"))
                {
                    lblFeeOrInterest.Content = "Interest:";
                }
                lblFeeOrInterest.Visibility = Visibility.Visible;
                tbFeeOrInterest.Visibility = Visibility.Visible;
            }
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsValid()) { return; }
            MessageBoxResult result = MessageBox.Show("Please confirm the new Account Information before proceed:\n\nCustomer name: " + currentCust.FullName + "\nAccount type: " + selectedAccType.Description + "\n" + lblFeeOrInterest.Content + " " + tbFeeOrInterest.Text + "\n\nWould you like to confirm the creation of the new account above?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                Account account = new Account();
                account.OpenDate = DateTime.Today;
                account.UserId = currentCust.Id;
                account.Balance = 0;
                account.AccountTypeId = selectedAccType.Id;
                if (selectedAccType.Description == "Checking" || selectedAccType.Description == "Business")
                {
                    account.MonthlyFee = decimal.Parse(tbFeeOrInterest.Text);
                }
                if (selectedAccType.Description == "Savings" || selectedAccType.Description == "Investment")
                {
                    account.Interest = decimal.Parse(tbFeeOrInterest.Text);
                }
                account.IsActive = true;

                try
                {
                    EFData.context.Accounts.Add(account);
                    EFData.context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                    MessageBox.Show(error.ErrorMessage);
                    EFData.context.Entry(account).State = EntityState.Detached;
                    return;
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show(ex.InnerException.InnerException.Message);
                    return;
                }

               MessageBox.Show("Account created successfully","New account created", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;

                ViewAccountInfo viewCreatedAccInfoDialog = new ViewAccountInfo(currentCust, account);
                viewCreatedAccInfoDialog.Owner = Utilities.adminDashboard;
                
                viewCreatedAccInfoDialog.ShowDialog();
            }
        }

        private bool AreFieldsValid()
        {
           if (comboAcctTypes.Items.Count == 0 || comboAcctTypes.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an account type.", "Action required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (tbFeeOrInterest.Text.Length == 0)
            {
                MessageBox.Show("The interest/fee field cannot be empty.", "Action required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            decimal interestFee;
            try
            {
                interestFee = decimal.Parse(tbFeeOrInterest.Text);
                if (interestFee <= 0)
                {
                    MessageBox.Show("Interest/Fee must not be 0 or negative.", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if ((selectedAccType.Id == 1 || selectedAccType.Id == 4) && (interestFee < 4 || interestFee > 50))
                {
                    MessageBox.Show("Fee must be between 4 $ and 50 $", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                if ((selectedAccType.Id == 2 || selectedAccType.Id == 3) && (interestFee < (decimal)0.5 || interestFee > 10))
                {
                    MessageBox.Show("Interest must be between 0.5 % and 10 %", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Interest/Fee must contain only digits and period/decimal(.) symbol", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        } 

        private void DecimalInput(KeyEventArgs e)
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
                case Key.OemPeriod:
                case Key.Decimal:
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }
        private void tbFeeOrInterest_KeyDown(object sender, KeyEventArgs e)
        {
            DecimalInput(e);
        }
    }
}
