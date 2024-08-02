using MoneyManagement;
using MoneyManagement.DataAccess;
using MoneyManagement.DataAccess.FileAccess;
using MoneyManagement.Models;
using FinanceTracker.UIMappings;
using MoneyManagement.DbContexts;
using MoneyManagement.Entities;

namespace FinanceTracker.Utilities
{
    internal static class View
    {
        public static string MainMenu(List<AccountDTO> accounts)
        {
            if (accounts.Count > 0)
                View.ShowAccounts(accounts);

            Console.WriteLine("");
            for (int i = 0; i <= accounts.Count-1; i++)
            {
                Console.WriteLine($"{i + 1} - Show account: {accounts[i].Name}");
            }
            Console.WriteLine($"{accounts.Count +1} - Make a transfer between accounts");
            if (accounts.Count <= 6)
                Console.WriteLine($"{accounts.Count +2} - Create a new account");
            Console.WriteLine($"9 - Exit ");
            Console.WriteLine("");
            return Console.ReadLine() ?? String.Empty;
        }

        public static void ShowTransactions (AccountDTO account, List<TransactionDTO> transactions)
        {
            

            List<TransactionDTO> accountTransactions =
                    transactions
                        .Where(t => t.FromAccountId == account.Id || t.ToAccountId == account.Id || t.AccountId == account.Id)
                        .OrderByDescending(t => t.Date)
                        .Take(10)
                        .ToList();

            Console.WriteLine($"");
            Console.WriteLine($"Last transactions:");
            foreach (TransactionDTO transaction in accountTransactions)
            {
                //nur mit FileDataAccess
                //string stringCategories = UIMappings.MapCategoryToString(transaction.Category);
                string date = transaction.Date.ToString("dddd, dd.MMMM.yyyy HH:mm:ss");
                
                //display transfers
                //TODO: von AccountName
                if(transaction.FromAccountId == account.Id)
                    Console.WriteLine($"{date}: -{transaction.Amount}, {transaction.Category}");
                ////display positive amounts
                //else if (transaction.Amount > 0.0m)
                //    Console.WriteLine($"{date}: +{transaction.Amount}, {transaction.Category}");
                //display negative amounts
                else
                    Console.WriteLine($"{date}: {transaction.Amount}, {transaction.Category}");
            }
        }

        public static void AccountMenu(AccountDTO account, List<AccountDTO> accounts)
        {
            //MoneyManagementFileService moneyManager = new MoneyManagementFileService(new FileTransactionRepository());
            //var transactions = moneyManager.LoadTransactions();

            //MoneyManagementService transactionManager = new MoneyManagementService(new TransactionRepository(new MoneyManagement.DbContexts.FinanceContext()), account.Id);
            MoneyManagementService transactionManager = new (new TransactionRepository(new MoneyManagement.DbContexts.FinanceContext()));
            var transactions = transactionManager.LoadTransactions(account.Id).Result;

            ShowTransactions(account, transactions);

            Console.WriteLine("");
            Console.WriteLine("1 - Make a transaction");
            Console.WriteLine("9 - Main Menu");
            string entry = Console.ReadLine();

            if (entry == "1")
            {
                View.TransactionMenu(account, accounts);
                View.AfterTransactionMenuLoop(entry, account, accounts);
            }
            else if (entry == "9") { }
            else
            { Console.WriteLine("You failed horribly at this simple task!"); }



        }

