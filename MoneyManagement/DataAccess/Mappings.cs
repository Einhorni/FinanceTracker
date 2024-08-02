using MoneyManagement.Models;
using MoneyManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.DataAccess
{
   
    public class Mappings
    {
        public static AccountDTO AccountToAccountDto(Account account, decimal balance)
        {
            //ist entweder Bargeldkto oder Girokonto, in Zukunft noch andere
            if (account.KindOfAccount == "Bargeldkonto")
            {
                return
                    new Bargeldkonto
                    {
                        Name = account.Name,
                        Balance = balance,
                        Id = Guid.Parse(account.Id),
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
                        Id = Guid.Parse(account.Id),
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

        public static Account AccountDtoToAccount(AccountDTO accountDto)
        {
            if (accountDto == null) return null;

            if (accountDto is Bargeldkonto)
            {
                var bar = accountDto as Bargeldkonto;
                return new Account
                {
                    Name = bar.Name,
                    Currency = bar.Currency.ToString(),
                    KindOfAccount = nameof(Bargeldkonto),
                    DateOfCreation = bar.DateOfCreation,
                    Id = bar.Id.ToString()
                };
            };

            if (accountDto is Girokonto)
            {
                var giro = accountDto as Girokonto;
                return new Account
                {
                    Name = giro.Name,
                    Currency = giro.Currency.ToString(),
                    KindOfAccount = nameof(Girokonto),
                    DateOfCreation = giro.DateOfCreation,
                    Id = giro.Id.ToString(),
                    Overdraft = giro.OverdraftLimit,
                };
            }

            else return null;
            
        }

        public static TransactionDTO TransactionToTransactionDto(Transaction transaction)
        {
            return new TransactionDTO
            {
                TransactionId = Guid.Parse(transaction.Id),
                Amount = transaction.Amount,
                Title = transaction.Title,
                Date = transaction.Date,
                Category = transaction.Category,
                AccountId = Guid.Parse(transaction.AccountId),
                ToAccountId = Guid.Parse(transaction.ToAccountId),
                FromAccountId = Guid.Parse(transaction.FromAccountId)
                //TODO: Write CategoryMappings
            };
        }


        public static Transaction TransactionDtoToTransaction(TransactionDTO transactionDTO)
        {
            return new Transaction
            {
                Id = transactionDTO.TransactionId.ToString(),
                Amount = transactionDTO.Amount,
                Category = transactionDTO.Category,
                Date = transactionDTO.Date,
                Title = transactionDTO.Title ?? string.Empty,
                AccountId = transactionDTO.AccountId.ToString(),
                FromAccountId = transactionDTO.FromAccountId.ToString(),
                ToAccountId = transactionDTO.ToAccountId.ToString()
            };
        }


        public static MockCurrency MapToCurrency(string currencyString)
        {
            MockCurrency currency = new();
            switch (currencyString)
            {
                case "Dollar":
                    currency = MockCurrency.Dollar;
                    break;

                case "EUR":
                    currency = MockCurrency.EUR;
                    break;
            }
            return currency;
        }
    }
}
