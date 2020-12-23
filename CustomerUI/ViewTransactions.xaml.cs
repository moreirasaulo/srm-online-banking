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
    /// Interaction logic for ViewTransactions.xaml
    /// </summary>
    public partial class ViewTransactions : Window
    {
        List<string> transactionHistoryDays = new List<string> { "7 days", "30 days", "60 days" };
        
        public ViewTransactions()
        {
            InitializeComponent();
            
            lblLoggedInAs.Content = string.Format("Logged as {0} {1}", Utils.login.User.FirstName,
                Utils.login.User.LastName);
           // LoadUserAccounts();
            comboAccountType.ItemsSource = Utils.login.User.Accounts;
            comboAccountType.DisplayMemberPath = "AccountType.Description";
            comboHistory.Items.Add("7 days");
            comboHistory.Items.Add("30 days");
            comboHistory.Items.Add("60 days"); //FIX items are not loading to combo from list

            comboHistory.SelectedIndex = 0;
            if (Utils.login.User.Accounts.Count == 0)
            {
                lblError.Content = "There's no bank account linked to your profile yet.";
                return;
            }
           
        }

     /*   private void LoadUserAccounts()
        {
            Utils.userAccounts = EFData.context.Accounts.Where(a => a.UserId == Utils.loggedInUser.Id).ToList();

            foreach (Account account in Utils.userAccounts)
            {
                account.AccountType = EFData.context.AccountTypes.FirstOrDefault(t => t.Id == account.AccountTypeId);

                //account.Transactions = EFData.context.Transactions.Where(tr => tr.AccountId == account.Id).ToList();

                account.User = EFData.context.Users.FirstOrDefault(u => u.Id == account.UserId);

            }           
        } */

        private void SortTransactions()
        {
            List<Transaction> sortedTransactions = new List<Transaction>();

            //by type
            if (rbTransactAll.IsChecked == true)
            { 
                sortedTransactions = Utils.userTransactions;

            }
            else if (rbTransacDeposits.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Deposit").ToList();
            }
            else if (rbTransacWithdrawals.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Withdrawal").ToList();
            }
            else if (rbTransacTransfers.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Transfer").ToList();
            }
            else if (rbTransacPayments.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Payment").ToList();
            }
            else
            {
                sortedTransactions = Utils.userTransactions;
            }

            //by date
            if(comboHistory.SelectedIndex == 0)
            {
                sortedTransactions = Utils.userTransactions.Where(t => (DateTime.Now - t.Date).TotalDays <= 7).ToList();
            }
            else if (comboHistory.SelectedIndex == 1)
            {
                sortedTransactions = Utils.userTransactions.Where(t => (DateTime.Now - t.Date).TotalDays <= 30).ToList();
            }
            else if (comboHistory.SelectedIndex == 2)
            {
                sortedTransactions = Utils.userTransactions.Where(t => (DateTime.Now - t.Date).TotalDays <= 60).ToList();
            }
            else
            {
                sortedTransactions = Utils.userTransactions;
            }

            lvTransactions.ItemsSource = sortedTransactions;
        }

        private void btShowTransactionsClicked(object sender, RoutedEventArgs e)
        {
            Account selectedAcc = (Account)comboAccountType.SelectedItem;
            if (selectedAcc == null)
            {
                MessageBox.Show("Please choose an acount to view transactions");
                return;
            }

            Utils.userTransactions = EFData.context.Transactions.Where(t => t.AccountId ==
            selectedAcc.Id).ToList();

            SortTransactions();

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (comboHistory == null || comboHistory.Items.Count == 0)
            {
                return;
            }
                SortTransactions();
            
        }

        

        private void comboHistory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (comboHistory == null || comboHistory.Items.Count == 0)
            {
                return;
            }
            SortTransactions();
            
        }
    }
}
