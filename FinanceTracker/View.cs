using MoneyManagement;
using MoneyManagement.Models;
using String = System.String;
using FinanceTrackerConsole.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;

namespace FinanceTracker.View
{
    internal static class View
    {
        public async static Task MainLoop(MoneyManagementService accountManager)
        {
            bool mainExit = false;

            do
            {
                var accounts = await accountManager.LoadAccounts();
                string entry = MainMenu(accounts);
                //int entryAsInt; --> kann man sich durch out var entryAsInt sparen
                bool mainMenuEntryIsInt = Int32.TryParse(entry, out var entryAsInt);

                if (mainMenuEntryIsInt)
                {
                    if (entryAsInt <= accounts.Count)
                    {
                        await AccountMenu(accounts[entryAsInt - 1], accounts, accountManager);
                    }

                    //Show Transfer between two accounts Menu only if there are at least 2 accounts
                    else if (entryAsInt == accounts.Count + 1 && accounts.Count > 1)
                    {
                        await TransferMenu(accounts, accountManager);
                    }

                    //Choosable number differs depending on the amount of accounts
                    else if (entryAsInt == accounts.Count + 1 && accounts.Count == 1)
                        await CreateAccountMenu(["Dollar", "Euro"], accountManager);

                    else if (entryAsInt == accounts.Count + 2 && accounts.Count > 1)
                        await CreateAccountMenu(["Dollar", "Euro"], accountManager);

                    else if (entryAsInt == (9))
                        mainExit = true;

                    else
                    {
                        Console.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You failed horribly. Try again!");
                        Console.WriteLine("");
                    }

                }

                else
                {
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
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
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Available Accounts:");

                Utilities.ShowAccounts(accounts);
                Console.WriteLine("");
                Console.WriteLine("------------------------------");
                Console.ForegroundColor = ConsoleColor.White;

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


        public async static Task AccountMenu(Account account, List<Account> accounts, MoneyManagementService accountManager)
        {
            var transactions = await accountManager.LoadTransactions(account.Id);

            await Utilities.ShowTransactions(account, transactions, accountManager);

            Console.WriteLine("");
            Console.WriteLine("1 - Enter an expense");
            Console.WriteLine("2 - Enter an income");
            Console.WriteLine("9 - Main Menu");
            string entry = Console.ReadLine();

            switch(entry)
            {
                case "1":
                    await TransactionMenu(account, accountManager);
                    await AfterTransactionMenuLoop(account, accountManager);
                    break;
                case "2":
                    await IncomeMenu(account, accountManager);
                    await AfterIncomeMenuLoop(account, accountManager);
                    break;
                case "9":
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You failed horribly at this simple task!");
                    break;
            }


            //if (entry == "1")
            //{
            //    await TransactionMenu(account, accountManager);
            //    await AfterTransactionMenuLoop(account, accountManager);
            //}

            //else if (entry == "2")
            //{
            //    await IncomeMenu(account, accountManager);
            //    await AfterIncomeMenuLoop(account, accountManager);
            //}

            //else if (entry == "9") { }
            //else
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("You failed horribly at this simple task!"); 
            //}
        }



        public async static Task CreateAccountMenu(List<string> currencies, MoneyManagementService accountManager)
        {
            string name = Utilities.GetAccoutName();

            switch (name)
            {
                case "9": break;
                case "": break;
                default:
                    string balanceString = Utilities.GetAccountBalance();

                    if (!(decimal.TryParse(balanceString, out decimal balance)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You failed horribly at this simple task!");
                    }

                    else
                    {
                        var currency = Utilities.GetAccoutCurreny() ?? String.Empty;

                        if (!currencies.Exists(c => c == currency))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You failed horribly at this simple task!");
                        }
                        else
                        {
                            string accountTypeString = Utilities.GetNewAccountType();
                            await Utilities.SaveAccount(accountTypeString, name, balance, currency, accountManager);
                        }
                    };
                    break;
            }                     
        }


        public async static Task IncomeMenu(Account account, MoneyManagementService accountManager)
        {
            var categories= await accountManager.GetCategories();
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

                    await Utilities.SaveTransactions(incomeAmount, transactionCategory, account, accountManager);

                    Console.WriteLine("");
                    Console.WriteLine("------------------------------");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{incomeAmount} {account.Currency} {transactionCategory} added");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You failed horribly at this simple task!");
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Current balance = {account.Balance}");
                Console.WriteLine("");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You failed horribly at this simple task!");
            }
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


        public async static Task AfterIncomeMenuLoop(Account account, MoneyManagementService accountManager)
        {
            bool showMainMenu = false;

            do
            {
                var transactions = await accountManager.LoadTransactions(account.Id);
                await Utilities.ShowTransactions(account, transactions, accountManager);
                var entry = AfterTransactionMenu("income", "expense");

                switch (entry)
                {
                    case "1":
                        await IncomeMenu(account, accountManager);
                        break;
                    case "2":
                        await TransactionMenu(account, accountManager);
                        break;
                    case "9":
                        showMainMenu = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You failed horribly at this simple task!");
                        break;
                }
            } while (!showMainMenu);
        }


        public async static Task TransactionMenu(Account account, MoneyManagementService accountManager)
        {
            var categories = await accountManager.GetCategories();
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
                            await Utilities.SaveTransactions(-amount, transactionCategory, account, accountManager);

                            Console.WriteLine("");
                            Console.WriteLine("------------------------------");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"{amount} {account.Currency} for {transactionCategory} substracted");
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You failed horribly at this simple task!");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You failed horribly at this simple task!");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Current balance = {account.Balance}");
            Console.WriteLine("");
        }


        public async static Task AfterTransactionMenuLoop(Account account, MoneyManagementService accountManager)
        {
            bool showMainMenu = false;

            do
            {
                var transactions = await accountManager.LoadTransactions(account.Id);
                await Utilities.ShowTransactions(account, transactions, accountManager);
                var entry = AfterTransactionMenu("expense", "income");

                switch (entry)
                {
                    case "1":
                        await TransactionMenu(account, accountManager);
                        break;
                    case "2":
                        await IncomeMenu(account, accountManager);
                        break;
                    case "9":
                        showMainMenu = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You failed horribly at this simple task!");
                        break;
                }
            } while (!showMainMenu);
        }


        public async static Task TransferMenu(List<Account>accounts, MoneyManagementService accountManager)
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

                            await Utilities.SaveAccounts(amountString, fromAccount, toAccount, accountManager);
                        } 
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You failed horribly at this simple task.");
            }
        }
    }
}
