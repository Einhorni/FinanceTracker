using MoneyManagement.Models;
using MoneyManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.DataAccess
{
   
    public static class Mappings
    {
        //Extension Method auf AccountEntity mit this
        public static Account AccountEntityToAccount(this AccountEntity account)
        {
            var balance = account.Transactions
                        .Select(t => t.Amount).Sum();

            //ist entweder Bargeldkto oder Girokonto, in Zukunft noch andere
            if (account.KindOfAccount == "Bargeldkonto")
            {
                return
                    new Bargeldkonto
                    {
                        Name = account.Name,
                        Balance = balance,
                        Id = account.Id,
                        DateOfCreation = account.DateOfCreation,
                        Currency = account.Currency
                    };
            }

            if (account.KindOfAccount == "Girokonto")
            {
                return
                    new Girokonto
                    {
                        Name = account.Name,
                        Balance = balance,
                        Id = account.Id,
                        DateOfCreation = account.DateOfCreation,
                        Currency = account.Currency,
                        OverdraftLimit = account.Overdraft
                    };
            }

            //sollte nicht eintreten
            //else return null;
            {
                return
                    new Bargeldkonto
                    {
                        Name = "",
                        Balance = 0.0m,
                        Id = Guid.Empty,
                        DateOfCreation = DateTime.MinValue,
                        Currency = account.Currency
                    };
            };
        }


        public static AccountEntity AccountToAccountEntity(this Account accountDto)
        {
            if (accountDto == null) return null;

            if (accountDto is Bargeldkonto)
            {
                var bar = accountDto as Bargeldkonto;
                return new AccountEntity
                {
                    Name = bar.Name,
                    Currency = bar.Currency.ToString(),
                    KindOfAccount = nameof(Bargeldkonto),
                    DateOfCreation = bar.DateOfCreation,
                    Id = bar.Id
                };
            };

            if (accountDto is Girokonto)
            {
                var giro = accountDto as Girokonto;
                return new AccountEntity
                {
                    Name = giro.Name,
                    Currency = giro.Currency.ToString(),
                    KindOfAccount = nameof(Girokonto),
                    DateOfCreation = giro.DateOfCreation,
                    Id = giro.Id,
                    Overdraft = giro.OverdraftLimit,
                };
            }

            else return null;
            
        }


        public static TransactionEntity TransactionToTransactionEntity(this Transaction transactionDTO)
        {
            return new TransactionEntity
            {
                Id = transactionDTO.TransactionId,
                Amount = transactionDTO.Amount,
                CategoryName = transactionDTO.Category,
                Date = transactionDTO.Date,
                Title = transactionDTO.Title ?? string.Empty,
                AccountId = transactionDTO.AccountId,
                FromAccountId = transactionDTO.SendingAccountId,
                ToAccountId = transactionDTO.ReceivingAccountId
            };
        }


        public static Transaction TransactionEntityToTransaction(this TransactionEntity transactionEntity)
        {
            var transaction = new Transaction(transactionEntity.Amount, transactionEntity.CategoryName, transactionEntity.AccountId);
            transaction.TransactionId = transactionEntity.Id;
            transaction.SendingAccountId = transactionEntity.FromAccountId;
            transaction.ReceivingAccountId = transactionEntity.ToAccountId;
            transaction.Date = transactionEntity.Date;
            transaction.Title = transactionEntity.Title;
            return transaction;
        }
    }
}
