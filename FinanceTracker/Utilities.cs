using MoneyManagement;
using MoneyManagement.DataAccess;
using MoneyManagement.DataAccess.FileAccess;
using MoneyManagement.Models;
using FinanceTracker.UIMappings;
using MoneyManagement.DbContexts;
using MoneyManagement.Entities;
using Microsoft.Identity.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;
using String = System.String;
using System.Collections.Generic;

namespace FinanceTracker.Utilities
{
    internal static class View
    {
        public static string MainMenu(List<Account> accounts)
        {
            if (accounts.Count > 0)
            {
                Console.WriteLine("------------------------------");
                Console.WriteLine("Available Accounts:");
                View.ShowAccounts(accounts);
                Console.WriteLine("");
                for (int i = 0; i <= accounts.Count - 1; i++)
                {
                    Console.WriteLine($"{i + 1} - Show account: {accounts[i].Name}");
                }
            }

            Console.WriteLine("");

            if (accounts.Count > 1)
            {
                Console.WriteLine($"{accounts.Count + 1} - Make a transfer between accounts");
            }

            if (accounts.Count <= 6)
            {
                int choosableNumber;
                switch (accounts.Count)
                { 
                    case 0: choosableNumber = 1; break;
                    case 1: choosableNumber = 2; break;
                    default: choosableNumber = accounts.Count + 2; break;
                }

                //if (accounts.Count == 0)
                //    choosableNumber = 1;
                //else choosableNumber = accounts.Count + 2;


                Console.WriteLine($"{choosableNumber} - Create a new account");
            }
                
            
            Console.WriteLine($"9 - Exit ");
            Console.WriteLine("");
            return Console.ReadLine() ?? String.Empty;
        }

        public static void ShowTransactions (Account account, List<Transaction> transactions)
        {
            

            List<Transaction> accountTransactions =
                    transactions
                        .Where(t => t.FromAccountId == account.Id || t.ToAccountId == account.Id || t.AccountId == account.Id)
                        .OrderByDescending(t => t.Date)
                        .Take(10)
                        .ToList();

            Console.WriteLine("------------------------------");
            Console.WriteLine($"This is Account {account.Name}. It's Balance is {account.Balance} {account.Currency}.");
            Console.WriteLine($"Last transactions:");
            foreach (Transaction transaction in accountTransactions)
            {
                //nur mit FileDataAccess
                //string stringCategories = UIMappings.MapCategoryToString(transaction.Category);
                string date = transaction.Date.ToString("dddd, dd.MMMM.yyyy HH:mm:ss");

                //TODO: von AccountName - account müsste geladen werden - Account laden im Repository
                //if (transaction.Category == "Transfer" && transaction.FromAccountId == account.Id)
                //if (transaction.Category == "Transfer" && transaction.ToAccountId == account.Id)
                Console.WriteLine($"{date}: {transaction.Amount}, {transaction.Category}");
            }
        }

        public static void AccountMenu(Account account, List<Account> accounts, MoneyManagementService accountManager)
        {
            //MoneyManagementFileService moneyManager = new MoneyManagementFileService(new FileTransactionRepository());
            //var transactions = moneyManager.LoadTransactions();

            var transactions = accountManager.LoadTransactions(account.Id).Result;

            ShowTransactions(account, transactions);

            Console.WriteLine("");
            Console.WriteLine("1 - Enter an expense");
            Console.WriteLine("2 - Enter an income");
            Console.WriteLine("9 - Main Menu");
            string entry = Console.ReadLine();

            if (entry == "1")
            {
                View.TransactionMenu(account, accountManager);
                View.AfterTransactionMenuLoop(account, accountManager);
            }

            if (entry == "2")
            {
                View.IncomeMenu(account, accountManager);
                View.AfterIncomeMenuLoop(account, accountManager);
            }

            else if (entry == "9") { }
            else
            { Console.WriteLine("You failed horribly at this simple task!"); }



        }

        public static void ShowAccounts(List<Account> accounts)
        {
            Console.WriteLine("");

            for (int i = 0; i < accounts.Count; i++ )
            {
                string accountType;
                if (accounts[i] as Bargeldkonto != null)
                    accountType = "Bargeldkonto";
                else if (accounts[i] as Girokonto != null)
                    accountType = "Girokonto";
                else
                    accountType = "";
                Console.WriteLine($"{i+1}. {accountType}: {accounts[i].Name}, Balance {accounts[i].Balance} {accounts[i].Currency.ToString()}");
            }
        }

