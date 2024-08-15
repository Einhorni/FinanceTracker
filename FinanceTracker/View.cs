using MoneyManagement;
using MoneyManagement.Models;
using String = System.String;
using FinanceTrackerConsole.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Security.Cryptography;

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

                if (Int32.TryParse(entry, out var entryAsInt))
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
                        Console.WriteLine();
                        Utilities.YouReAFailureMessage();
                        Console.WriteLine("Try again");
                        Console.WriteLine();
                    }

                }

                else
                {
                    Console.WriteLine();
                    Utilities.YouReAFailureMessage(); 
                    Console.WriteLine("Try again");
                    Console.WriteLine();
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
                Console.WriteLine();
                Console.WriteLine("------------------------------");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine();
                for (int i = 0; i <= accounts.Count - 1; i++)
                {
                    Console.WriteLine($"{i + 1} - Show account: {accounts[i].Name}");
                }
            }

            Console.WriteLine();

            if (accounts.Count > 1)
            {
                Console.WriteLine($"{accounts.Count + 1} - Make a transfer between accounts");
            }

            if (accounts.Count <= 6)
            {
                int newAccountEntry = accounts.Count switch
                {
                    0 => 1,
                    1 => 2,
                    _ => accounts.Count + 2
                };

                Console.WriteLine($"{newAccountEntry} - Create a new account");
            }
            
            Console.WriteLine($"9 - Exit ");
            Console.WriteLine();
            return Console.ReadLine() ?? String.Empty;
        }


        public async static Task AccountMenu(Account account, List<Account> accounts, MoneyManagementService accountManager)
        {
            var transactions = await accountManager.LoadTransactions(account.Id);

            await Utilities.ShowTransactions(account, transactions, accountManager);

            Console.WriteLine();
            Console.WriteLine("1 - Enter an expense");
            Console.WriteLine("2 - Enter an income");
            Console.WriteLine("9 - Main Menu");
            string entry = Console.ReadLine();

            const string expense = "expense";
            const string income = "income";

            switch (entry)
            {
                case "1":
                    await TransactionMenu(account, accountManager);
                    await AfterTransactionMenuLoop(account, accountManager, expense, income);
                    break;
                case "2":
                    await IncomeMenu(account, accountManager);
                    await AfterTransactionMenuLoop(account, accountManager, income, expense);
                    break;
                case "9":
                    break;
                default:
                    Utilities.YouReAFailureMessage();
                    break;
            }
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
                        Utilities.YouReAFailureMessage();
                        
                    }
                    else
                    {
                        var currency = Utilities.GetAccoutCurreny() ?? String.Empty;

                        if (!currencies.Exists(c => c == currency))
                        {
                            Utilities.YouReAFailureMessage();
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

                    Console.WriteLine();
                    Console.WriteLine("------------------------------");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{incomeAmount} {account.Currency} {transactionCategory} added");
                }
                else
                {
                    Utilities.YouReAFailureMessage();
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Current balance = {account.Balance}");
                Console.WriteLine();
            }
            else
            {
                Utilities.YouReAFailureMessage();
            }
        }



        public static string AfterTransactionMenu(string enterOption1, string enterOption2)
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.WriteLine($"1 - To enter another {enterOption1}");
            Console.WriteLine($"2 - To enter an {enterOption2}");
            Console.WriteLine("9 - To return to main menu");
            Console.WriteLine();
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
        }


        public async static Task AfterTransactionMenuLoop(Account account, MoneyManagementService accountManager, string firstMenuOption, string secondMenuOption)
        {
            bool showMainMenu = false;
            string caseTransactionMenu = firstMenuOption == "expense" ? "1" : "2";
            string caseIncomeMenu = secondMenuOption == "income" ? "2" : "1";

            do
            {
                var transactions = await accountManager.LoadTransactions(account.Id);
                await Utilities.ShowTransactions(account, transactions, accountManager);
                var entry = AfterTransactionMenu(firstMenuOption, secondMenuOption);

                if (entry == caseTransactionMenu)
                {
                    await TransactionMenu(account, accountManager);
                }
                else if (entry == caseIncomeMenu)
                {
                    await IncomeMenu(account, accountManager);
                }
                else if (entry == "9")
                {
                    showMainMenu = true;
                }
                else Utilities.YouReAFailureMessage();
            } while (!showMainMenu);
        }

        public async static Task TransactionMenu(Account account, MoneyManagementService accountManager)
        {
            var categories = await accountManager.GetCategories();
            var (categoryNumberString, listedCategories) = Utilities.GetChosenCategoryNumberStringAndAmountOfCategories(categories);

            if (!Int32.TryParse(categoryNumberString, out int categoryNumber))
            {
                Utilities.YouReAFailureMessage();
                return;
            }
            
            if (!(categoryNumber >= 1) || !(categoryNumber <= (listedCategories.Count)))
            {
                Utilities.YouReAFailureMessage();
                return;
            }
            
            string amountString = Utilities.GetTransactionAmount("expense. Without \"-\".");

            if (!decimal.TryParse(amountString, out decimal amount))
            {
                Utilities.YouReAFailureMessage();
                return;
            }

            var transactionCategory = listedCategories[categoryNumber - 1];

            var amountIsValid = Utilities.ValidateAmount(amount, account);

            if (amountIsValid)
            {
                await Utilities.SaveTransactions(-amount, transactionCategory, account, accountManager);
                Utilities.SubstractionMessage(amount, account, transactionCategory);
            }
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Current balance = {account.Balance}");
            Console.WriteLine();
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
                Utilities.YouReAFailureMessage();
            }
        }
    }
}
