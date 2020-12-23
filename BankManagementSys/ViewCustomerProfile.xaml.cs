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
    /// Interaction logic for ViewCustomerProfile.xaml
    /// </summary>
    public partial class ViewCustomerProfile : Window
    {
        User currentUser;
        public ViewCustomerProfile(User user)
        {
            currentUser = user;
            InitializeComponent();
            comboCountry.ItemsSource = Utils.Countries;
            DeactivateFields();
            LoadCustomerInfoToFileds();
            
        }

        private void LoadCustomerInfoToFileds()
        {
            tbFirstName.Text = currentUser.FirstName;
            tbMiddleName.Text = currentUser.MiddleName;
            tbLastName.Text = currentUser.LastName;
            tbNatId.Text = currentUser.NationalId;
            if (currentUser.Gender == "male")
            {
                rbGenderMale.IsChecked = true;
            }
            else if (currentUser.Gender == "female")
            {
                rbGenderFemale.IsChecked = true;
            }
            else if (currentUser.Gender == "other")
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

        }

        private void btUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            ActivateFields();
        }

        private void btCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomerInfoToFileds();
            DeactivateFields();
        }

        private void btConfirmUpdate_Click(object sender, RoutedEventArgs e)
        {
            //FIX exceptions
            //FIX validate fields
            //if fields are valid then
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

            EFData.context.SaveChanges();
            LoadCustomerInfoToFileds();
            DeactivateFields();
        }

        private void BackToManageCusts_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
