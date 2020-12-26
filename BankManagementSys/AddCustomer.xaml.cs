using SharedCode;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        public AddCustomer()
        {
            InitializeComponent();
            comboCountry.ItemsSource = Utilities.Countries;
            comboCountry.SelectedIndex = 0;
            if(rbCustCatCompany.IsChecked == true)
            {
                lblCompanyRep.Visibility = Visibility.Visible;
                lblNatIdCompRegNo.Content = "Company registration ID: *";
                lblDateBirthOrRegist.Content = "Date of company registration";
                lblCompName.Content = "Company name: *";
                tbCompanyName.Visibility = Visibility.Visible;
            }
        }

        private bool ValidateFields()
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
                MessageBox.Show("Please select gender", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (tbNatId.Text.Length < 5 || tbNatId.Text.Length > 20)
            {
                MessageBox.Show("National Id/Company registration Id number must containt between 5 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (dpBirthday == null)
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
            if (tbProvinceState.Text.Length > 20)
            {
                MessageBox.Show("Province or State must have not more than 20 caracters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (tbCompanyName.Text.Length < 1 || tbCompanyName.Text.Length > 70)
            {
                MessageBox.Show("Company name must contain between 1 and 70 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void AddCust_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) { return; }
    
            string gender = "";
            if(rbGenderMale.IsChecked == true)
            {
                gender = "male";
            }
            else if(rbGenderFemale.IsChecked == true)
            {
                gender = "female";
            }
            else if(rbGenderOther.IsChecked == true)
            {
                gender = "other";
            }
            else
            {
                MessageBox.Show("Please select gender");
                return;
            }
            try
            {
                User user = new User();

                user.FirstName = tbFirstName.Text;
                user.MiddleName = tbMiddleName.Text;
                user.LastName = tbLastName.Text;
                user.Gender = gender;
                user.NationalId = tbNatId.Text;
                user.DateOfBirth = (DateTime)dpBirthday.SelectedDate;
                user.PhoneNo = tbPhoneNo.Text;
                user.Address = tbAddress.Text;
                user.City = tbCity.Text;
                user.ProvinceState = tbProvinceState.Text;
                user.PostalCode = tbPostalCode.Text;
                user.Country = comboCountry.Text;
                user.Email = tbEmail.Text;
                if(rbCustCatCompany.IsChecked == true)
                {
                    user.CompanyName = tbCompanyName.Text;
                }
                
                EFData.context.Users.Add(user);
                EFData.context.SaveChanges();
                string successMessage = string.Format("new customer {0} {1} added successfully", user.FirstName, user.LastName);
                MessageBox.Show(successMessage, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
