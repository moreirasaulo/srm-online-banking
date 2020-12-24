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
    /// Interaction logic for Deposit.xaml
    /// </summary>
    public partial class Deposit : Window
    {
        User currentUser;
        Account currentAccount;

        public Deposit(User user,Account account)
        {
            InitializeComponent();
            currentUser = user;
            currentAccount = account;
            lblOwner.Content = string.Format("{0} {1} {2}", user.FirstName, user.MiddleName, user.LastName);
            lblAccountNo.Content = account.Id;
            lblAccType.Content = account.AccountType.Description;
            lblBalance.Content = account.Balance + " $";

        }

        private void btDeposit_Click(object sender, RoutedEventArgs e)
        {
            decimal amount = 0;
            decimal.TryParse(tbAmount.Text, out amount); //FIX exception (format exception?)
            try
            {
                Transaction transDeposit = new Transaction
                {
                    Date = DateTime.Now,
                    Amount = amount,
                    Type = "Deposit",
                    AccountId = currentAccount.Id
                };
                EFData.context.Transactions.Add(transDeposit);
                currentAccount.Balance = currentAccount.Balance + amount;
                EFData.context.SaveChanges();
                lblBalance.Content = currentAccount.Balance + " $";
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
