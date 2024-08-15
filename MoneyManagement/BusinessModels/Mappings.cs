using MoneyManagement.Models;
using MoneyManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.BusinessModels
{

    public static class Mappings
    {
        public const string bAccount = "Bargeldkonto";
        public const string gAccount = "Girokonto";

        //Extension Method auf AccountEntity mit this
        // CodeReview: nicht glütigen Fall mit Exception quittieren. Beispiel: NotImplementException
        public static Account AccountEntityToAccount(this AccountEntity account)
        {
            var balance = account.Transactions
                        .Select(t => t.Amount).Sum();

            if (account.KindOfAccount == bAccount)
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

            if (account.KindOfAccount == gAccount)
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

            {
                throw new NotImplementedException();
            };
        }


        public static AccountEntity AccountToAccountEntity(this Account accountDto)
        {
            if (accountDto == null)
            {
                throw new ArgumentNullException(nameof(accountDto));
            }

            if (accountDto is Bargeldkonto bar)
            {
                return new AccountEntity
                {
                    Name = bar.Name,
                    Currency = bar.Currency.ToString(),
                    KindOfAccount = bAccount,
                    DateOfCreation = bar.DateOfCreation,
                    Id = bar.Id
                };
            };

            if (accountDto is Girokonto giro)
            {
                return new AccountEntity
                {
                    Name = giro.Name,
                    Currency = giro.Currency.ToString(),
                    KindOfAccount = gAccount,
                    DateOfCreation = giro.DateOfCreation,
                    Id = giro.Id,
                    Overdraft = giro.OverdraftLimit,
                };
            }

            {
                throw new NotImplementedException();
            };
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
            return new Transaction(transactionEntity.Amount, transactionEntity.CategoryName, transactionEntity.AccountId)
            {
                TransactionId = transactionEntity.Id,
                SendingAccountId = transactionEntity.FromAccountId,
                ReceivingAccountId = transactionEntity.ToAccountId,
                Date = transactionEntity.Date,
                Title = transactionEntity.Title
            };
        }
    }
}
