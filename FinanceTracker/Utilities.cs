using FinanceTracker.DataAccess;
using FinanceTracker.Classes;
using FinanceTracker.DataAccess;
using FinanceTracker.MoneyManagement;
using System.ComponentModel.Design;
using System.Security.Principal;
using System.Xml.Linq;

namespace FinanceTracker.Utilities
{

    internal class Mappings
    {
        public static Currency CreateCurrency(string currencyString)
        {
            Currency currency = new();
            switch (currencyString)
            {
                case "b":
                    currency = Currency.Bitcoin;
                    break;

                case "d":
                    currency = Currency.Dollar;
                    break;

                case "e":
                    currency = Currency.EUR;
                    break;

                case "f":
                    currency = Currency.ETF;
                    break;
            }
            return currency;
        }
    }

    internal static class View
    {
        public static string MainMenu(List<Account> accounts)
        {
            Console.WriteLine("");
            for (int i = 0; i <= accounts.Count-1; i++)
            {
                Console.WriteLine($"{i+1} - Make a transaction in account: {accounts[i].Name}");
            }
            Console.WriteLine($"{accounts.Count +1} - Make a transfer between accounts");
            if (accounts.Count <= 6)
                Console.WriteLine($"{accounts.Count +2} - Create a new account");
            Console.WriteLine($"9 - Exit ");
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
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

        public static void CreateAccount(bool showMainMenu, List<Account> accounts)
        {

            string name = View.GetAccoutName();

            //TODO: get account type: giro, tages, fest, bar
            
            if (name != "9" && name != "")
            {
                string balanceString = View.GetAccountBalance();

                if (decimal.TryParse(balanceString, out decimal balance))
                {

                    Currency currency = View.GetAccoutCurreny();
                    if (currency == Currency.Error)
                    {
                        Console.WriteLine("You failed horribly at this simple task!");
                        showMainMenu = true;
                    }
                        
                    else
                    { 
                        string accountTypeString = View.GetNewAccountType();

                        SaveAccount(accountTypeString, accounts, name, balance, currency);                        
                    }
                    showMainMenu = true;                    

                }
                else
                {
                    Console.WriteLine("You failed horribly at this simple task!");
                    showMainMenu = true; 
                }
            }
            else
                Console.WriteLine("You failed horribly at this simple task!");
        }

        public static void SaveAccount(string accountTypeString, List<Account> accounts, string name, decimal balance, Currency currency)
        {
            AccountManager accountManager = new AccountManager(new AccountRepository());
            switch (accountTypeString)
            {
                case "1":
                    accounts.Add(new Bargeldkonto(name, balance, currency, Guid.Empty));
                    accountManager.SaveAccounts(accounts);
                    break;
                case "2":
                    string overDraftLimit = View.GetOverDraftLimit();
                    if (Int32.TryParse(overDraftLimit, out int validLimit))
                    {
                        accounts.Add(new Girokonto(name, balance, currency, Guid.Empty, 0.0m));
                        accountManager.SaveAccounts(accounts);
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
        public static Currency GetAccoutCurreny()
        {
            Console.WriteLine("");
            Console.WriteLine("Enter a currency:");
            Console.WriteLine("Euro = e");
            Console.WriteLine("US Dollar = d");
            Console.WriteLine("Bitcoin = b");
            Console.WriteLine("ETF = f");
            Console.WriteLine("");
            string currencyString = Console.ReadLine() ?? String.Empty;

            Currency currency;

            if (currencyString == "b" || currencyString == "d" || currencyString == "e" || currencyString == "f")
            {
                currency = Mappings.CreateCurrency(currencyString);

                return currency;
            }
            return Currency.Error;
        }

        #region recursion
        //rekursion in dem Fall okay, da sie nur 2 mal aufgerufen wird, ansonten in C# eher Loops verwenden
        //public static void GetAccountCurrencyLoopRec(string name, decimal balance, bool exiter, bool exiter2, bool firstmain, int loopCounter)
        //{
        //    string currencyString = Views.GetAccoutCurreny();

        //    Currency currency;

        //    //2 mal falsch eingeben -> Möglichkeit nochmal eingeben, danach Programmende


        //    if (currencyString == "b" || currencyString == "d" || currencyString == "e" || currencyString == "f")
        //    {
        //        currency = Account.CreateCurrency(currencyString);
        //        Girokonto account = new(name, balance, currency);

        //        Views.AccountMenuLoop(account, exiter, exiter2, firstmain);
        //    }

        //    else
        //    {
        //        loopCounter++;
        //        if (loopCounter > 2) 
        //            firstmain = true;
        //        else
        //            GetAccountCurrencyLoopRec( name, balance,  exiter,  exiter2,  firstmain, loopCounter);
        //    }    
        //}
        #endregion

        //Loop bei mehreren Durchläufen einer rekursiven Funktion vorzuziehen
        //public static void GetAccountCurrencyLoop(string name, decimal balance, bool showMainMenu, bool mainExit, List<Account> accounts)
        //{
        //    for (int i = 0; i<3; i++)
        //    {
                
        //        if ()
        //        {
        //            
        //        }

        //        else
        //        {
        //            if (i ==2)
        //            {
        //                Console.WriteLine("");
        //                Console.WriteLine("To many false entries. You will be directed back to the main menu");
        //                Console.WriteLine("");
        //                Console.WriteLine("");
        //                continue;
        //            }
        //            else
        //            {
        //                Console.WriteLine("False entry. Try again.");
        //                Console.WriteLine("");
        //                Console.WriteLine("");
        //                continue;
        //            }                   
        //        }
        //    }
        //}


        public static string CreatedAccountMenu(Account account)
        {
            Console.WriteLine("");
            Console.WriteLine($"Current balance in Account {account.Name} = {account.Balance}");
            Console.WriteLine("");
            Console.WriteLine("1 - To add a transaction");
            Console.WriteLine("9 - To main menu");
            Console.WriteLine("");
            string entry = Console.ReadLine() ?? String.Empty; //?? String.Empty --> wenn das vor den Fragezeichen null ist, dann verwende diesen Wert = Alternative zum null, muss den gleichen Rückgabewert haben
            return entry;
        }


        public static void TransactionMenu(Account account)
        {
            Console.WriteLine($"This is Account {account.Name}. It's Balance is {account.Balance} {account.Currency}.");
            Console.WriteLine("Enter transaction amount");
            Console.WriteLine("");
            string amountString = Console.ReadLine() ?? String.Empty;


            if (decimal.TryParse(amountString, out decimal result)) //!!!!der name hinter dem out wird nach dem tryparse verwendet und muss vor nicht initialisiert werden
            {
                Irregular newTransaction = new(result, "new", "ny");
                account.Balance = account.AddIncome(result);
            }
                    
            else
                Console.WriteLine("Invalid entry");

            Console.WriteLine($"Current balance = {account.Balance}");

        }

        public static string AfterTransactionMenu()
        {
            Console.WriteLine("1 - To enter another amount");
            Console.WriteLine("2 - To return to main menu");
            Console.WriteLine("9 - To exit");
            Console.WriteLine("");
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
        }

        public static void AfterTransactionMenuLoop(string entry, Account account, bool showMainMenu, bool mainExit)
        {
            do
            {
                entry = View.AfterTransactionMenu();

                switch (entry)
                {
                    case "1":
                        View.TransactionMenu(account);
                        break;
                    case "2":
                        showMainMenu = true;
                        break;
                    case "9":
                        showMainMenu = true;
                        mainExit = true;
                        break;
                    default:
                        break;
                }
            } while (!showMainMenu);
        }

        public static void TransferMenu(bool showMainMenu, List<Account>accounts)
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
                    Account fromAccount = accounts[fromAccRes - 1];
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
                            Account toAccount = accounts[toAccRes - 1];
                            accounts.Remove(toAccount);

                            //TODO: dispo berücksichtigen
                            Console.WriteLine($"Enter the amount (xx.xx) (max. {fromAccount.Balance} possible)");
                            string amountString = Console.ReadLine() ?? String.Empty;

                            if (decimal.TryParse(amountString, out decimal amount))
                            {
                                fromAccount.Balance -= amount;
                                toAccount.Balance += amount;

                                accounts.AddRange([fromAccount, toAccount]);

                                //TODO: transaktionen speichern
                                AccountManager accountManager = new AccountManager(new AccountRepository());
                                accountManager.SaveAccounts(accounts);
                            }
                            else
                                Console.WriteLine("You failed horribly at this simple task.");
                        }
                    }
                }
                
            }

            else Console.WriteLine("You failed horribly at this simple task.");
        }
    }
}
