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
            calendarMonthYear.SelectedDate = new DateTime(1983, 01, 01);
        }

        private void Calendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            //DateTime monthYear = new DateTime();
            if (calendarMonthYear.DisplayMode == CalendarMode.Month)
            {
                calendarMonthYear.DisplayMode = CalendarMode.Year;                
            }

            //calendarMonthYear.SelectedDate = new DateTime(2019, 01, 01);
            /*if (calendarMonthYear.DisplayMode == CalendarMode.Month)
            {

                calendarMonthYear.DisplayMode = CalendarMode.Year;
                //monthYear. = calendarMonthYear.SelectedDate;
                //MessageBox.Show(monthYear.Year + ""); // 1
                //calendarMonthYear.SelectedDate.
                DateTime date = calendarMonthYear.SelectedDate.Value;
                calendarMonthYear.SelectedDate = new DateTime(2019, 12, 1);
                MessageBox.Show(calendarMonthYear.SelectedDate.ToString()); // empty
                if (calendarMonthYear.SelectedDate.HasValue) 
                {
                    MessageBox.Show(calendarMonthYear.SelectedDate.Value.ToString("dd/MM/yyyy"));
                }

                
                 if (calendarMonthYear. == Calendar.D) 
                {
                    Calendar.DisplayModeProperty == Microsoft.Windows.Controls.CalendarMode.Year
                }
                 
            }*/

        }

        private void calendarMonthYear_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
                DateTime date = calendarMonthYear.SelectedDate.Value;                 
        }
    }
}
