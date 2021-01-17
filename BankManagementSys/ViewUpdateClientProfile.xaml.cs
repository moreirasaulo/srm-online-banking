using SharedCode;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for ViewUpdateClientProfile.xaml
    /// </summary>
    public partial class ViewUpdateClientProfile : UserControl
    {
        User currentUser;
        public ViewUpdateClientProfile(User user)
        {
            InitializeComponent();
            currentUser = user;
            if (currentUser.CompanyName != null)
            {
                lblDate.Content = "Company reg. date:";
                lblId.Content = "Company reg. No:";
                lblCompanyName.Content = "Company name:";
                tbCompanyName.Visibility = Visibility.Visible;
                tbCompanyName.Text = currentUser.CompanyName;
            }
            comboCountry.ItemsSource = Utilities.Countries;
            DeactivateFields();
            LoadCustomerInfoToFileds();
        }

        private void LoadCustomerInfoToFileds()
        {
            tbFirstName.Text = currentUser.FirstName;
            tbMiddleName.Text = currentUser.MiddleName;
            tbLastName.Text = currentUser.LastName;
            tbNatId.Text = currentUser.NationalId;
            if (currentUser.Gender.ToLower() == "male")
            {
                rbGenderMale.IsChecked = true;
            }
            else if (currentUser.Gender.ToLower() == "female")
            {
                rbGenderFemale.IsChecked = true;
            }
            else if (currentUser.Gender.ToLower() == "other")
            {
                rbGenderOther.IsChecked = true;
            }
            dpBirthday.SelectedDate = currentUser.DateOfBirth;
            tbPhoneNo.Text = currentUser.PhoneNo;
            tbAddress.Text = currentUser.Address;
            tbCity.Text = currentUser.City;
            tbProvinceState.Text = currentUser.ProvinceState;
            tbPostalCode.Text = currentUser.PostalCode;
            comboCountry.Text = currentUser.Country;
            tbEmail.Text = currentUser.Email;
        }

        private void DeactivateFields()
        {
            tbFirstName.IsEnabled = false;
            tbMiddleName.IsEnabled = false;
            tbLastName.IsEnabled = false;
            tbNatId.IsEnabled = false;
            rbGenderMale.IsEnabled = false;
            rbGenderFemale.IsEnabled = false;
            rbGenderOther.IsEnabled = false;
            dpBirthday.IsEnabled = false;
            tbPhoneNo.IsEnabled = false;
            tbAddress.IsEnabled = false;
            tbCity.IsEnabled = false;
            tbProvinceState.IsEnabled = false;
            tbPostalCode.IsEnabled = false;
            comboCountry.IsEnabled = false;
            tbCompanyName.IsEnabled = false;
            tbEmail.IsEnabled = false;

        }

        private void ActivateFields()
        {
            tbFirstName.IsEnabled = true;
            tbMiddleName.IsEnabled = true;
            tbLastName.IsEnabled = true;
            tbNatId.IsEnabled = true;
            rbGenderMale.IsEnabled = true;
            rbGenderFemale.IsEnabled = true;
            rbGenderOther.IsEnabled = true;
            dpBirthday.IsEnabled = true;
            tbPhoneNo.IsEnabled = true;
            tbAddress.IsEnabled = true;
            tbCity.IsEnabled = true;
            tbProvinceState.IsEnabled = true;
            tbPostalCode.IsEnabled = true;
            comboCountry.IsEnabled = true;
            tbCompanyName.IsEnabled = true;
            tbEmail.IsEnabled = true;
        }

        private void btUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            btConfirmUpdate.Visibility = Visibility.Visible;
            btCancelUpdate.Visibility = Visibility.Visible;
            btUpdateCustomer.Visibility = Visibility.Hidden;
            ActivateFields();
        }

        private void btCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomerInfoToFileds();
            DeactivateFields();
            btCancelUpdate.Visibility = Visibility.Hidden;
            btConfirmUpdate.Visibility = Visibility.Hidden;
            btUpdateCustomer.Visibility = Visibility.Visible;
        }


        private void btConfirmUpdate_Click(object sender, RoutedEventArgs e)
        {

            currentUser.FirstName = tbFirstName.Text;
            currentUser.MiddleName = tbMiddleName.Text;
            currentUser.LastName = tbLastName.Text;
            currentUser.NationalId = tbNatId.Text;
            if (rbGenderMale.IsChecked == true)
            {
                currentUser.Gender = "male";
            }
            else if (rbGenderFemale.IsChecked == true)
            {
                currentUser.Gender = "female";
            }
            else if (rbGenderOther.IsChecked == true)
            {
                currentUser.Gender = "other";
            }
            currentUser.DateOfBirth = (DateTime)dpBirthday.SelectedDate;
            currentUser.PhoneNo = tbPhoneNo.Text;
            currentUser.Address = tbAddress.Text;
            currentUser.City = tbCity.Text;
            currentUser.PostalCode = tbPostalCode.Text;
            currentUser.ProvinceState = tbProvinceState.Text;
            currentUser.Country = comboCountry.Text;
            currentUser.Email = tbEmail.Text;
            if (currentUser.CompanyName != null)
            {
                currentUser.CompanyName = tbCompanyName.Text;
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
                MessageBox.Show("Database error: " + ex.Message, "Error fetching from database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadCustomerInfoToFileds();
            DeactivateFields();
            btCancelUpdate.Visibility = Visibility.Hidden;
            btConfirmUpdate.Visibility = Visibility.Hidden;
            btUpdateCustomer.Visibility = Visibility.Visible;
        }

    }
}
