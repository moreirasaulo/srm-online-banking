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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerUI
{
    /// <summary>
    /// Interaction logic for CustProfile.xaml
    /// </summary>
    public partial class CustProfile : UserControl
    {
        User currentUser;
        public CustProfile()
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

            if (currentUser.CompanyName != null)
            {
                lblCompanyName.Visibility = Visibility.Visible;
                tbCompanyName.Visibility = Visibility.Visible;
                lblNatID.Content = "Register Number:";
                lblDOB.Content = "Registration Date:";
                tbCompanyName.Text = currentUser.CompanyName;
            }
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser.CompanyName != null)
            {
                if (btUpdate.Content.Equals("Update Contact Info"))
                {
                    btUpdate.Content = "Confirm Changes";
                    tbPhoneNo.BorderBrush = System.Windows.Media.Brushes.Green;
                    tbEmail.BorderBrush = System.Windows.Media.Brushes.Green;
                    tbPhoneNo.IsEnabled = true;
                    tbEmail.IsEnabled = true;
                }
                else if (btUpdate.Content.Equals("Confirm Changes"))
                {
                    currentUser.PhoneNo = tbPhoneNo.Text;
                    currentUser.Email = tbEmail.Text;
                    tbPhoneNo.BorderBrush = System.Windows.Media.Brushes.LightGray;
                    tbEmail.BorderBrush = System.Windows.Media.Brushes.LightGray;
                    tbPhoneNo.IsEnabled = false;
                    tbEmail.IsEnabled = false;
                    btUpdate.Content = "Update Contact Info";

                    try
                    {
                        EFData.context.SaveChanges();
                        MessageBox.Show("Contact information successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }               
            }
            else 
            {
                if (btUpdate.Content.Equals("Update Contact Info"))
                {
                    btUpdate.Content = "Confirm Changes";
                    tbPhoneNo.BorderBrush = System.Windows.Media.Brushes.Green;
                    tbEmail.BorderBrush = System.Windows.Media.Brushes.Green;
                    tbPhoneNo.IsEnabled = true;
                    tbEmail.IsEnabled = true;
                }
                else if (btUpdate.Content.Equals("Confirm Changes"))
                {
                    currentUser.PhoneNo = tbPhoneNo.Text;
                    currentUser.Email = tbEmail.Text;
                    tbPhoneNo.BorderBrush = System.Windows.Media.Brushes.LightGray;
                    tbEmail.BorderBrush = System.Windows.Media.Brushes.LightGray;
                    tbPhoneNo.IsEnabled = false;
                    tbEmail.IsEnabled = false;
                    btUpdate.Content = "Update Contact Info";

                    try
                    {
                        EFData.context.SaveChanges();
                        MessageBox.Show("Contact information successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }                        
            }            
        }
    }
}
