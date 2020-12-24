using SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerUI
{
    public class Utils
    {
        public static Login login;
        public static List<Transaction> userTransactions;
        public static List<string> transactionHistoryDays = new List<string> { "7 days", "30 days", "60 days" };
    }
}
