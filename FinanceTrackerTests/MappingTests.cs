using MoneyManagement.BusinessModels;
using MoneyManagement.Entities;
using MoneyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTrackerTests
{
    public class MappingTests
    {
        Guid kontoGuid = Guid.NewGuid();
        Guid transactionGuid = Guid.NewGuid();
        Guid fromKontoGuid = Guid.NewGuid();
        Guid toKontoGuid = Guid.NewGuid();
        DateTime dateTimeKonto = DateTime.Now;
        DateTime dateTimeTransaction = DateTime.Now;


        public Girokonto NewGirokonto()
        {
            var giro = new Girokonto("name", 70.0m, "Euro", kontoGuid, 10.0m);
            giro.DateOfCreation = dateTimeKonto;
            return giro;
        }

        public Bargeldkonto NewBarkonto()
        {
            var bar = new Bargeldkonto("name", 70.0m, "Euro", kontoGuid);
            bar.DateOfCreation = dateTimeKonto;
            return bar;
        }

        public AccountEntity NewAccountEntityForGiro()
        {
            var accountEntity = new AccountEntity();
            accountEntity.Id = kontoGuid;
            accountEntity.DateOfCreation = dateTimeKonto;
            accountEntity.Name = "name";
            accountEntity.Currency = "Euro";
            accountEntity.Overdraft = 10.0m;
            accountEntity.InterestRate = 0.0;
            accountEntity.InterestInterval = 0;
            accountEntity.InvestmentDuration = 0;
            accountEntity.KindOfAccount = "Girokonto";

            return accountEntity;
        }


        public AccountEntity NewAccountEntityForBar()
        {
            var accountEntity = new AccountEntity();
            accountEntity.Id = kontoGuid;
            accountEntity.DateOfCreation = dateTimeKonto;
            accountEntity.Name = "name";
            accountEntity.Currency = "Euro";
            accountEntity.Overdraft = 10.0m;
            accountEntity.InterestRate = 0.0;
            accountEntity.InterestInterval = 0;
            accountEntity.InvestmentDuration = 0;
            accountEntity.KindOfAccount = "Bargeldkonto";

            return accountEntity;
        }

        public TransactionEntity NewTransactionEntity(AccountEntity testAccountEntity)
        {
            var testTransactionEntity = new TransactionEntity();
            testTransactionEntity.Id = transactionGuid;
            testTransactionEntity.Title = "Title";
            testTransactionEntity.Amount = 70.0m;
            testTransactionEntity.Date = dateTimeTransaction;
            testTransactionEntity.CategoryName = "testCat";
            testTransactionEntity.Account = testAccountEntity;
            testTransactionEntity.AccountId = kontoGuid;
            testTransactionEntity.FromAccountId = fromKontoGuid;
            testTransactionEntity.ToAccountId = toKontoGuid;

            return testTransactionEntity;
        }


        public Transaction NewTransaction(TransactionEntity testTransactionEntity)
        {
            var testTransaction = new Transaction(70.0m, "testCat", testTransactionEntity.AccountId);
            testTransaction.TransactionId = testTransactionEntity.Id;
            testTransaction.Title = "Title";
            testTransaction.SendingAccountId = testTransactionEntity.FromAccountId;
            testTransaction.ReceivingAccountId = testTransactionEntity.ToAccountId;
            testTransaction.Date = testTransactionEntity.Date;

            return testTransaction;
        }


        #region Mappings Entity to Giro / Barkonto


        [Fact]
        public void AccountEntityToAccount_MapsToGiroAccount()
        {
            var testAccountEntity = NewAccountEntityForGiro();

            var testTransactionEntity = NewTransactionEntity(testAccountEntity);

            testAccountEntity.Transactions = [testTransactionEntity];

            var account = Mappings.AccountEntityToAccount(testAccountEntity);
            var testAccount = account as Girokonto;

            Assert.Equal(testAccountEntity.Id, testAccount.Id);
            Assert.Equal(testAccountEntity.DateOfCreation, testAccount.DateOfCreation);
            Assert.Equal(testAccountEntity.Name, testAccount.Name);
            Assert.Equal(testAccountEntity.Currency, testAccount.Currency);
            Assert.Equal(testAccountEntity.Overdraft, testAccount.OverdraftLimit);
        }


        [Fact]
        public void AccountEntityToAccount_MapsToBarAccount()
        {
            var testAccountEntity = NewAccountEntityForBar();

            var testTransactionEntity = NewTransactionEntity(testAccountEntity);

            testAccountEntity.Transactions = [testTransactionEntity];

            var account = Mappings.AccountEntityToAccount(testAccountEntity);
            var testAccount = account as Bargeldkonto;

            Assert.Equal(testAccountEntity.Id, testAccount.Id);
            Assert.Equal(testAccountEntity.DateOfCreation, testAccount.DateOfCreation);
            Assert.Equal(testAccountEntity.Name, testAccount.Name);
            Assert.Equal(testAccountEntity.Currency, testAccount.Currency);
        }
        #endregion

        #region Mapping Bar / Giro To Entity

        [Fact]
        public void AccountToAccountEntity_MapsBargeldKontoToAccountEntity()
        {
            var bar = NewBarkonto();

            var accountEntity = Mappings.AccountToAccountEntity(bar);

            Assert.Equal(bar.Id, accountEntity.Id);
            Assert.Equal(bar.DateOfCreation, accountEntity.DateOfCreation);
            Assert.Equal(bar.Name, accountEntity.Name);
            Assert.Equal(bar.Currency, accountEntity.Currency);
        }


        [Fact]
        public void AccountToAccountEntity_MapsGirokontoToAccountEntity()
        {
            var giro = NewGirokonto();

            var accountEntity = Mappings.AccountToAccountEntity(giro);

            Assert.Equal(giro.Id, accountEntity.Id);
            Assert.Equal(giro.DateOfCreation, accountEntity.DateOfCreation);
            Assert.Equal(giro.Name, accountEntity.Name);
            Assert.Equal(giro.Currency, accountEntity.Currency);
            Assert.Equal(giro.OverdraftLimit, accountEntity.Overdraft);
        }
        #endregion

        #region Transaction Mapping

        [Fact]
        public void TransactionToTransactionEntity_DoesWhatItSays()
        {
            var testAccountEntity = NewAccountEntityForGiro();

            var testTransactionEntity = NewTransactionEntity(testAccountEntity);

            //testAccountEntity.Transactions = [testTransactionEntity];

            var transaction = NewTransaction(testTransactionEntity);

            var transactionEntity = Mappings.TransactionToTransactionEntity(transaction);

            Assert.Equal(transactionEntity.Id, transaction.TransactionId);
            Assert.Equal(transactionEntity.Title, transaction.Title);
            Assert.Equal(transactionEntity.Amount, transaction.Amount);
            Assert.Equal(transactionEntity.Date, transaction.Date);
            Assert.Equal(transactionEntity.CategoryName, transaction.Category);
            Assert.Equal(transactionEntity.AccountId, transaction.AccountId);
            Assert.Equal(transactionEntity.FromAccountId, transaction.SendingAccountId);
            Assert.Equal(transactionEntity.ToAccountId, transaction.ReceivingAccountId);
        }


        [Fact]
        public void TransactionEntityToTransaction_DoesWhatIsSays()
        {
            var giro = NewGirokonto();
            var testAccountEntity = NewAccountEntityForGiro();

            var testTransactionEntity = NewTransactionEntity(testAccountEntity);

            var transaction = Mappings.TransactionEntityToTransaction(testTransactionEntity);

            Assert.Equal(transaction.TransactionId, testTransactionEntity.Id);
            Assert.Equal(transaction.Title, testTransactionEntity.Title);
            Assert.Equal(transaction.Amount, testTransactionEntity.Amount);
            Assert.Equal(transaction.AccountId, testTransactionEntity.AccountId);
            Assert.Equal(transaction.Category, testTransactionEntity.CategoryName);
            Assert.Equal(transaction.ReceivingAccountId, testTransactionEntity.ToAccountId);
            Assert.Equal(transaction.SendingAccountId, testTransactionEntity.FromAccountId);
            Assert.Equal(transaction.Date, testTransactionEntity.Date);

        }

        #endregion
    }
}