        public static void CreateAccountMenu(List<string> currencies, MoneyManagementService accountManager)//(List<AccountDTO> accounts)
        {

            string name = View.GetAccoutName();
            
            if (name != "9" && name != "")
            {
                string balanceString = View.GetAccountBalance();

                if (decimal.TryParse(balanceString, out decimal balance))
                {
                    var currency = View.GetAccoutCurreny() ?? String.Empty;

                    if (!currencies.Exists(c => c == currency))
                    {
                        Console.WriteLine("You failed horribly at this simple task!");
                    }
                    else
                    { 
                        string accountTypeString = View.GetNewAccountType();
                        SaveAccount(accountTypeString, name, balance, currency, accountManager);                        
                    }
                }

                else
                {
                    Console.WriteLine("You failed horribly at this simple task!");
                }
            }
            else if (name == "9")
                { }
            else
                Console.WriteLine("You failed horribly at this simple task!");
        }

        public static void SaveAccount(string accountTypeString, string name, decimal balance, string currency, MoneyManagementService accountManager)
        {
            //MoneyManagementFileService accountManager = new MoneyManagementFileService(new FileAccountRepository());

            switch (accountTypeString)
            {
                case "1":
                    //accounts.Add(new Bargeldkonto(name, balance, currency, Guid.Empty));
                    var bAccount = new Bargeldkonto(name, balance, currency, Guid.Empty);
                    var btransaction = new IrregularTransaction(balance, "Initial", bAccount.Id);
                    //Wait() da Save Account ein Task ist. Hätte ich die Funktionen im Repo async gemacht, müsste ich return schreiben
                    //da aber nichts zurückgegeben wird: Wait
                    accountManager.SaveAccount(bAccount).Wait();
                    accountManager.SaveTransactions([btransaction]).Wait();
                    break;
                case "2":
                    string overDraftLimit = View.GetOverDraftLimit();
                    if (Int32.TryParse(overDraftLimit, out int validLimit))
                    {
                        //accounts.Add(new Girokonto(name, balance, currency, Guid.Empty, DateTime.Now, 0.0m));
                        var gAccount = new Girokonto(name, balance, currency, Guid.Empty, DateTime.Now, validLimit);
                        var gtransaction = new IrregularTransaction(balance, "Initial", gAccount.Id);
                        accountManager.SaveAccount(gAccount).Wait();
                        accountManager.SaveTransactions([gtransaction]).Wait();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("You failed horribly at this simple task!");
                        break;
                    }
                //true konto
                //else back

                default:
                    Console.WriteLine("You failed horribly at this simple task!");
                    break;

            }
        }

        public static string GetOverDraftLimit()
        {
            Console.WriteLine("");
            Console.WriteLine("Enter the overdraft limit (xx.xx) and hit enter.");
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
            Console.WriteLine("To enter a new Account, type in the name and hit enter.");
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
            Console.WriteLine("Enter current balance (xx.xx) and hit enter.");
            Console.WriteLine("");
            string balanceString = Console.ReadLine() ?? String.Empty;
            return balanceString;
        }
        public static string GetAccoutCurreny()
        {
            Console.WriteLine("");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Enter a currency:");
            Console.WriteLine("Euro = e");
            Console.WriteLine("US Dollar = d");
            //Console.WriteLine("Bitcoin = b");
            //Console.WriteLine("ETF = f");
            Console.WriteLine("");
            string currencyString = Console.ReadLine() ?? String.Empty;

            //MockCurrency currency;

            var returnCurrency = String.Empty;
            
            if (currencyString == "e" || currencyString == "d") // || currencyString == "e" || currencyString == "f")
            {
                var currency = UIMappings.UIMappings.MapToCurrencyString(currencyString);

                returnCurrency = currency;
                return returnCurrency;
            }

            return returnCurrency;
        }

        
        public static (string, int, List<string>) GetCategoryNumberStringAndAmountOfCategories (List<Category> categories)
        {
            var listedCategories = categories
                .Where(c => c.Expense && c.Name != "Transfer")
                .Select(c => c.Name).ToList();

            var amountOfCategories = listedCategories.Count;

            Console.WriteLine("------------------------------");
            Console.WriteLine("Please choose a category:");
            Console.WriteLine("");
            for (int i = 0; i < amountOfCategories; i++)
            {
                Console.WriteLine($"{i+1} for {listedCategories[i]}");
            }

            Console.WriteLine("");
            string categoryNumberString = Console.ReadLine() ?? String.Empty;

            return (categoryNumberString, amountOfCategories, listedCategories);
        }