        public static void ShowAccounts(List<AccountDTO> accounts)
        {
            Console.WriteLine("");

            for (int i = 0; i < accounts.Count; i++ )
            {
                //TODO: einfach KindOfAccount mit ins AccountDTO?
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

        public static void CreateAccountMenu(List<string> currencies)//(List<AccountDTO> accounts)
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
                        SaveAccount(accountTypeString, name, balance, currency);                        
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

        public static void SaveAccount(string accountTypeString, string name, decimal balance, string currency)
        {
            //MoneyManagementFileService accountManager = new MoneyManagementFileService(new FileAccountRepository());

            MoneyManagementService accountManager = new(new AccountRepository(new MoneyManagement.DbContexts.FinanceContext()));

            switch (accountTypeString)
            {
                case "1":
                    //accounts.Add(new Bargeldkonto(name, balance, currency, Guid.Empty));
                    var bAccount = new Bargeldkonto(name, balance, currency, Guid.Empty);
                    accountManager.SaveAccount(bAccount);
                    break;
                case "2":
                    string overDraftLimit = View.GetOverDraftLimit();
                    if (Int32.TryParse(overDraftLimit, out int validLimit))
                    {
                        //accounts.Add(new Girokonto(name, balance, currency, Guid.Empty, DateTime.Now, 0.0m));
                        var GAccount = new Girokonto(name, balance, currency, Guid.Empty, DateTime.Now, 0.0m);
                        accountManager.SaveAccount(GAccount);
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
            Console.WriteLine("Enter current balance (xx.xx) and hit enter.");
            Console.WriteLine("");
            string balanceString = Console.ReadLine() ?? String.Empty;
            return balanceString;
        }
        public static string GetAccoutCurreny()
        {
            Console.WriteLine("");
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

        
        public static string GetCategoryString(List<string> listedCategories)
        {
            var amountOfCategories = listedCategories.Count();
            
            Console.WriteLine("Please choose a category:");
            Console.WriteLine("");
            for (int i = 0; i < amountOfCategories; i++)
            {
                Console.WriteLine($"{listedCategories[i]} for {listedCategories[i]}");
            }
            //Console.WriteLine("+ for an Income");
            //Console.WriteLine("A for Art Supplies");
            //Console.WriteLine("B for Books");
            //Console.WriteLine("C for Clothes");
            //Console.WriteLine("F for Fees");
            //Console.WriteLine("G for Groceries");
            //Console.WriteLine("H for Household");
            //Console.WriteLine("I for Insurace");
            //Console.WriteLine("O for Other Hobbies");
            //Console.WriteLine("P for Personal Health");
            //Console.WriteLine("S for Stram, Tv and Phone");
            //Console.WriteLine("T for Taxes");
            //Console.WriteLine("V for Vehicle and Fuel");
            Console.WriteLine("");
            string categoryNumberString = Console.ReadLine() ?? String.Empty;
            return categoryNumberString;
        }

        public static async void TransactionMenu(AccountDTO account, List<AccountDTO> accounts)
        {
            Console.WriteLine("");
            Console.WriteLine($"This is Account {account.Name}. It's Balance is {account.Balance} {account.Currency}.");
            Console.WriteLine("Enter transaction amount. DON'T put a \"-\" in front.");
            Console.WriteLine("");
            string amountString = Console.ReadLine() ?? String.Empty;

            MoneyManagementService categoryManager = new MoneyManagementService(new CategoryRepository(new FinanceContext()));
            var categories = await categoryManager.GetCategories();



            if (decimal.TryParse(amountString, out decimal result)) 
            {
                //press a number of them
                string categoryNumberString = GetCategoryString(categories);
                Int32.TryParse(categoryNumberString, out int categoryNumber);

                if (
                        categoryNumber >=1 && categoryNumber <= categories.Count
                        //categoryNumberString.ToUpperInvariant() == "A" ||
                        //categoryNumberString.ToUpperInvariant() == "B" ||
                        //categoryNumberString.ToUpperInvariant() == "C" ||
                        //categoryNumberString.ToUpperInvariant() == "F" ||
                        //categoryNumberString.ToUpperInvariant() == "G" ||
                        //categoryNumberString.ToUpperInvariant() == "H" ||
                        //categoryNumberString.ToUpperInvariant() == "I" ||
                        //categoryNumberString.ToUpperInvariant() == "O" ||
                        //categoryNumberString.ToUpperInvariant() == "P" ||
                        //categoryNumberString.ToUpperInvariant() == "S" ||
                        //categoryNumberString.ToUpperInvariant() == "T" ||
                        //categoryNumberString.ToUpperInvariant() == "V" ||
                        //categoryNumberString.ToUpperInvariant() == "+"
                    )
                {
                    //CategoryDTO category = UIMappings.MapToCategory (categoryNumberString);

                    //MoneyManagementFileService transactionManager = new MoneyManagementFileService(new FileTransactionRepository());

                    MoneyManagementService transactionManager = new MoneyManagementService(new TransactionRepository(new FinanceContext()));

                    //TODO: Überarbeiten: Income woander machen!
                    //if (category == CategoryDTO.Income)
                    //{
                    //    //IrregularTransaction newTransaction = new(result, category, account.Id);
                    //    IrregularTransaction newTransaction = new(result, category.ToString(), account.Id);
                    //    account.Balance = account.AddAmount(result);
                    //    transactionManager.SaveTransaction(newTransaction);
                    //}

                    //else
                    //{
                    IrregularTransaction newTransaction = new(result, categories[categoryNumber-1], account.Id);
                    account.Balance = account.SubstractAmount(result);
                    transactionManager.SaveTransactions([newTransaction]);
                    //} 

                    //MoneyManagementFileService accountManager = new (new FileAccountRepository());
                    //accountManager.SaveAccounts(accounts);
                }
                else
                    Console.WriteLine("You failed horribly at this simple task!");
            }
                    
            else
                Console.WriteLine("You failed horribly at this simple task!");

            Console.WriteLine("");
            Console.WriteLine($"Current balance = {account.Balance}");
            Console.WriteLine("");
        }

        

        public static string AfterTransactionMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("1 - To enter another amount");
            Console.WriteLine("9 - To return to main menu");
            Console.WriteLine("");
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
        }

        public static void AfterTransactionMenuLoop(string entry, AccountDTO account, List<AccountDTO>accounts)
        {
            //MoneyManagementFileService transactionManager = new (new FileTransactionRepository());
            
            MoneyManagementService transactionManager = new MoneyManagementService(new TransactionRepository(new FinanceContext()));
            var transactions = transactionManager.LoadTransactions(account.Id).Result;

            bool showMainMenu = false;

            do
            {
                ShowTransactions(account, transactions);
                entry = View.AfterTransactionMenu();

                switch (entry)
                {
                    case "1":
                        View.TransactionMenu(account, accounts);
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

        public static List<AccountDTO> SaveAccounts(string amountString, List<AccountDTO> accounts, AccountDTO fromAccount, AccountDTO toAccount)
        {
            if (decimal.TryParse(amountString, out decimal amount))
            {
                var transactionFrom =
                    new IrregularTransfer
                    (
                        -amount,
                        "Transer",
                        fromAccount.Id,
                        toAccount.Id,
                        fromAccount.Id
                    );

                var transactionTo =
                    new IrregularTransfer
                    (
                        amount,
                        "Transer",
                        fromAccount.Id,
                        toAccount.Id,
                        toAccount.Id
                    );


                //MoneyManagementFileService transactionManager = new MoneyManagementFileService(new FileTransactionRepository());
                MoneyManagementService transactionManager = new(new TransactionRepository(new FinanceContext()));
                transactionManager.SaveTransactions([transactionFrom, transactionTo]);

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

        public static List<AccountDTO> TransferMenu(List<AccountDTO>accounts)
        {
            for (int i = 0; i < accounts.Count; i++) 
            {
                Console.WriteLine($"{i+1} - Transfer FROM {accounts[i].Name}, {accounts[i].Balance}{accounts[i].Currency}");
            }
            
            string fromAccountString = Console.ReadLine();

            if (Int32.TryParse(fromAccountString, out int fromAccRes))
            {
                if (fromAccRes <= accounts.Count)
                {
                    //remove fromAccount from List of Account
                    AccountDTO fromAccount = accounts[fromAccRes - 1];
                    accounts.Remove(fromAccount);

                    for (int i = 0; i < accounts.Count; i++)
                    {
                        Console.WriteLine($"{i + 1} - Transfer TO {accounts[i].Name}, {accounts[i].Balance}{accounts[i].Currency}");
                    }

                    string toAccountString = Console.ReadLine();

                    if (Int32.TryParse(toAccountString, out int toAccRes))
                    {
                        if (toAccRes <= accounts.Count)
                        {
                            //remove toAccount from List of Account
                            AccountDTO toAccount = accounts[toAccRes - 1];
                            accounts.Remove(toAccount);

                            Console.WriteLine($"Enter the amount (xx.xx) (max. {fromAccount.Balance} possible)");
                            string amountString = Console.ReadLine() ?? String.Empty;

                            var newAccounts = SaveAccounts(amountString, accounts, fromAccount, toAccount);
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
