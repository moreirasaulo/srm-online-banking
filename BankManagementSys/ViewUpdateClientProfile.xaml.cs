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

     /*   private bool ValidateFields()
        {
            if (tbFirstName.Text.Length < 1 || tbFirstName.Text.Length > 20)
            {
                MessageBox.Show("First name must contain between 1 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (tbMiddleName.Text.Length > 20)
            {
                MessageBox.Show("Middle name must containt not more than 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (tbLastName.Text.Length < 1 || tbLastName.Text.Length > 20)
            {
                MessageBox.Show("Last name must containt between 1 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } 
            if (rbGenderMale.IsChecked == false && rbGenderFemale.IsChecked == false && rbGenderOther.IsChecked == false)
            {
                MessageBox.Show("Please choose gender of customer", "Selection required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (tbNatId.Text.Length < 5 || tbNatId.Text.Length > 20)
            {
                MessageBox.Show("National Id/Company registration Id number must containt between 5 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (dpBirthday.SelectedDate == null)
            {
                MessageBox.Show("Please select date of birth/date of company registration", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (dpBirthday.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Date of birth/company registration date must be earlier than today's date", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(tbPhoneNo.Text, @"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$"))
            {
                MessageBox.Show("Please enter valid phone number xxx-xxx-xx-xx", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (tbAddress.Text.Length < 5 || tbAddress.Text.Length > 50)
            {
                MessageBox.Show("Address must contain between 5 and 50 caracters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (tbCity.Text.Length < 2 || tbCity.Text.Length > 20)
            {
                MessageBox.Show("City must contain between 2 and 20 caracters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (tbProvinceState.Text.Length < 2 || tbProvinceState.Text.Length > 20)
            {
                MessageBox.Show("Province or State must be between 2 and 20 caracters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (tbPostalCode.Text.Length < 5 || tbPostalCode.Text.Length > 10)
            {
                MessageBox.Show("Postal code must be made of 5 to 10 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (comboCountry.Text.Length > 20)
            {
                MessageBox.Show("Country must contain maximum 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (comboCountry.SelectedIndex == -1)
            {
                MessageBox.Show("Please select country", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (tbEmail.Text.Length > 60)
            {
                MessageBox.Show("E-mail must contain maximum 60 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (currentUser.CompanyName != null)
            {
                MessageBox.Show("Company name must contain between 1 and 70 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true; 
        } */

        private void btConfirmUpdate_Click(object sender, RoutedEventArgs e)
        {
          /*  if (!ValidateFields()) { return; }  */

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
                // EFData.context.Entry(userType).State = EntityState.Detached;
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
