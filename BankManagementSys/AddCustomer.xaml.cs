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
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        public AddCustomer()
        {
            InitializeComponent();
        }

        private void AddCust_Click(object sender, RoutedEventArgs e)
        {
            //FIX VALIDATION
            // if fields are valid then
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

          /*  User user = new User
            {
                FirstName = tbFirstName.Text,
                MiddleName = tbMiddleName.Text,
                LastName = tbLastName.Text,
                Gender = gender,
                NationalId = tbNatId.Text,


            } */

        }
    }
}
