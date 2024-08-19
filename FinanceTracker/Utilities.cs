using FinanceTracker.UIMappings;
using MoneyManagement.Models;
using MoneyManagement;
using Microsoft.Identity.Client;
using CSharpFunctionalExtensions;
using Serilog;
using System.Linq.Expressions;

namespace FinanceTrackerConsole.Utilities
{
    internal class Utilities
    {
        public static string GetOverDraftLimit()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Enter the overdraft limit (xx.xx) and hit enter.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            string overdraftLimit = Console.ReadLine() ?? String.Empty;
            return overdraftLimit;
        }


        public static string GetNewAccountType()
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.WriteLine("1 - Cash Account");
            Console.WriteLine("2 - Bank Account");
            Console.WriteLine("9 - Main Menu");
            Console.WriteLine();
            string accountType = Console.ReadLine() ?? String.Empty;
            return accountType;
        }


        public static string GetAccoutName()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("To enter a new Account, type in the name and hit enter.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("9 - Main Menu");
            Console.WriteLine();
            string name = Console.ReadLine() ?? String.Empty;
            Console.WriteLine();
            return name;
        }


        public static string GetAccountBalance()
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Enter current balance (xx.xx) and hit enter.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            string balanceString = Console.ReadLine() ?? String.Empty;
            return balanceString;
        }


        public static string GetAccoutCurreny()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine("Enter a currency:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Euro = e");
            Console.WriteLine("US Dollar = d");
            Console.WriteLine();
            string currencyString = Console.ReadLine() ?? String.Empty;

            if (currencyString.ToUpperInvariant() == "e" || currencyString.ToUpperInvariant() == "d")
            {
                var currency = UIMappings.MapToCurrencyString(currencyString);
                return currency;
            }
            else
            {
                return String.Empty;
            }
        }


        public static (string, List<string>) GetChosenCategoryNumberStringAndAmountOfCategories(List<Category> categories)
        {
            const string transferItem = "Transfer";
            
            var listedCategories = categories
                .Where(c => c.Expense && c.Name != transferItem)
                .Select(c => c.Name).ToList();

            ListCatgories(listedCategories, "expense category");

            Console.WriteLine();
            string categoryNumberString = Console.ReadLine() ?? String.Empty;

            return (categoryNumberString, listedCategories);
        }


        public static void ListCatgories(List<string> categories, string typeOfCategory)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine();
            Console.WriteLine($"What kind of {typeOfCategory}:");
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i <= categories.Count - 1; i++)
            {
                Console.WriteLine($"{i + 1} - {categories[i]}");
            }
            Console.WriteLine();
        }


        public static string GetTransactionAmount(string kindOfAmount)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Enter {kindOfAmount}.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            var incomeString = Console.ReadLine() ?? String.Empty;

            return incomeString;
        }


        public static List<Transaction> GetAccountTransactions(Account account, List<Transaction> transactions)
        {
            var accountTransactions =
                    transactions
                        .Where(t => t.SendingAccountId == account.Id || t.ReceivingAccountId == account.Id || t.AccountId == account.Id)
                        .OrderByDescending(t => t.Date)
                        .Take(10)
                        .ToList();
            return accountTransactions;
        }


        public static string GetChosenAccountStringForTransfer(List<Account> accounts, string fromOrTo)
        {
            Console.WriteLine("------------------------------");
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1} - Transfer {fromOrTo} {accounts[i].Name}, {accounts[i].Balance} {accounts[i].Currency}");
            }
            Console.WriteLine();

            return Console.ReadLine() ?? String.Empty;
        }


        public static string GetTransferAmount(Account fromAccount, Account toAccount)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Transfer between Account {fromAccount.Name} to {toAccount.Name}");
            Console.WriteLine($"Enter the amount (xx.xx) (max. {fromAccount.Balance} possible)");
            Console.WriteLine();
            return Console.ReadLine() ?? String.Empty;
        }


        public static void ShowNotEnoughMoney()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------");
            Console.WriteLine("You don't have enough money");
            Console.WriteLine("No transaction made");
        }


        public static bool ValidateAmount(decimal amount, Account account)
        {
            if (account is Bargeldkonto && account.Balance < amount ||
                account is Girokonto gaccount && (account.Balance + gaccount.OverdraftLimit < amount))
            {
                ShowNotEnoughMoney();
                return false;
            }
            else
                return true;
        }


        public async static Task ShowTransactions(Account account, List<Transaction> transactions, MoneyManagementService accountManager)
        {
            List<Transaction> accountTransactions = GetAccountTransactions(account, transactions);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine($"This is Account {account.Name}. It's Balance is {account.Balance} {account.Currency}.");
            Console.WriteLine($"Last transactions:");
            foreach (Transaction? transaction in accountTransactions)
            {
                if (transaction is null)
                {
                    Console.WriteLine("Transaction could not be loaded");
                    break;
                }


                const string transfer = "Transfer";

                //show special text with AccountName for "Transfer" category.
                string otherAccountText = "";
                if (transaction.Category == transfer && !(transaction.SendingAccountId is null) && !(transaction.ReceivingAccountId is null))
                {
                    if (transaction.SendingAccountId != account.Id)
                    {
                        var otherAccount = await accountManager.LoadAccount(transaction.SendingAccountId.Value);
                        if (otherAccount.HasNoValue)
                            otherAccountText = "from another account (could not be loaded)";
                        else otherAccountText = $"from {otherAccount.Value.Name}";
                    }
                    else if (transaction.ReceivingAccountId != account.Id)
                    {
                        var otherAccount = await accountManager.LoadAccount(transaction.ReceivingAccountId.Value);
                        if (otherAccount.HasNoValue)
                            otherAccountText = "to another account (could not be loaded)";
                        else otherAccountText = $"to {otherAccount.Value.Name}";
                    }
                }

                string date = transaction.Date.ToString("dddd, dd.MMMM yyyy HH:mm:ss");
                Console.WriteLine($"{date}: {transaction.Amount}, {transaction.Category} {otherAccountText}");

            }
            Console.ForegroundColor = ConsoleColor.White;
        }


        public static void ShowAccounts(List<Account> accounts)
        {
            Console.WriteLine();

            for (int i = 0; i < accounts.Count; i++)
            {
                string accountType;
                if (accounts[i] as Bargeldkonto != null)
                    accountType = "Bargeldkonto";
                else if (accounts[i] as Girokonto != null)
                    accountType = "Girokonto";
                else
                    accountType = "";

                string overdraftLimit;
                if (accounts[i] is Girokonto gAccount)
                {
                    overdraftLimit = $", Overdraft-Limit = {gAccount.OverdraftLimit}";
                }
                else { overdraftLimit = ""; }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{i + 1}. {accountType}: {accounts[i].Name}, Balance {accounts[i].Balance} {accounts[i].Currency.ToString()} {overdraftLimit}");
            }
        }


        public async static Task<Result> SaveSingleTransaction(decimal amount, string transactionCategory, Account account, MoneyManagementService accountManager)
        {
                Transaction newTransaction = new(amount, transactionCategory, account.Id);
                account.ChangeAmount(amount);
                return await accountManager.SaveTransactions([newTransaction]);
        }


        public async static Task SaveTransferTransactions(string amountString, Account fromAccount, Account toAccount, MoneyManagementService accountManager)
        {
            if (decimal.TryParse(amountString, out decimal amount))
            {
                var validAmount = ValidateAmount(amount, fromAccount);

                if (validAmount)
                {
                    var transactionFrom =
                    new Transaction
                    (
                        -amount,
                        "Transfer",
                        fromAccount.Id,
                        toAccount.Id,
                        fromAccount.Id
                    );

                    var transactionTo =
                        new Transaction
                        (
                            amount,
                            "Transfer",
                            fromAccount.Id,
                            toAccount.Id,
                            toAccount.Id
                        );

                    var saveResult = await accountManager.SaveTransactions([transactionFrom, transactionTo]);
                    
                    if (saveResult.IsFailure)
                        Console.WriteLine($"{saveResult}");

                    fromAccount.ChangeAmount(-amount);
                    toAccount.ChangeAmount(amount);

                    Console.WriteLine($"New balance account {fromAccount.Name} = {fromAccount.Balance} {fromAccount.Currency}.");
                    Console.WriteLine($"New balance account {toAccount.Name} = {toAccount.Balance} {toAccount.Currency}.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not enough money.");
                }
            }

            else
            {
                Utilities.YouReAFailureMessage();
            }
        }


        public async static Task SaveAccount(string accountTypeString, string name, decimal balance, string currency, MoneyManagementService accountManager)
        {
            try
            {
                switch (accountTypeString)
                {
                    case "1":
                        var bAccount = new Bargeldkonto(name, balance, currency, Guid.Empty);
                        var btransaction = new Transaction(balance, "Initial", bAccount.Id);
                        await accountManager.SaveAccount(bAccount);
                        var saveResult = await accountManager.SaveTransactions([btransaction]);

                        if (saveResult.IsFailure)
                            Console.WriteLine($"{saveResult}");
                        break;
                    case "2":
                        string overDraftLimit = GetOverDraftLimit();
                        if (Int32.TryParse(overDraftLimit, out int validLimit))
                        {
                            var gAccount = new Girokonto(name, balance, currency, Guid.Empty, validLimit);
                            var gtransaction = new Transaction(balance, "Initial", gAccount.Id);
                            await accountManager.SaveAccount(gAccount);
                            saveResult = await accountManager.SaveTransactions([gtransaction]);

                            if (saveResult.IsFailure)
                                Console.WriteLine($"{saveResult}");
                            break;
                        }
                        else
                        {
                            Utilities.YouReAFailureMessage();
                            break;
                        }
                    default:
                        Utilities.YouReAFailureMessage();
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Save Transaction failed");
            }
        }

        public static void YouReAFailureMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You failed a that simple task");
        }

        public static void SubstractionMessage(decimal amount, Account account, string transactionCategory)
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{amount} {account.Currency} for {transactionCategory} substracted");
        }
    }
}
