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
        public MakeTransfer()
        {
            InitializeComponent();

        }

        private bool ValidateFields()
        {
            Account selectedAcc = (Account)comboAccounts.SelectedItem;
            if (selectedAcc == null)
            {
                MessageBox.Show("Please choose an account to make transfer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int destinationAccNo;
            int.TryParse(tbBeneficiaryAcct.Text, out destinationAccNo);  //FIX exception
            Account beneficiaryAcc = EFData.context.Accounts.SingleOrDefault(a => a.Id == destinationAccNo);
            if(beneficiaryAcc == null)
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
            if (amount > selectedAcc.Balance)
            {
                MessageBox.Show("You do not have sufficinet funds to make this transfer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure you would like to proceed with this transfer?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                Account selectedAcc = (Account)comboAccounts.SelectedItem;
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
                        AccountId = selectedAcc.Id
                    };
                    EFData.context.Transactions.Add(transTransfer);
                    selectedAcc.Balance = selectedAcc.Balance - amount;
                    Account beneficiaryAcc = EFData.context.Accounts.SingleOrDefault(a => a.Id == destinationAccNo);
                    beneficiaryAcc.Balance = beneficiaryAcc.Balance + amount;
                    EFData.context.SaveChanges();
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MessageBox.Show("The transfer was completed successfuly.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

       
    }
}
