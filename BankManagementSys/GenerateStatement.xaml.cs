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
    /// Interaction logic for GenerateStatement.xaml
    /// </summary>
    public partial class GenerateStatement : Window
    {
        public GenerateStatement(Account currentAccount)
        {
            InitializeComponent();
        }

        /*private void Calendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            if (Calendar.DisplayModeProperty = CalendarMode.Month) 
            {
                Calendar.DisplayModeProperty == Microsoft.Windows.Controls.CalendarMode.Year
            }
        }*/
    }
}
