using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FinanceTracker.Classes;
using FinanceTracker.AccountRepository;

namespace FinanceTracker.Utilities
{

    //alles was die anzeige betrifft, Properties, Aufbereitung der Daten zur Anzeige, 

    internal class Helper
    {
        public decimal Balance { get; set; }
        public override string ToString()
        {
            return ($"Current balance = {Balance}");
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
            Console.WriteLine($"{accounts.Count +1} - Create a new account");
            Console.WriteLine($"{accounts.Count +2} - Exit ");
            string entry = Console.ReadLine() ?? String.Empty;
            return entry;
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
        public static void GetAccountCurrencyLoop(string name, decimal balance, bool showMainMenu, bool mainExit, List<Account> accounts)
        {

            for (int i = 0; i<3; i++)
            {
                
                
                string currencyString = View.GetAccoutCurreny();

                Currency currency;

                if (currencyString == "b" || currencyString == "d" || currencyString == "e" || currencyString == "f")
                {
                    currency = Account.CreateCurrency(currencyString);
                    Girokonto account = new(name, balance, currency, Guid.Empty);

                    accounts.Add(account);
                    AccountRepository.AccountRepository.SaveAccounts(accounts);

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

        public static string CreatedAccountMenu(Account account)
        {
            Console.WriteLine("");
            Console.WriteLine($"Current balance in Account {account.Name} = {account.Balance}");
            Console.WriteLine("");
            Console.WriteLine("1 - To add a transaction");
            Console.WriteLine("9 - To main menu");
            string entry = Console.ReadLine() ?? String.Empty; //?? String.Empty --> wenn das vor den Fragezeichen null ist, dass verwende diesen Wert = Alternative zum null, muss den gleichen Rückgabewert haben
            return entry;
        }

        public static void AccountMenuLoop(Account account, bool showMainMenu, bool mainExit)
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
                    //case "2":
                    //    //hier muss der Code rein, um ein neues Konto einzugeben
                    //    break;
                    case "9":
                        showMainMenu = true;
                        break;
                    default: break;
                }

                

            } while (!showMainMenu);
        }
        public static void TransactionMenu(Account account)
        {
            Console.WriteLine($"This is Account {account.Name}. It's Balance is {account.Balance}.");
            Console.WriteLine("Enter transaction amount");
            string amountString = Console.ReadLine() ?? String.Empty;


            if (decimal.TryParse(amountString, out decimal result)) //!!!!der name hinter dem out wird nach dem tryparse verwendet und muss vor nicht initialisiert werden
            {
                Regular newTransaction = new(result, "new", "ny");
                account.Balance = account.AddIncome(result);
            }
                    
            else
                Console.WriteLine("Invalid entry");

            Console.WriteLine($"Current balance = {account.Balance}");

        }

        public static string AfterTransactionMenu()
        {
            Console.WriteLine("1 - To enter another amount");
            Console.WriteLine("3 - To return to main menu");
            Console.WriteLine("9 - To exit");
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
                    case "3":
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
