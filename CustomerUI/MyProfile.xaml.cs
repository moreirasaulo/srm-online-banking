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
    /// Interaction logic for MyProfile.xaml
    /// </summary>
    public partial class MyProfile : Window
    {
        User currentUser;
        public MyProfile()
        {
            InitializeComponent();
            currentUser = Utils.login.User;

            if (currentUser.MiddleName != null)
            {
                tbFullName.Text = currentUser.FirstName + " " + currentUser.MiddleName + " " + currentUser.LastName;
            }
            else 
            {
                tbFullName.Text = currentUser.FirstName + " " + currentUser.LastName;
            }

            tbPhoneNo.Text = currentUser.PhoneNo;
            tbNationalID.Text = currentUser.NationalId;
            tbDOB.Text = currentUser.DateOfBirth.ToString();
            tbAddress.Text = currentUser.Address;
            tbCity.Text = currentUser.City;
            tbProvinceState.Text = currentUser.ProvinceState;
            tbPostalCode.Text = currentUser.PostalCode;
            tbCountry.Text = currentUser.Country;
            tbEmail.Text = currentUser.Email;
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            btUpdate.Content = "Confirm Changes";
            tbPhoneNo.BorderBrush = System.Windows.Media.Brushes.Green;
            tbEmail.BorderBrush = System.Windows.Media.Brushes.Green;
            tbPhoneNo.IsEnabled = true;
            tbEmail.IsEnabled = true;
        }
    }
}
