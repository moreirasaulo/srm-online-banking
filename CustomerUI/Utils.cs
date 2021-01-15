using SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomerUI
{
    public static class Utils
    {
        public static MainWindow mainWindow; // login window
        public static Window clientDashboard;

        public static Login login;
        public static Account selectedAcc;
        public static List<Transaction> userTransactions;
        public static List<string> transactionHistoryDays = new List<string> { "7 days", "30 days", "60 days" };
        public static List<User> Payees;
        public static List<string> paymentCategories = new List<string> { "Utility bills", "Education", "Groceries", "Government", "Transportation", "Leisure", "Other" };
        public static string emailForReceipts;
    }
}
