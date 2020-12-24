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
    /// Interaction logic for MakePayment.xaml
    /// </summary>
    public partial class MakePayment : Window
    {
        public MakePayment()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure you would like to proceed with this payment?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                MessageBox.Show("The payment was completed successfuly.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
    }
}
