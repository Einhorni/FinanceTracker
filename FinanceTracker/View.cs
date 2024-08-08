using MoneyManagement;
using MoneyManagement.Models;
using String = System.String;
using FinanceTrackerConsole.Utilities;

namespace FinanceTracker.View
{
    internal static class View
    {
        public static void MainLoop(MoneyManagementService accountManager)
        {
            bool mainExit = false;

            do
            {
                var accounts = accountManager.LoadAccounts().Result;
                string entry = MainMenu(accounts);
                //int entryAsInt; --> kann man sich durch out var entryAsInt sparen
                bool mainMenuEntryIsInt = Int32.TryParse(entry, out var entryAsInt);

                if (mainMenuEntryIsInt)
                {
                    if (entryAsInt <= accounts.Count)
                    {
                        AccountMenu(accounts[entryAsInt - 1], accounts, accountManager);
                    }

                    //Show Transfer between two accounts Menu only if there are at least 2 accounts
                    else if (entryAsInt == accounts.Count + 1 && accounts.Count > 1)
                    {
                        TransferMenu(accounts, accountManager);
                    }

                    //Choosable number differs depending on the amount of accounts
                    else if (entryAsInt == accounts.Count + 1 && accounts.Count == 1)
                        CreateAccountMenu(["Dollar", "Euro"], accountManager);

                    else if (entryAsInt == accounts.Count + 2 && accounts.Count > 1)
                        CreateAccountMenu(["Dollar", "Euro"], accountManager);

                    else if (entryAsInt == (9))
                        mainExit = true;

                    else
                    {
                        Console.WriteLine("");
                        Console.WriteLine("You failed horribly. Try again!");
                        Console.WriteLine("");
                    }

                }

                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("You failed horribly. Try again");
                    Console.WriteLine("");
                }

            } while (!mainExit);
        }


        public static string MainMenu(List<Account> accounts)
        {
            if (accounts.Count > 0)
            {
                Console.WriteLine("------------------------------");
                Console.WriteLine("Available Accounts:");

                Utilities.ShowAccounts(accounts);
                
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
                int newAccountEntry;
                switch (accounts.Count)
                { 
                    case 0: newAccountEntry = 1; break;
                    case 1: newAccountEntry = 2; break;
                    default: newAccountEntry = accounts.Count + 2; break;
                }
                Console.WriteLine($"{newAccountEntry} - Create a new account");
            }
            
            Console.WriteLine($"9 - Exit ");
            Console.WriteLine("");
            return Console.ReadLine() ?? String.Empty;
        }


        public static void AccountMenu(Account account, List<Account> accounts, MoneyManagementService accountManager)
        {
            var transactions = accountManager.LoadTransactions(account.Id).Result;

            Utilities.ShowTransactions(account, transactions);

            Console.WriteLine("");
            Console.WriteLine("1 - Enter an expense");
            Console.WriteLine("2 - Enter an income");
            Console.WriteLine("9 - Main Menu");
            string entry = Console.ReadLine();

            if (entry == "1")
            {
                TransactionMenu(account, accountManager);
                AfterTransactionMenuLoop(account, accountManager);
            }

            if (entry == "2")
            {
                IncomeMenu(account, accountManager);
                AfterIncomeMenuLoop(account, accountManager);
            }

            else if (entry == "9") { }
            else
            { Console.WriteLine("You failed horribly at this simple task!"); }
        }



        public static void CreateAccountMenu(List<string> currencies, MoneyManagementService accountManager)//(List<AccountDTO> accounts)
        {
            string name = Utilities.GetAccoutName();

            if (name == "9") { }
            if (name == "")
            {
                Console.WriteLine("You failed horribly at this simple task!");
            }

            else
            {
                string balanceString = Utilities.GetAccountBalance();

                if (!(decimal.TryParse(balanceString, out decimal balance)))
                {
                    Console.WriteLine("You failed horribly at this simple task!");
                }

                else
                {
                    var currency = Utilities.GetAccoutCurreny() ?? String.Empty;

                    if (!currencies.Exists(c => c == currency))
                    {
                        Console.WriteLine("You failed horribly at this simple task!");
                    }
                    else
                    { 
                        string accountTypeString = Utilities.GetNewAccountType();
                        Utilities.SaveAccount(accountTypeString, name, balance, currency, accountManager);                        
                    }
                }
            }            
        }


