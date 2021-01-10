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
    /// Interaction logic for SpendingReport.xaml
    /// </summary>
    public partial class SpendingReport : Window
    {
        Account currentAcc;
        public SpendingReport(Account account)
        {
            InitializeComponent();
            currentAcc = account;
        }

        private void ViewChart()
        {
            if (rbPieChart.IsChecked == true)
            {
                this.contentControl.Content = new PieChart(currentAcc);
            }
            else if (rbBarGraph.IsChecked == true)
            {
                this.contentControl.Content = new BarChart(currentAcc);
            }
            else
            {
                this.contentControl.Content = "Choose pie chart or bar graph to visualize data";
            }
        }


        private void btViewReport_Click(object sender, RoutedEventArgs e)
        {
            ViewChart();
        }

        private void rbPieChart_Checked(object sender, RoutedEventArgs e)
        {
            if(this.contentControl == null)
            {
                return;
            }
            ViewChart();
        }

        private void rbBarGraph_Checked(object sender, RoutedEventArgs e)
        {
            ViewChart();
        }
    }
}
