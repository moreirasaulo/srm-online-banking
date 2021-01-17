using SharedCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CustomerUI
{
    /// <summary>
    /// Interaction logic for TransferPaymentDialog.xaml
    /// </summary>
    public partial class TransferPaymentDialog : Window
    {
        string currentTransType;
        public TransferPaymentDialog(string type)
        {
            InitializeComponent();
            currentTransType = type;
            this.Title = type;
            lblTransacTypeTitle.Content = type;
            btMakeTransaction.Content = "Make " + type;
            lblAccNo.Content = Utils.selectedAcc.Id;
            lblOwnerName.Content = Utils.login.User.FullName;
            lblAccType.Content = Utils.selectedAcc.AccountType.Description;
            lblBalance.Content = Utils.selectedAcc.Balance + " $";

            if (type == "Transfer")
            {
                lblBeneficiaryPayee.Content = "Beneficiary account No:";
                tbBeneficiaryAcct.Visibility = Visibility.Visible;
            }
            if (type == "Payment")
            {
                lblPayee.Content = "Payee:";
                comboPayees.Visibility = Visibility.Visible;
                try
                {
                    Utils.Payees = EFData.context.Users.Include("Accounts").Where(u => u.CompanyName != null).ToList();
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                comboPayees.ItemsSource = Utils.Payees;
                comboPayees.DisplayMemberPath = "CompanyName";
                lblBeneficiaryPayee.Content = "Payment category:";
                comboPayCategory.Visibility = Visibility.Visible;
                comboPayCategory.ItemsSource = Utils.paymentCategories;
                comboPayCategory.SelectedIndex = 0;
            }
        }

        private void btMakeTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) { return; }
            string message = string.Format("Proceed with this {0}: {1} $ ?", currentTransType.ToLower(),
                decimal.Parse(tbAmount.Text).ToString("N2"));
            MessageBoxResult answer = MessageBox.Show(message, "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                MakeTransaction();
            }
        }

        private void MakeTransaction()
        {
            Transaction transac = null;
            Transaction depositToBenefAccount = null;
            try
            {
                decimal amount = decimal.Parse(tbAmount.Text);
                transac = new Transaction();
                transac.Date = DateTime.Today;
                transac.Amount = amount;
                transac.Type = currentTransType;
                transac.AccountId = Utils.selectedAcc.Id;
                if (currentTransType == "Transfer")
                {
                    int destinationAccNo = int.Parse(tbBeneficiaryAcct.Text);
                    transac.ToAccount = destinationAccNo;
                }
                if (currentTransType == "Payment")
                {
                    User payee = (User)comboPayees.SelectedItem;

                    transac.ToAccount = (from a in payee.Accounts
                                         where a.AccountType.Description == "Business"
                                         select a.Id).FirstOrDefault();

                    transac.PaymentCategory = comboPayCategory.Text;
                }
                EFData.context.Transactions.Add(transac);

                decimal previousBalance = Utils.selectedAcc.Balance; //balance before transaction
                
                    Account beneficiaryAcc = EFData.context.Accounts.SingleOrDefault(a => a.Id == transac.ToAccount);
                Utils.selectedAcc.Balance = Utils.selectedAcc.Balance - Math.Round(amount, 2);  //new balance

                depositToBenefAccount = new Transaction
                {
                    Date = DateTime.Today,
                    Amount = amount,
                    Type = "Deposit",
                    AccountId = beneficiaryAcc.Id
                };
                EFData.context.Transactions.Add(depositToBenefAccount);
                beneficiaryAcc.Balance = beneficiaryAcc.Balance + Math.Round(amount, 2);  //add money to beneficiary
                
                EFData.context.SaveChanges();
                lblBalance.Content = Utils.selectedAcc.Balance + " $";


                string message = string.Format("The {0} was completed successfully", currentTransType.ToLower());
                MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearFields();

                Receipt receiptDlg = new Receipt(Utils.selectedAcc, previousBalance, transac, true);
                receiptDlg.Owner = this;
                bool? result = receiptDlg.ShowDialog();
                if (result == true)
                {
                    MessageBoxResult answer = MessageBox.Show("Would you like to perform another " + currentTransType.ToLower() + " ?", "Choice required", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (answer == MessageBoxResult.No)
                    {
                        DialogResult = true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                MessageBox.Show(error.ErrorMessage);
                EFData.context.Entry(transac).State = EntityState.Detached;
                EFData.context.Entry(depositToBenefAccount).State = EntityState.Detached;
                return;
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFields()
        {
            tbAmount.Text = "";
            tbBeneficiaryAcct.Text = "";
            comboPayCategory.SelectedIndex = 0;
            comboPayees.SelectedIndex = -1;
            lblBeneficiaryName.Content = "";
        }

        private bool ValidateFields()
        {
            // transfer and payment(amount)
            decimal amount;
            try
            {
                amount = decimal.Parse(tbAmount.Text);
                if (amount <= 0)
                {
                    MessageBox.Show("Amount must not be 0 or negative", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Amount must contain only digits and period/decimal(.) symbol", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Utils.selectedAcc.Balance < amount)
            {
                MessageBox.Show("Insufficient funds to proceed with operation", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //transfer (dest account)
            if (currentTransType == "Transfer")
            {
                return ValidateDestAccount();
            }
            //payment (payee account, payment category)
            if (currentTransType == "Payment")
            {
                if (comboPayCategory.Items.Count == 0 || comboPayCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Payment category must be selected", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                return ValidateDestAccount();
            }
            return true;
        }

        private bool ValidateDestAccount()
        {
            int destinationAccNo;
            try
            {
                if (currentTransType == "Transfer")
                {
                    destinationAccNo = int.Parse(tbBeneficiaryAcct.Text);

                    Account beneficiaryAcc = EFData.context.Accounts.SingleOrDefault(a => a.Id == destinationAccNo);
                    if (beneficiaryAcc == null)
                    {
                        lblBeneficiaryName.Foreground = new SolidColorBrush(Colors.Red);
                        lblBeneficiaryName.Content = "This destination account does not exist";
                        return false;
                    }
                    lblBeneficiaryName.Foreground = new SolidColorBrush(Colors.Black);
                    lblBeneficiaryName.Content = string.Format("Beneficiary account holder: {0}", beneficiaryAcc.User.FullName);
                    if (beneficiaryAcc.Id == Utils.selectedAcc.Id)
                    {
                        MessageBox.Show("Destination account and current account must be different", "Transaction prohibited", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    return true;
                }
                if (currentTransType == "Payment")
                {
                    if (comboPayees.Items.Count == 0 || comboPayees.SelectedIndex == -1)
                    {
                        MessageBox.Show("Payee must be selected", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    User payee = (User)comboPayees.SelectedItem;

                    Account payeeAcc = (from a in payee.Accounts
                                        where a.AccountType.Description == "Business"
                                        select a).FirstOrDefault();
                    if (payeeAcc == null)
                    {
                        MessageBox.Show("Payee business account does not exist", "Payment impossible", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    return true;
                }
                return false;

            }
            catch (FormatException)
            {
                if (tbBeneficiaryAcct.Text.Length == 0)
                {
                    lblBeneficiaryName.Foreground = new SolidColorBrush(Colors.Red);
                    lblBeneficiaryName.Content = "Enter beneficiary account number";
                    return false;
                }
                MessageBox.Show("Destination account must contain digits only", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void tbBeneficiaryAcct_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDestAccount();
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
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        private void MoneyInput(KeyEventArgs e)
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

        private void tbBeneficiaryAcct_KeyDown(object sender, KeyEventArgs e)
        {
            NumbersOnly(e);
        }

        private void tbAmount_KeyDown(object sender, KeyEventArgs e)
        {
            MoneyInput(e);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DialogResult = true;
        }
    }
}