        public static void IncomeMenu(Account account, MoneyManagementService accountManager)
        {
            var categories= accountManager.GetCategories().Result;
            var incomeCategories = categories
                .Where(c => !c.Expense)
                .Select(c => c.Name)
                .ToList();

            Utilities.ListCatgories(incomeCategories, "income");

            var categoryString = Console.ReadLine() ?? String.Empty;

            if (Int32.TryParse(categoryString, out var categoryNumber))
            {
                var incomeString = Utilities.GetTransactionAmount("income");

                if (Int32.TryParse(incomeString, out var incomeAmount))
                {
                    var transactionCategory = incomeCategories[categoryNumber - 1];

                    Utilities.SaveTransactions(incomeAmount, transactionCategory, account, accountManager);

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



        public static string AfterTransactionMenu(string enterOption1, string enterOption2)
        {
            Console.WriteLine("");
            Console.WriteLine("------------------------------");
            Console.WriteLine($"1 - To enter another {enterOption1}");
            Console.WriteLine($"2 - To enter an {enterOption2}");
            Console.WriteLine("9 - To return to main menu");
            Console.WriteLine("");
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
        }


        public static void AfterIncomeMenuLoop(Account account, MoneyManagementService accountManager)
        {
            bool showMainMenu = false;

            do
            {
                var transactions = accountManager.LoadTransactions(account.Id).Result;
                Utilities.ShowTransactions(account, transactions);
                var entry = AfterTransactionMenu("income", "expense");

                switch (entry)
                {
                    case "1":
                        IncomeMenu(account, accountManager);
                        break;
                    case "2":
                        TransactionMenu(account, accountManager);
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
            var categories = accountManager.GetCategories().Result;
            var (categoryNumberString, listedCategories) = Utilities.GetChosenCategoryNumberStringAndAmountOfCategories(categories);
            
            Int32.TryParse(categoryNumberString, out int categoryNumber);

            if (categoryNumber >=1 && categoryNumber <= (listedCategories.Count))
            {
                string amountString = Utilities.GetTransactionAmount("expense. Without \"-\".");

                if (decimal.TryParse(amountString, out decimal amount))
                {
                    var transactionCategory = listedCategories[categoryNumber - 1];

                    var amountIsValid = Utilities.ValidateAmount(amount, account);

                    if (amountIsValid)
                    {
                        {
                            Utilities.SaveTransactions(-amount, transactionCategory, account, accountManager);

                            Console.WriteLine("");
                            Console.WriteLine("------------------------------");
                            Console.WriteLine($"{amount} {account.Currency} for {transactionCategory} substracted");
                        }
                    }
                }
                else
                    Console.WriteLine("You failed horribly at this simple task!");
            }
            else
                Console.WriteLine("You failed horribly at this simple task!");
            
            Console.WriteLine($"Current balance = {account.Balance}");
            Console.WriteLine("");
        }


        public static void AfterTransactionMenuLoop(Account account, MoneyManagementService accountManager)
        {
            bool showMainMenu = false;

            do
            {
                var transactions = accountManager.LoadTransactions(account.Id).Result;
                Utilities.ShowTransactions(account, transactions);
                var entry = AfterTransactionMenu("expense", "income");

                switch (entry)
                {
                    case "1":
                        TransactionMenu(account, accountManager);
                        break;
                    case "2":
                        IncomeMenu(account, accountManager);
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


        public static void TransferMenu(List<Account>accounts, MoneyManagementService accountManager)
        {
            var fromAccountString = Utilities.GetChosenAccountStringForTransfer(accounts, "FROM");

            if (Int32.TryParse(fromAccountString, out int fromAccRes))
            {
                if (fromAccRes <= accounts.Count)
                {
                    //remove fromAccount from List of Accounts for display
                    var fromAccount = accounts[fromAccRes - 1];
                    accounts.Remove(fromAccount);

                    string toAccountString = Utilities.GetChosenAccountStringForTransfer(accounts, "TO");

                    if (Int32.TryParse(toAccountString, out int toAccChoice))
                    {
                        if (toAccChoice <= accounts.Count)
                        {
                            var toAccount = accounts[toAccChoice - 1];

                            var amountString = Utilities.GetTransferAmount(fromAccount, toAccount);

                            Utilities.SaveAccounts(amountString, fromAccount, toAccount, accountManager);
                        } 
                    }
                }
            }
            else
            {
                Console.WriteLine("You failed horribly at this simple task.");
            }
        }
    }
}