        //TODO fast wie expense menu - kann ich das zusammenlegen
        public static void IncomeMenu(Account account, MoneyManagementService accountManager)
        {
            //TODO: warum wird nur Cashback aufgelistet???
            var categories= accountManager.GetCategories().Result;
            var incomeCategories = categories
                .Where(c => !c.Expense)
                .Select(c => c.Name)
                .ToList();


            Console.WriteLine("");
            Console.WriteLine("------------------------------");
            Console.WriteLine("What kind of income:");
            for(int i = 0; i <= incomeCategories.Count-1 ;i++)
            {
                Console.WriteLine($"{i+1} - {incomeCategories[i]}");
            }
            Console.WriteLine("");

            var categoryString = Console.ReadLine() ?? String.Empty;

            if (Int32.TryParse(categoryString, out var categoryNumber))
            {
                Console.WriteLine("");
                Console.WriteLine("------------------------------");
                Console.WriteLine("Enter income.");
                Console.WriteLine("");

                var icomeString = Console.ReadLine() ?? String.Empty;

                if (Int32.TryParse(icomeString, out var incomeAmount))
                {
                    var transactionCategory = incomeCategories[categoryNumber - 1];

                    IrregularTransaction newTransaction = new(incomeAmount, transactionCategory, account.Id);
                    account.Balance = account.SubstractAmount(incomeAmount);
                    accountManager.SaveTransactions([newTransaction]).Wait();

                    Console.WriteLine("");
                    Console.WriteLine("------------------------------");
                    Console.WriteLine($"{incomeAmount} {account.Currency} {transactionCategory} added");
                }
                else
                    Console.WriteLine("You failed horribly at this simple task!");

                Console.WriteLine($"Current balance = {account.Balance}");
                Console.WriteLine("");
            }
            else
                Console.WriteLine("You failed horribly at this simple task!");
        }


        //TODO fast wie afterexpensemenu - kann ich das zusammenlegen
        public static string AfterIncomeMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1 - To enter another income");
            Console.WriteLine("2 - To enter an expense");
            Console.WriteLine("9 - To return to main menu");
            Console.WriteLine("");
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
        }


        //TODO fast wie afterexpensemenuloop - kann ich das zusammenlegen
        public static void AfterIncomeMenuLoop(Account account, MoneyManagementService accountManager)
        {
            //MoneyManagementFileService transactionManager = new (new FileTransactionRepository());
            bool showMainMenu = false;

            do
            {
                var transactions = accountManager.LoadTransactions(account.Id).Result;
                ShowTransactions(account, transactions);
                var entry = View.AfterIncomeMenu();

                switch (entry)
                {
                    case "1":
                        View.IncomeMenu(account, accountManager);
                        break;
                    case "2":
                        View.TransactionMenu(account, accountManager);
                        break;
                    case "9":
                        showMainMenu = true;
                        break;
                    default:
                        Console.WriteLine("You failed horribly at this simple task!");
                        break;
                }
            } while (!showMainMenu);
        }



        public static void TransactionMenu(Account account, MoneyManagementService accountManager)
        {
            //TODO: drehen: erst category, danach betrag
            Console.WriteLine("");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Enter expense amount. DON'T put a \"-\" in front.");
            Console.WriteLine("");
            string amountString = Console.ReadLine() ?? String.Empty;

            var categories = accountManager.GetCategories().Result;
            

            if (decimal.TryParse(amountString, out decimal amount)) 
            {
                var (categoryNumberString, amoutOfCategories, listedCategories) = GetCategoryNumberStringAndAmountOfCategories(categories);
                Int32.TryParse(categoryNumberString, out int categoryNumber);

                if (categoryNumber >=1 && categoryNumber <= amoutOfCategories)
                {
                    //MoneyManagementFileService transactionManager = new MoneyManagementFileService(new FileTransactionRepository());

                    //TODO: Überarbeiten: Income woanders machen!

                    var transactionCategory = listedCategories[categoryNumber-1];

                    IrregularTransaction newTransaction = new(-amount, transactionCategory, account.Id);
                    account.Balance = account.SubstractAmount(amount);
                    accountManager.SaveTransactions([newTransaction]).Wait();

                    //MoneyManagementFileService accountManager = new (new FileAccountRepository());
                    //accountManager.SaveAccounts(accounts);
                    Console.WriteLine("");
                    Console.WriteLine("------------------------------");
                    Console.WriteLine($"{amount} {account.Currency} for {transactionCategory} substracted");
                }
                else
                    Console.WriteLine("You failed horribly at this simple task!");
            }
                    
            else
                Console.WriteLine("You failed horribly at this simple task!");

            Console.WriteLine($"Current balance = {account.Balance}");
            Console.WriteLine("");
        }

        

