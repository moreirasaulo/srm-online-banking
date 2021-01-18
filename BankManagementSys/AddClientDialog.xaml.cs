using SharedCode;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for AddClientDialog.xaml
    /// </summary>
    public partial class AddClientDialog : Window
    {
        bool LoadNewCustomer;
        public AddClientDialog()
        {
            InitializeComponent();
            comboCountry.ItemsSource = Utilities.Countries;
            comboCountry.SelectedIndex = 0;
            Wizard.FinishButtonContent = "Add customer";
            Wizard.CancelButtonClosesWindow = false;
            LoadNewCustomer = true;
        }


        private void Wizard_Next(object sender, Xceed.Wpf.Toolkit.Core.CancelRoutedEventArgs e)
        {
            //page 1 (customer type)
            if (Wizard.CurrentPage == Page1 && rbCustCatIndividual.IsChecked == false && rbCustCatCompany.IsChecked == false)
            {
                MessageBox.Show("Please choose the type of customer", "Selection required", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Cancel = true;
            }

            //page 2 (full name +- company name)
            if (Wizard.CurrentPage == Page2)
            { 
                if (tbFirstName.Text.Length < 1 || tbFirstName.Text.Length > 20)
                {
                    MessageBox.Show("First name must contain between 1 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (tbMiddleName.Text.Length > 20)
                {
                    MessageBox.Show("Middle name must containt not more than 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (tbLastName.Text.Length < 1 || tbLastName.Text.Length > 20)
                {
                    MessageBox.Show("Last name must containt between 1 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);                   
                    e.Cancel = true;
                    return;
                }
                if (rbGenderMale.IsChecked == false && rbGenderFemale.IsChecked == false && rbGenderOther.IsChecked == false)
                {
                    MessageBox.Show("Please choose gender of customer", "Selection required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    e.Cancel = true;
                    return;
                }
                if (rbCustCatCompany.IsChecked == true && (tbCompanyName.Text.Length < 1 || tbCompanyName.Text.Length > 70))
                {
                    MessageBox.Show("Company name must contain between 1 and 70 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
            }

            //page 3 (nat id/company reg number and date of birth/company reg date)
            if (Wizard.CurrentPage == Page3)
            {
                if (tbNatId.Text.Length < 5 || tbNatId.Text.Length > 20)
                {
                    MessageBox.Show("National Id/Company registration Id number must containt between 5 and 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (dpBirthday.SelectedDate == null)
                {
                    MessageBox.Show("Please select date of birth/date of company registration", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (dpBirthday.SelectedDate > DateTime.Now)
                {
                    MessageBox.Show("Date of birth/company registration date must be earlier than today's date", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
            }

            //page 4 (contact information)
            if(Wizard.CurrentPage == Page4)
            {
                if (!Regex.IsMatch(tbPhoneNo.Text, @"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$"))
                {
                    MessageBox.Show("Please enter a valid phone number using the following format: xxx-xxx-xxxx", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (tbAddress.Text.Length < 5 || tbAddress.Text.Length > 50)
                {
                    MessageBox.Show("Address must contain between 5 and 50 caracters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (tbCity.Text.Length < 2 || tbCity.Text.Length > 20)
                {
                    MessageBox.Show("City must contain between 2 and 20 caracters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (tbProvinceState.Text.Length > 20)
                {
                    MessageBox.Show("Province or State must have not more than 20 caracters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (tbPostalCode.Text.Length < 5 || tbPostalCode.Text.Length > 10)
                {
                    MessageBox.Show("Postal code must be made of 5 to 10 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (comboCountry.Text.Length > 20)
                {
                    MessageBox.Show("Country must contain maximum 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (comboCountry.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select country", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (tbEmail.Text.Length > 60)
                {
                    MessageBox.Show("E-mail must contain maximum 60 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
            }

            // online bank
            if (Wizard.CurrentPage == Page5)
            {
                if (rbOnlineBankYes.IsChecked == false && rbOnlineBankNo.IsChecked == false)
                {
                    MessageBox.Show("Please choose one of the options", "Selection required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    e.Cancel = true;
                }
                if(rbOnlineBankYes.IsChecked == true)
                {
                    if (tbUsername.Text.Length < 5 || tbUsername.Text.Length > 20)
                    {
                        MessageBox.Show("Username must be made of 5 to 20 characters", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true;
                        return;
                    }
                    string username = null;
                    try
                    {
                        username = (from l in EFData.context.Logins
                                                          where l.Username == tbUsername.Text
                                                          select l.Username).FirstOrDefault();
                    }
                    catch(SystemException ex)
                    {
                        MessageBox.Show("Error fetching from database: " + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (username != null)
                    {
                        MessageBox.Show("This username already exists, please choose another one.", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true;
                        return;
                    }
                    if (tbPassword.Text.Length < 8 || tbPassword.Text.Length > 20)
                    {
                        MessageBox.Show("Password must be made of 8 to 20 characters.", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true;
                        return;
                    }
                }
            }

                //summary
                if (Wizard.CurrentPage == Page5)
            {
                lblSumFirstName.Content = tbFirstName.Text;
                lblSumMidName.Content = tbMiddleName.Text;
                lblSumLastName.Content = tbLastName.Text;
                string gender = "";
                if (rbGenderMale.IsChecked == true)
                {
                    gender = "Male";
                }
                else if (rbGenderFemale.IsChecked == true)
                {
                    gender = "Female";
                }
                else if (rbGenderOther.IsChecked == true)
                {
                    gender = "Other";
                }
                lblSumGender.Content = gender;
                lblSumCompName.Content = tbCompanyName.Text;
                lblSumDateOfBirth.Content = dpBirthday.Text;
                lblSumNatId.Content = tbNatId.Text;
                lblSumPhoneNo.Content = tbPhoneNo.Text;
                lblSumEmail.Content = tbEmail.Text;
                lblSumAddress.Content = tbAddress.Text;
                lblSumCity.Content = tbCity.Text;
                lblSumPostalCode.Content = tbPostalCode.Text;
                lblSumProvinceState.Content = tbProvinceState.Text;
                lblSumCountry.Content = comboCountry.Text;
                lblSumUsername.Content = tbUsername.Text;
            }
        }

        private void rbCustCatIndividual_Checked(object sender, RoutedEventArgs e)
        {
            if(lblCompanyRep == null) { return; }
            lblCompanyRep.Visibility = Visibility.Hidden;  //company representative title
            Page3.Title = "National Id and date of birth";
            lblNatIdCompRegNo.Content = "National Id: *";
            lblDateBirthOrRegist.Content = "Date of birth: *";
            lblCompName.Content = "";
            tbCompanyName.Visibility = Visibility.Hidden;
            lblSummaryCompInfo.Content = "";
            lblSummaryCompInfo.Content = "";
            lblSumCompName.Content = "";
            lblSummaryDateOfBirth.Content = "Date of birth:";
            lblSummaryNatId.Content = "National Id:";
            tbCompanyName.Text = "";
        }

        private void rbCustCatCompany_Checked(object sender, RoutedEventArgs e)
        {
            lblCompanyRep.Visibility = Visibility.Visible;  //company representative title
            Page3.Title = "Company registration number and date of registration";
            lblNatIdCompRegNo.Content = "Company registration number: *";
            lblDateBirthOrRegist.Content = "Company registration date: *";
            lblCompName.Visibility = Visibility.Visible;
            lblCompName.Content = "Company name: *";
            tbCompanyName.Visibility = Visibility.Visible;
            lblSummaryCompInfo.Content = "Company name:";
            lblSummaryDateOfBirth.Content = "Company reg date:";
            lblSummaryNatId.Content = "Company reg no:";
        }

        private void Wizard_Finish(object sender, Xceed.Wpf.Toolkit.Core.CancelRoutedEventArgs e)
        {
            string gender = "";
            if (rbGenderMale.IsChecked == true)
            {
                gender = "Male";
            }
            else if (rbGenderFemale.IsChecked == true)
            {
                gender = "Female";
            }
            else if (rbGenderOther.IsChecked == true)
            {
                gender = "Other";
            }
            User user = null;
            Login login = null;
            try
            {
                user = new User();

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
                if (rbCustCatCompany.IsChecked == true)
                {
                    user.CompanyName = tbCompanyName.Text;
                }
                string message = string.Format("Add new customer {0} ?", user.FullName);
                MessageBoxResult answer = MessageBox.Show(message, "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    EFData.context.Users.Add(user);
                    string successMessage = string.Format("New customer {0} {1} added successfully", user.FirstName, user.LastName);
                    if (rbOnlineBankYes.IsChecked == true)
                    {

                        login = new Login
                        {
                            Username = tbUsername.Text,
                            Password = tbPassword.Text,
                            UserTypeId = 3,
                            UserId = user.Id
                        };
                        EFData.context.Logins.Add(login);
                        successMessage = successMessage + ",\nonline bank with Username: " + login.Username + " was created";
                    }
                    EFData.context.SaveChanges();
                    LoadNewCustomer = true;
                    MessageBox.Show(successMessage, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                if (answer == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    LoadNewCustomer = false;
                    return;
                }

            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                MessageBox.Show(error.ErrorMessage);
                EFData.context.Entry(user).State = EntityState.Detached;
                if (login != null)
                {
                    EFData.context.Entry(login).State = EntityState.Detached;
                }
                return;
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rbOnlineBankYes_Checked(object sender, RoutedEventArgs e)
        {
            lblUsername.Content = "Create username: *";
            tbUsername.Visibility = Visibility.Visible;
            lblPassword.Content = "Create password: *";
            tbPassword.Visibility = Visibility.Visible;
            lblSummaryUsername.Content = "Username:";
            lblRequired5.Visibility = Visibility.Visible;
        }

        private void rbOnlineBankNo_Checked(object sender, RoutedEventArgs e)
        {
            if (lblUsername == null) { return; }
            lblUsername.Content = "";
            tbUsername.Visibility = Visibility.Hidden;
            lblPassword.Content = "";
            tbPassword.Visibility = Visibility.Hidden;
            tbUsername.Text = "";
            tbPassword.Text = "";
            lblSummaryUsername.Content = "";
            lblSumUsername.Content = "";
        }

        private void Wizard_Cancel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Exit 'Add new customer' wizard?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                LoadNewCustomer = false;
                Wizard.CancelButtonClosesWindow = true; 
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LoadNewCustomer == true)
            {
                DialogResult = true;
            }
        }
    }
}
