using SharedCode;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SetupNewRate.xaml
    /// </summary>
    public partial class SetupNewRate : Window
    {
        Account currentAccount;
        public SetupNewRate(Account account)
        {
            InitializeComponent();
            currentAccount = account;
            if (currentAccount.AccountType.Description == "Checking") 
            {
                lblInformation.Content = "New monthly fee rate:";
            }
            else if (currentAccount.AccountType.Description == "Savings")
            {
                lblInformation.Content = "New interest rate:";
            }
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsValid()) 
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you would like to setup the new rate for this " + currentAccount.AccountType.Description + " account at " + tbNewRate.Text + "?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) 
            {
                if (currentAccount.AccountType.Description == "Checking" || currentAccount.AccountType.Description == "Business")
                {
                    currentAccount.MonthlyFee = decimal.Parse(tbNewRate.Text);
                }
                if (currentAccount.AccountType.Description == "Savings" || currentAccount.AccountType.Description == "Investment")
                {
                    currentAccount.Interest = decimal.Parse(tbNewRate.Text);
                }
                try
                {
                    EFData.context.SaveChanges();      
                }
                    catch (DbEntityValidationException ex)
                    {
                        var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                        MessageBox.Show(error.ErrorMessage);
                        return;
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                MessageBox.Show("The rate was modified successfully");
            }
            DialogResult = true;
        }

        private bool AreFieldsValid()
        {

            decimal interestFee;
            try
            {
                interestFee = decimal.Parse(tbNewRate.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("The new rate must contain only digits and . symbol", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void tbNewRate_KeyDown(object sender, KeyEventArgs e)
        {
            DecimalInput(e);
        }
    }
}
