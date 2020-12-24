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

namespace CustomerUI
{
    /// <summary>
    /// Interaction logic for MakeTransfer.xaml
    /// </summary>
    public partial class MakeTransfer : Window
    {
        Account currentAccount;

        public MakeTransfer(Account account)
        {
            InitializeComponent();
            currentAccount = account;
            lblOwnerName.Content = string.Format("{0} {1} {2}", Utils.login.User.FirstName, Utils.login.User.MiddleName, Utils.login.User.LastName);
            lblBalance.Content = string.Format("$ {0}", currentAccount.Balance);
            lblAccNo.Content = currentAccount.Id;
            lblAccType.Content = currentAccount.AccountType.Description;
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure you would like to proceed with this transfer?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                if (!ValidateFields()) { return; }

                decimal amount = 0;
                decimal.TryParse(tbAmount.Text, out amount);  //FIX exception

                int destinationAccNo;
                int.TryParse(tbBeneficiaryAcct.Text, out destinationAccNo);  //FIX exception

                try
                {
                    Transaction transTransfer = new Transaction
                    {
                        Date = DateTime.Now,
                        Amount = amount,
                        ToAccount = destinationAccNo,
                        Type = "Transfer",
                        AccountId = currentAccount.Id
                    };
                    EFData.context.Transactions.Add(transTransfer);
                    currentAccount.Balance = currentAccount.Balance - amount;
                    Account beneficiaryAcc = EFData.context.Accounts.SingleOrDefault(a => a.Id == destinationAccNo);
                    beneficiaryAcc.Balance = beneficiaryAcc.Balance + amount;
                    EFData.context.SaveChanges();
                    lblBalance.Content = currentAccount.Balance;
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MessageBox.Show("The transfer was completed successfuly.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                tbAmount.Text = "";
                tbBeneficiaryAcct.Text = "";
            }
        }

        private bool ValidateFields()
        {
            int destinationAccNo;
            int.TryParse(tbBeneficiaryAcct.Text, out destinationAccNo);  //FIX exception
            Account beneficiaryAcc = EFData.context.Accounts.SingleOrDefault(a => a.Id == destinationAccNo);
            if (beneficiaryAcc == null)
            {
                MessageBox.Show("Destination account does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            decimal amount = 0;
            decimal.TryParse(tbAmount.Text, out amount); //FIX exception
            if (amount < 0)
            {
                MessageBox.Show("You cannot transfer negative amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (amount > currentAccount.Balance)
            {
                MessageBox.Show("You do not have sufficinet funds to make this transfer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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

        private void tbBeneficiaryAcct_KeyDown(object sender, KeyEventArgs e)
        {
            NumbersOnly(e);
        }

        private void tbAmount_KeyDown(object sender, KeyEventArgs e)
        {
            NumbersOnly(e);
        }

        private void btBackToTrans_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
