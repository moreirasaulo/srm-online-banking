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
    /// Interaction logic for AddClientDialog.xaml
    /// </summary>
    public partial class AddClientDialog : Window
    {
        public AddClientDialog()
        {
            InitializeComponent();
        }


        private void Wizard_Next(object sender, Xceed.Wpf.Toolkit.Core.CancelRoutedEventArgs e)
        {
            //page 1 (customer type)
            if (rbCustCatIndividual.IsChecked == false && rbCustCatCompany.IsChecked == false)
            {
                MessageBox.Show("Please choose the type of customer", "Selection required", MessageBoxButton.OK, MessageBoxImage.Warning);
                Page1.CanSelectNextPage = false;
                return;
            }
            Page1.CanSelectNextPage = true;
            if (rbCustCatCompany.IsChecked == true)
            {
                lblCompanyRep.Visibility = Visibility.Visible;  //company representative title
                Page3.Title = "Company registration number and date of registration";
                lblNatIdCompRegNo.Content = "Company registration number: *";
                lblDateBirthOrRegist.Content = "Company registration date: *";
            }

            //page 2 (full name)
            if (tbFirstName.Text.Length < 1 || tbFirstName.Text.Length > 20)
            {
                MessageBox.Show("First name must contain between 1 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                Page2.CanSelectNextPage = false;
                return;
            }
            if (tbMiddleName.Text.Length > 20)
            {
                MessageBox.Show("Middle name must containt not more than 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                Page2.CanSelectNextPage = false;
                return;
            }
            if (tbLastName.Text.Length < 1 || tbLastName.Text.Length > 20)
            {
                MessageBox.Show("Last name must containt between 1 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                Page2.CanSelectNextPage = false;
                return;
            }
            if (rbGenderMale.IsChecked == false && rbGenderFemale.IsChecked == false  && rbGenderOther.IsChecked == false)
            {
                MessageBox.Show("Please choose gender of customer", "Selection required", MessageBoxButton.OK, MessageBoxImage.Warning);
                Page2.CanSelectNextPage = false;
                return;
            }
            Page2.CanSelectNextPage = true;

            //page 3 (nat id/company reg number and date of birth/company reg date)
            if (tbNatId.Text.Length < 5 || tbNatId.Text.Length > 20)
            {
                MessageBox.Show("National Id/Company registration Id number must containt between 5 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                Page3.CanSelectNextPage = false;
                return;
            }
            if (dpBirthday == null)
            {
                MessageBox.Show("Please select date of birth/date of company registration", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                Page3.CanSelectNextPage = false;
                return;
            }
            if (dpBirthday.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Date of birth/company registration date must be earlier than today's date", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                Page3.CanSelectNextPage = false;
                return;
            }
            Page3.CanSelectNextPage = true;
        }
    }
}
