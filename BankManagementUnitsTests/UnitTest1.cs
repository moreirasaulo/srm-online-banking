using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedCode;
using System;

namespace BankManagementUnitsTests
{
    [TestClass]
    public class UnitTest1
    {
        BankManagementSys.TransactionDialog transDialog;
        User user;
        Account account;
        string type;

        [TestInitialize]
        public void TestInitialize()
        {
            user = new User();
            account = new Account();
            type = "Deposit";
            transDialog = new BankManagementSys.TransactionDialog(user, account, type);
        }
        [TestMethod]
        public void CalculateBalanceAfterDeposit()
        {
            Assert.AreEqual(transDialog.CalculateNewBalance(type, 100, 10), 90);
            Assert.AreEqual(transDialog.CalculateNewBalance(type, 1000, 1000), 0);
        }
    }
}
