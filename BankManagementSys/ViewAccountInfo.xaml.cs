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
    /// Interaction logic for ViewAccountInfo.xaml
    /// </summary>
    public partial class ViewAccountInfo : Window
    {

        public ViewAccountInfo(User user,Account account)
        {
            InitializeComponent();
            lblFullName.Content = string.Format("{0} {1} {2}", user.FirstName, user.MiddleName, user.LastName);
            lblDateOfBirth.Content = user.DateOfBirth.ToShortDateString();
            lblNatId.Content = user.NationalId;
            lblAccNo.Content = account.Id;
            lblOpenDate.Content = account.OpenDate.ToShortDateString();
            lblBalance.Content = account.Balance;
            lblAccType.Content = account.AccountType.Description;
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