        public static string AfterTransactionMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1 - To enter another expense");
            Console.WriteLine("2 - To enter an income");
            Console.WriteLine("9 - To return to main menu");
            Console.WriteLine("");
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
        }

        public static void AfterTransactionMenuLoop(Account account, MoneyManagementService accountManager)
        {
            //MoneyManagementFileService transactionManager = new (new FileTransactionRepository());
            bool showMainMenu = false;

            do
            {
                var transactions = accountManager.LoadTransactions(account.Id).Result;
                ShowTransactions(account, transactions);
                var entry = View.AfterTransactionMenu();

                switch (entry)
                {
                    case "1":
                        View.TransactionMenu(account, accountManager);
                        break;
                    case "2":
                        View.IncomeMenu(account, accountManager);
                        break;
                    case "9":
                        showMainMenu = true;
                        break;
                    default:
                        Console.WriteLine("You failed horribly at this simple task!");
                        break;
                }
            } while (!showMainMenu);
        }

        public static List<Account> SaveAccounts(string amountString, List<Account> accounts, Account fromAccount, Account toAccount, MoneyManagementService accountManager)
        {
            if (decimal.TryParse(amountString, out decimal amount))
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


                //MoneyManagementFileService transactionManager = new MoneyManagementFileService(new FileTransactionRepository());
                accountManager.SaveTransactions([transactionFrom, transactionTo]).Wait();

                //MoneyManagementFileService accountManager = new(new FileAccountRepository());
                //accountManager.SaveAccounts([fromAccount, toAccount]);

                fromAccount.Balance = fromAccount.SubstractAmount(amount);
                toAccount.Balance = toAccount.AddAmount(amount);

                accounts.Remove(fromAccount);
                accounts.Remove(toAccount);
                accounts.Add(fromAccount);
                accounts.Add (toAccount);

                return accounts;
            }

            else
            {
                Console.WriteLine("You failed horribly at this simple task.");
                return accounts;
            }
        }

        public static List<Account> TransferMenu(List<Account>accounts, MoneyManagementService accountManager)
        {
            Console.WriteLine("------------------------------");
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1} - Transfer FROM {accounts[i].Name}, {accounts[i].Balance}{accounts[i].Currency}");
            }
            Console.WriteLine("");

            string fromAccountString = Console.ReadLine();

            if (Int32.TryParse(fromAccountString, out int fromAccRes))
            {
                if (fromAccRes <= accounts.Count)
                {
                    //remove fromAccount from List of Account
                    Account fromAccount = accounts[fromAccRes - 1];
                    accounts.Remove(fromAccount);

                    Console.WriteLine("------------------------------");
                    for (int i = 0; i < accounts.Count; i++)
                    {
                        Console.WriteLine($"{i + 1} - Transfer TO {accounts[i].Name}, {accounts[i].Balance}{accounts[i].Currency}");
                    }
                    Console.WriteLine("");

                    string toAccountString = Console.ReadLine();

                    if (Int32.TryParse(toAccountString, out int toAccRes))
                    {
                        if (toAccRes <= accounts.Count)
                        {
                            //remove toAccount from List of Account
                            Account toAccount = accounts[toAccRes - 1];
                            accounts.Remove(toAccount);

                            Console.WriteLine("------------------------------");
                            Console.WriteLine($"Transfer between Account {fromAccount.Name} to {toAccount.Name}");
                            Console.WriteLine($"Enter the amount (xx.xx) (max. {fromAccount.Balance} possible)");
                            Console.WriteLine("");
                            string amountString = Console.ReadLine() ?? String.Empty;

                            var newAccounts = SaveAccounts(amountString, accounts, fromAccount, toAccount, accountManager);
                            return newAccounts;
                        } 
                        else return accounts;
                    }
                    else return accounts;
                }
                else return accounts;
            }

            else
            {
                Console.WriteLine("You failed horribly at this simple task.");
                return accounts;
            }
        }
    }
}
