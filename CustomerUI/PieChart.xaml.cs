using SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
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
    /// Interaction logic for PieChart.xaml
    /// </summary>
    public partial class PieChart : UserControl
    {
        Account currentAcc;
        List<Transaction> transactions;
        public PieChart(Account account, List<Transaction> transList)
        {
            InitializeComponent();
            currentAcc = account;
            transactions = transList;
            LoadPieChartData();
        }

        private void LoadPieChartData()
        {

            List<decimal> amounts = new List<decimal>();


            foreach (string pc in Utils.paymentCategories)
            {
                var transacByCat = transactions.FindAll(t => t.PaymentCategory == pc);
                decimal sum = 0;
                foreach (Transaction t in transacByCat)
                {
                    sum = sum + t.Amount;
                }
                amounts.Add(sum);
            }

            var CategoryAmount = new List<KeyValuePair<string, decimal>>();

                CategoryAmount = Enumerable.Range(0, Utils.paymentCategories.Count)
                .Select(i => new KeyValuePair<string, decimal>(Utils.paymentCategories[i], amounts[i]))
                .ToList();

                ((PieSeries)mcChart.Series[0]).ItemsSource = CategoryAmount;


        }
    }
}
