using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SharedCode;

namespace AccountTests
{
    [TestClass]
    public class ManageAccountsTests
    {
        BankManagementSys.ManageAccounts manageAccountsDlg;
        Account account;
        AccountType accType;

        [TestInitialize]
        public void TestInitialize()
        {
            manageAccountsDlg = new BankManagementSys.ManageAccounts();
            accType = new AccountType { Id = 5, Description = "Checking" };

            account = new Account { Id = 100, Balance = 900, AccountTypeId = 1, MonthlyFee = 10, InterestFeeDate = new DateTime(2021, 1, 1), OpenDate = new DateTime(2020, 12, 12), IsActive = true, UserId = 78, AccountType = accType };

        }

        [TestMethod]
        public void PositiveBalance_ReturnsTrue()
        {
            var result = manageAccountsDlg.IsBalanceSufficient(190);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ZeroBalance_ReturnsFalse()
        {
            var result = manageAccountsDlg.IsBalanceSufficient(0);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NegativeBalance_ReturnsFalse()
        {
            var result = manageAccountsDlg.IsBalanceSufficient(-100);

            Assert.IsFalse(result);
        }
    }
}
