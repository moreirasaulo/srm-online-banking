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
            comboHistory.Items.Add("7 days");
            comboHistory.Items.Add("30 days");
            comboHistory.Items.Add("60 days"); //FIX items are not loading to combo from list

            
            lblLoggedInAs.Content = string.Format("Logged as {0} {1}", Utils.login.User.FirstName,
                Utils.login.User.LastName);
           // LoadUserAccounts();
            comboAccountType.ItemsSource = Utils.login.User.Accounts;
            comboAccountType.DisplayMemberPath = "AccountType.Description";
            
            if (Utils.login.User.Accounts.Count == 0)
            {
                lblError.Content = "There's no bank account linked to your profile yet.";
                return;
            }
           
        }

        private void SortTransactionsByTypeAndDate()
        {
            List<Transaction> sortedTransactions = new List<Transaction>();

            //by type
            if (rbTransactAll.IsChecked == true)
            { 
                sortedTransactions = Utils.userTransactions;
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else if (rbTransacDeposits.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Deposit").ToList();
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else if (rbTransacWithdrawals.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Withdrawal").ToList();
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else if (rbTransacTransfers.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Transfer").ToList();
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else if (rbTransacPayments.IsChecked == true)
            {
                sortedTransactions = Utils.userTransactions.Where(t => t.Type == "Payment").ToList();
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            else
            {
                sortedTransactions = Utils.userTransactions;
                //by date
                sortedTransactions = SortTransactionsByDate(sortedTransactions);
            }
            lvTransactions.ItemsSource = sortedTransactions;
        }

        private List<Transaction> SortTransactionsByDate(List<Transaction> list)
        {
            if (comboHistory.SelectedIndex == 0)
            {
                return list = list.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 7);
            }
            else if (comboHistory.SelectedIndex == 1)
            {
                return list = list.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 30);
            }
            else if (comboHistory.SelectedIndex == 2)
            {
                return list = list.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 60);
            }
            else
            {
                return list;
            }
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

            SortTransactionsByTypeAndDate();
            comboHistory.SelectedIndex = 0;

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SortTransactionsByTypeAndDate(); 
        }

        

        private void comboHistory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            SortTransactionsByTypeAndDate();  
        }

        private void btBackToDash_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
