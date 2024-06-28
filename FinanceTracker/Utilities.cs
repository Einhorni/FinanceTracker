using FinanceTracker.DataAccess;
using FinanceTracker.Classes;
using FinanceTracker.DataAccess;
using FinanceTracker.MoneyManagement;

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
        public static string MainMenu(List<Classes.Account> accounts)
        {
            Console.WriteLine("");
            for (int i = 0; i <= accounts.Count-1; i++)
            {
                Console.WriteLine($"{i+1} - Make a transaction in account: {accounts[i].Name}");
            }
            Console.WriteLine($"{accounts.Count +1} - Create a new account");
            Console.WriteLine($"9 - Exit ");
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
        }

        public static void CreateAccount(bool showMainMenu, List<Account> accounts)
        {
            string name = View.GetAccoutName();

            if (name != "9" && name != "")
            {
                string balanceString = View.GetAccountBalance();

                if (decimal.TryParse(balanceString, out decimal balance))
                {

                    View.GetAccountCurrencyLoop(name, balance, showMainMenu, showMainMenu, accounts);

                }
                else //TODO: balance else not valid ask again 2 times, else exit, wie get curreny loop
                { showMainMenu = true; }
            }
        }


        public static string GetAccoutName()
        {
            Console.WriteLine("");
            Console.WriteLine("To enter a new Account, type in the name and hit enter:");
            Console.WriteLine("9 - Main Menu");
            string name = Console.ReadLine() ?? String.Empty;
            Console.WriteLine("");
            return name;
        }

        public static string GetAccountBalance()
        {
            Console.WriteLine("");
            Console.WriteLine("Enter current balance and hit enter:");
            string balanceString = Console.ReadLine() ?? String.Empty;
            return balanceString;
        }
        public static string GetAccoutCurreny()
        {
            Console.WriteLine("");
            Console.WriteLine("Enter a currency:");
            Console.WriteLine("Euro = e");
            Console.WriteLine("US Dollar = d");
            Console.WriteLine("Bitcoin = b");
            Console.WriteLine("ETF = f");
            string currencyString = Console.ReadLine() ?? String.Empty;
            return currencyString;
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
        public static void GetAccountCurrencyLoop(string name, decimal balance, bool showMainMenu, bool mainExit, List<Classes.Account> accounts)
        {

            for (int i = 0; i<3; i++)
            {
                
                
                string currencyString = View.GetAccoutCurreny();

                Currency currency;

                if (currencyString == "b" || currencyString == "d" || currencyString == "e" || currencyString == "f")
                {
                    currency = Mappings.CreateCurrency(currencyString);
                    Girokonto account = new(name, balance, currency, Guid.Empty);

                    accounts.Add(account);
                    AccountManager accountManager = new AccountManager(new AccountRepository());
                    accountManager.SaveAccounts(accounts);
                    

                    View.AccountMenuLoop(account, showMainMenu, mainExit);
                    break;
                }

                else
                {
                    if (i ==2)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("To many false entries. You will be directed back to the main menu");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("False entry. Try again.");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        continue;
                    }
                        
                }
            }
        }

        public static string CreatedAccountMenu(Classes.Account account)
        {
            Console.WriteLine("");
            Console.WriteLine($"Current balance in Account {account.Name} = {account.Balance}");
            Console.WriteLine("");
            Console.WriteLine("1 - To add a transaction");
            Console.WriteLine("9 - To main menu");
            string entry = Console.ReadLine() ?? String.Empty; //?? String.Empty --> wenn das vor den Fragezeichen null ist, dass verwende diesen Wert = Alternative zum null, muss den gleichen Rückgabewert haben
            return entry;
        }

        public static void AccountMenuLoop(Classes.Account account, bool showMainMenu, bool mainExit)
        {
            do
            {
                string entry = View.CreatedAccountMenu(account);

                switch (entry)
                {
                    case "1":
                        View.TransactionMenu(account);
                        View.AfterTransactionMenuLoop(entry, account, showMainMenu, mainExit);
                        showMainMenu = true;
                        break;
                    case "9":
                        showMainMenu = true;
                        break;
                    default: break;
                }

                

            } while (!showMainMenu);
        }
        public static void TransactionMenu(Classes.Account account)
        {
            Console.WriteLine($"This is Account {account.Name}. It's Balance is {account.Balance}.");
            Console.WriteLine("Enter transaction amount");
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
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
        }

        public static void AfterTransactionMenuLoop(string entry, Classes.Account account, bool showMainMenu, bool mainExit)
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
    }
}
