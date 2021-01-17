using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedCode;

namespace BankManagementUnitsTests
{
    [TestClass]
    class TransactionTests
    {
        Transaction transaction;
        Account account;
        AccountType accType;

        [TestInitialize]
        public void TestInitialize()
        {
            accType = new AccountType { Id = 5, Description = "Checking" };
            
            account = new Account { Id = 100, Balance = 900, AccountTypeId = 1, MonthlyFee = 10, InterestFeeDate = new DateTime(2021, 1, 1), OpenDate = new DateTime(2020, 12, 12), IsActive = true, UserId = 78, AccountType = accType };
            transaction = new Transaction
            {
                Id = 278,
                Date = new DateTime(2021, 1, 1),
                Amount = 200,
                Type = "Deposit",
                AccountId = account.Id,
            };
        }

        [TestMethod]
        public void CalculateBalanceAfterTransaction()
        {
            Assert.AreEqual(transaction.CalculateBalanceAfterParticularTransaction(), 110);
           // Assert.AreEqual(transDialog.CalculateNewBalance("Deposit", 0, 1000), 1000);
        }
    }
}
