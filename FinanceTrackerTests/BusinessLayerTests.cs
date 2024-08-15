using MoneyManagement;
using MoneyManagement.Models;
using System.Xml.Linq;
using MoneyManagement.Entities;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using MoneyManagement.BusinessModels;

namespace FinanceTrackerTests
{

    public class BusinessLayerTests
    {
        #region Business Model Tests

        [Fact]
        public void ChangeAmount_ChangesAmount()
        {
            //Arrange
            Account acc = new Girokonto("name", 70.99m, "Euro", Guid.NewGuid());
            //Act
            acc.ChangeAmount(10.0m);
            //Assert
            Assert.Equal(80.99m, acc.Balance);
        }


        [Fact]
        public void TransactionValid_TransactionIfBalanceAndOverdraftMoreThanTransactionAmount()
        {
            Account acc = new Girokonto("name", 70.0m, "Euro", Guid.NewGuid(), 10.0m);
            Transaction testTransaction = new Transaction(-79.0m, "TestCat", Guid.NewGuid());

            var validation = acc.TransactionValid(testTransaction);

            Assert.True(validation);
        }


        [Fact]
        public void TransactionValid_TransactionIfBalanceAndOverdraftEqualThanTransactionAmount()
        {
            Account acc = new Girokonto("name", 70.0m, "Euro", Guid.NewGuid(), 10.0m);
            Transaction testTransaction = new Transaction(-80.0m, "TestCat", Guid.NewGuid());

            var validation = acc.TransactionValid(testTransaction);

            Assert.True(validation);
        }


        [Fact]
        public void TransactionValid_NoTransactionIfBalanceAndOverdraftLessThanTransactionAmount()
        {
            Account acc = new Girokonto("name", 70.0m, "Euro", Guid.NewGuid(), 10.0m);
            Transaction testTransaction = new Transaction(-81.0m, "TestCat", Guid.NewGuid());

            var validation = acc.TransactionValid(testTransaction);

            Assert.False(validation);
        }


        [Fact]
        public void TransactionValid_NoTransactionIfBalanceLessThanTransactionAmount()
        {
            Account acc = new Bargeldkonto("name", 70.0m, "Euro", Guid.NewGuid());
            Transaction testTransaction = new Transaction(-71.0m, "TestCat", Guid.NewGuid());

            var validation = acc.TransactionValid(testTransaction);

            Assert.False(validation);
        }


        [Fact]
        public void TransactionValid_TransactionIfBalanceEqualThanTransactionAmount()
        {
            Account acc = new Bargeldkonto("name", 70.0m, "Euro", Guid.NewGuid());
            Transaction testTransaction = new Transaction(-70.0m, "TestCat", Guid.NewGuid());

            var validation = acc.TransactionValid(testTransaction);

            Assert.True(validation);
        }


        [Fact]
        public void TransactionValid_TransactionIfBalanceMoreThanTransactionAmount()
        {
            Account acc = new Bargeldkonto("name", 70.0m, "Euro", Guid.NewGuid());
            Transaction testTransaction = new Transaction(-69.0m, "TestCat", Guid.NewGuid());

            var validation = acc.TransactionValid(testTransaction);

            Assert.True(validation);
        }

        #endregion
    }
}