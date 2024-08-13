using FinanceTracker.UIMappings;
using MoneyManagement.Models;
using MoneyManagement;

namespace FinanceTrackerConsole.Utilities
{
    internal class Utilities
    {
        public static string GetOverDraftLimit()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Enter the overdraft limit (xx.xx) and hit enter.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            string overdraftLimit = Console.ReadLine() ?? String.Empty;
            return overdraftLimit;
        }


        public static string GetNewAccountType()
        {
            Console.WriteLine("");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1 - Cash Account");
            Console.WriteLine("2 - Bank Account");
            Console.WriteLine("9 - Main Menu");
            Console.WriteLine("");
            string accountType = Console.ReadLine() ?? String.Empty;
            return accountType;
        }


        public static string GetAccoutName()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("To enter a new Account, type in the name and hit enter.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("9 - Main Menu");
            Console.WriteLine("");
            string name = Console.ReadLine() ?? String.Empty;
            Console.WriteLine("");
            return name;
        }


        public static string GetAccountBalance()
        {
            Console.WriteLine("");
            Console.WriteLine("------------------------------");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Enter current balance (xx.xx) and hit enter.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            string balanceString = Console.ReadLine() ?? String.Empty;
            return balanceString;
        }


        public static string GetAccoutCurreny()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine("Enter a currency:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Euro = e");
            Console.WriteLine("US Dollar = d");
            Console.WriteLine("");
            string currencyString = Console.ReadLine() ?? String.Empty;

            if (currencyString == "e" || currencyString == "d")
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
            var listedCategories = categories
                .Where(c => c.Expense && c.Name != "Transfer")
                .Select(c => c.Name).ToList();

            ListCatgories(listedCategories, "category");

            Console.WriteLine("");
            string categoryNumberString = Console.ReadLine() ?? String.Empty;

            return (categoryNumberString, listedCategories);
        }


        public static void ListCatgories(List<string> categories, string typeOfCategory)
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine("");
            Console.WriteLine($"What kind of {typeOfCategory}:");
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i <= categories.Count - 1; i++)
            {
                Console.WriteLine($"{i + 1} - {categories[i]}");
            }
            Console.WriteLine("");
        }


        public static string GetTransactionAmount(string kindOfAmount)
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Enter {kindOfAmount}.");
            Console.WriteLine("");
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
            Console.WriteLine("");

            return Console.ReadLine() ?? String.Empty;
        }


        public static string GetTransferAmount(Account fromAccount, Account toAccount)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Transfer between Account {fromAccount.Name} to {toAccount.Name}");
            Console.WriteLine($"Enter the amount (xx.xx) (max. {fromAccount.Balance} possible)");
            Console.WriteLine("");
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
                account is Girokonto && (account.Balance + ((account as Girokonto).OverdraftLimit) < amount))
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
            foreach (Transaction transaction in accountTransactions)
            {
                string otherAccountText = "";
                if (transaction.SendingAccountId == account.Id)
                {
                    var otherAccount = await accountManager.LoadAccount(transaction.SendingAccountId.Value);
                    otherAccountText = $"to {otherAccount.Name}";
                }
                else if (transaction.ReceivingAccountId == account.Id)
                {
                    var otherAccount = await accountManager.LoadAccount(transaction.ReceivingAccountId.Value);
                    otherAccountText = $"from {otherAccount.Name}";
                }

                string date = transaction.Date.ToString("dddd, dd.MMMM.yyyy HH:mm:ss");
                Console.WriteLine($"{date}: {transaction.Amount}, {transaction.Category} {otherAccountText}");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }


        public static void ShowAccounts(List<Account> accounts)
        {
            Console.WriteLine("");

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
                if (accounts[i] is Girokonto)
                {
                    Girokonto account =  accounts[i] as Girokonto;
                    overdraftLimit = $", Overdraft-Limit = {account.OverdraftLimit}";
                }
                else { overdraftLimit = ""; }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{i + 1}. {accountType}: {accounts[i].Name}, Balance {accounts[i].Balance} {accounts[i].Currency.ToString()} {overdraftLimit}");
            }
        }


        public async static Task<string> SaveTransactions(decimal amount, string transactionCategory, Account account, MoneyManagementService accountManager)
        {
            IrregularTransaction newTransaction = new(amount, transactionCategory, account.Id);
            account.ChangeAmount(amount);
            return await accountManager.SaveTransactions([newTransaction]);
        }


        public async static Task SaveAccounts(string amountString, Account fromAccount, Account toAccount, MoneyManagementService accountManager)
        {
            if (decimal.TryParse(amountString, out decimal amount))
            {
                var validAmount = ValidateAmount(amount, fromAccount);

                if (validAmount)
                {
                    var transactionFrom =
                    new IrregularTransfer
                    (
                        -amount,
                        "Transfer",
                        fromAccount.Id,
                        toAccount.Id,
                        fromAccount.Id
                    );

                    var transactionTo =
                        new IrregularTransfer
                        (
                            amount,
                            "Transfer",
                            fromAccount.Id,
                            toAccount.Id,
                            toAccount.Id
                        );

                    var messageFromRepo = await accountManager.SaveTransactions([transactionFrom, transactionTo]);
                    Console.WriteLine($"{messageFromRepo}");

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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You failed horribly at this simple task.");
            }
        }


        public async static Task SaveAccount(string accountTypeString, string name, decimal balance, string currency, MoneyManagementService accountManager)
        {
            switch (accountTypeString)
            {
                case "1":
                    var bAccount = new Bargeldkonto(name, balance, currency, Guid.Empty);
                    var btransaction = new IrregularTransaction(balance, "Initial", bAccount.Id);
                    await accountManager.SaveAccount(bAccount);
                    var messageFromRepo = await accountManager.SaveTransactions([btransaction]);
                    Console.WriteLine($"{messageFromRepo}");
                    break;
                case "2":
                    string overDraftLimit = GetOverDraftLimit();
                    if (Int32.TryParse(overDraftLimit, out int validLimit))
                    {
                        //accounts.Add(new Girokonto(name, balance, currency, Guid.Empty, DateTime.Now, 0.0m));
                        var gAccount = new Girokonto(name, balance, currency, Guid.Empty, DateTime.Now, validLimit);
                        var gtransaction = new IrregularTransaction(balance, "Initial", gAccount.Id);
                        await accountManager.SaveAccount(gAccount);
                        messageFromRepo = await accountManager.SaveTransactions([gtransaction]);
                        Console.WriteLine($"{messageFromRepo}");
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("You failed horribly at this simple task!");
                        break;
                    }
                default:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("You failed horribly at this simple task!");
                    break;
            }
        }
    }
}
