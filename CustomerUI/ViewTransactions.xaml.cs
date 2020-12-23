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
        
        public ViewTransactions()
        {
            InitializeComponent();
            lblLoggedInAs.Content = string.Format("Logged as {0} {1}", Utils.login.User.FirstName,
                Utils.login.User.LastName);

            comboAccountType.ItemsSource = Utils.login.User.Accounts;
            if (Utils.login.User.Accounts.Count == 0)
            {
                lblError.Content = "There's no bank account linked to your profile yet.";
                return;
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
            lvTransactions.ItemsSource = Utils.userTransactions;
        }
    }
}
