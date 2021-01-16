using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedCode;
using System;

namespace BankManagementUnitsTests
{
    [TestClass]
    public class TransactionDialogTests
    {
        BankManagementSys.TransactionDialog transDialog;
        User user;
        Account account;
        AccountType accType;

        [TestInitialize]
        public void TestInitialize()
        {
            accType = new AccountType { Id = 5, Description = "Checking" };
            user = new User
            {
                Id = 78,
                FirstName = "Jean",
                LastName = "Christophe",
                Gender = "male",
                Address = "78 Rose St",
                Country = "Canada",
                PostalCode = "H6G9T7",
                DateOfBirth = new DateTime(1970, 1, 1),
                City = "Montreal"
            };
            account = new Account {Id = 100, Balance = 900, AccountTypeId = 1, MonthlyFee = 10, InterestFeeDate = new DateTime(2021, 1, 1), OpenDate = new DateTime(2020, 12, 12), IsActive = true, UserId = 78, AccountType = accType };
            transDialog = new BankManagementSys.TransactionDialog(user, account, "Deposit");
        }

        [TestMethod]
        public void CalculateBalanceAfterDeposit()
        {
            Assert.AreEqual(transDialog.CalculateNewBalance("Deposit", 100, 10), 110);
            Assert.AreEqual(transDialog.CalculateNewBalance("Deposit", 0, 1000), 1000);
        }

        [TestMethod]
        public void CalculateBalanceAfterWidthdrawal()
        {
            Assert.AreEqual(transDialog.CalculateNewBalance("Withdrawal", 100, 10), 90);
            Assert.AreEqual(transDialog.CalculateNewBalance("Withdrawal", 500, 500), 0);
        }

        [TestMethod]
        public void CalculateBalanceAfterTransfer()
        {
            Assert.AreEqual(transDialog.CalculateNewBalance("Transfer", 750, 230), 520);
            Assert.AreEqual(transDialog.CalculateNewBalance("Transfer", 40, 23), 17);
        }

        [TestMethod]
        public void CalculateBalanceAfterPayment()
        {
            Assert.AreEqual(transDialog.CalculateNewBalance("Payment", 1060, 200), 860);
            Assert.AreEqual(transDialog.CalculateNewBalance("Payment", 300, 23), 277);
        }

        [TestMethod]
        public void CalculateBalanceAfterOtherTransType()
        {
            Assert.AreEqual(transDialog.CalculateNewBalance("Other", 600, 200), 600);
            Assert.AreEqual(transDialog.CalculateNewBalance("Other", 0, 40), 0);
        }
    }
}
