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
        string currentTransType;

        public Deposit(User user,Account account, string type)
        {
            InitializeComponent();
            currentTransType = type;
            this.Title = type;
            lblTransacTitle.Content = type;
            btMakeTrans.Content = type;
            if (type == "Transfer")
            {
                this.Title = type;
                lblTransacTitle.Content = type;
                btMakeTrans.Content = type;
                lblBenefAcc.Content = "Beneficiary account No:";
                btFindBenefAccHolder.Visibility = Visibility.Visible;
            }
            if(type == "Payment")
            {
                lblPayee.Content = "Payee:";
                comboPayees.Visibility = Visibility.Visible;
                Utilities.Payees = EFData.context.Users.Include("Accounts").Where(u => u.CompanyName != null).ToList();  //FIX Exception
                comboPayees.ItemsSource = Utilities.Payees;
                comboPayees.DisplayMemberPath = "CompanyName";
                lblBenefAccOwner.Content = "Payment category";
                comboPayCategory.Visibility = Visibility.Visible;
                comboPayCategory.ItemsSource = Utilities.paymentCategories;
            }
            currentUser = user;
            currentAccount = account;
            lblOwner.Content = user.FullName;
            lblAccountNo.Content = account.Id;
            lblAccType.Content = account.AccountType.Description;
            lblBalance.Content = account.Balance + " $";

        }


        //make transaction
        private void MakeTransaction()
        {
            try
            {
                decimal amount = decimal.Parse(tbAmount.Text);  //FIX exception
                Transaction transac = new Transaction();
                transac.Date = DateTime.Now;
                transac.Amount = amount;
                transac.Type = currentTransType;
                transac.AccountId = currentAccount.Id;
                if (currentTransType == "Transfer")
                {
                    int destinationAccNo = int.Parse(tbBenefAccNo.Text); //FIX exception
                    transac.ToAccount = destinationAccNo;
                }
                if (currentTransType == "Payment")
                {
                    User payee = (User)comboPayees.SelectedItem;

                    transac.ToAccount = (from a in payee.Accounts
                                                        where a.AccountType.Description == "Business"
                                         select a.Id).FirstOrDefault();  //FIX exception
                    transac.PaymentCategory = comboPayCategory.Text;
                }
                EFData.context.Transactions.Add(transac);
                decimal previousBalance = currentAccount.Balance; //balance before transaction
                if(currentTransType == "Deposit")
                {
                    currentAccount.Balance = currentAccount.Balance + amount;  //new balance
                }
                if(currentTransType == "Withdrawal")
                {
                    currentAccount.Balance = currentAccount.Balance - amount;  //new balance
                }
                if(currentTransType == "Transfer")
                {
                    int destinationAccNo = int.Parse(tbBenefAccNo.Text); //FIX exception
                    currentAccount.Balance = currentAccount.Balance - amount;  //new balance
                    Account beneficiaryAcc = EFData.context.Accounts.SingleOrDefault(a => a.Id == destinationAccNo);
                    beneficiaryAcc.Balance = beneficiaryAcc.Balance + amount;  //add money to beneficiary
                }
                if (currentTransType == "Payment")
                {
                    User payee = (User)comboPayees.SelectedItem;

                    Account payeeAcc = (from a in payee.Accounts
                                         where a.AccountType.Description == "Business"
                                         select a).FirstOrDefault();  //FIX exception
                    currentAccount.Balance = currentAccount.Balance - amount;  //new balance
                    payeeAcc.Balance = payeeAcc.Balance + amount;  //add money to company
                }
                EFData.context.SaveChanges();
                lblBalance.Content = currentAccount.Balance + " $";
                // int transactionId = transDeposit.Id;

                string message = string.Format("The {0} was completed successfully", currentTransType.ToLower());
                MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                tbAmount.Text = "";
                tbBenefAccNo.Text = "";
                DepositReceipt depositReceiptDlg = new DepositReceipt(currentAccount, previousBalance, transac, currentUser, true);
                depositReceiptDlg.Owner = this;
                bool? result = depositReceiptDlg.ShowDialog();
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

   

        private void btMakeTrans_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Format("Proceed with this {0}: {1} $ ?", currentTransType.ToLower(), tbAmount.Text);
            MessageBoxResult answer = MessageBox.Show(message, "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                decimal amount = decimal.Parse(tbAmount.Text); //FIX exception (format exception)

                MakeTransaction();

            }
        }

        private void btFindBenefAccHolder_Click(object sender, RoutedEventArgs e)
        {
            int destinationAccNo = int.Parse(tbBenefAccNo.Text); //FIX formatException
            Account beneficiaryAcc = EFData.context.Accounts.SingleOrDefault(a => a.Id == destinationAccNo);
            if (beneficiaryAcc == null)
            {
                lblBenefAccOwner.Content = "This destination account does not exist";
                lblBenefAccOwner.Foreground = new SolidColorBrush(Colors.Red);
            }
            if (beneficiaryAcc != null)
            {
                lblBenefAccOwner.Content = string.Format("Beneficiary account holder: {0}", beneficiaryAcc.User.FullName);
            }
        }
    }
}
