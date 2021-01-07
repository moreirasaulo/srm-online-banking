using SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerUI
{
    public static class Utils
    {
        public static MainWindow mainWindow; // login window

        public static Login login;
        public static List<Transaction> userTransactions;
        public static List<string> transactionHistoryDays = new List<string> { "7 days", "30 days", "60 days" };
        public static List<User> Payees;
        public static List<string> paymentCategories = new List<string> { "Select category", "Utility bills", "Educations", "Groceiries", "Government", "Transportation", "Leisure" };
        public static string emailForReceipts;
    }
}
