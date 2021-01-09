using SharedCode;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
    /// Interaction logic for UserTypeValidationTest.xaml
    /// </summary>
    public partial class UserTypeValidationTest : Window
    {
        public UserTypeValidationTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserType userType = null;
            try
            {
                // EFData.context.SaveChanges(); // this must not be necessary
                userType = new UserType();
                userType.Description = tbDescription.Text;
                EFData.context.UserTypes.Add(userType);
                EFData.context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                MessageBox.Show(error.ErrorMessage);
                EFData.context.Entry(userType).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some other exception: " + ex.Message);
            }
        }
    }
}
