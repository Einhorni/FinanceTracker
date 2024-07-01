using FinanceTracker;
using FinanceTracker.Classes;
using FinanceTracker.Utilities;
using System.ComponentModel.Design;
using System.Security.Principal;
using FinanceTracker.DataAccess;
using System.Xml.Linq;
using FinanceTracker.MoneyManagement;
using System.Security.Cryptography.X509Certificates;



bool mainExit = false;
bool showMainMenu = false;

AccountManager accountManager = new AccountManager(new AccountRepository());
List<Account> accounts = accountManager.Accounts;

do
{
    if (accounts.Count > 0 )
    {
        Console.WriteLine("");
        foreach ( var account in accounts )
        {
            Console.WriteLine($"Account: {account.Name}, Balance {account.Balance} {account.Currency.ToString()}");
        }
        
    }
     
    string entry = View.MainMenu(accounts);
    int entryAsInt;
    bool mainMenuEntryIsInt = Int32.TryParse(entry, out entryAsInt);


    if (mainMenuEntryIsInt)
    {
        //Zeige Account Funtionen an
        if (entryAsInt <= accounts.Count)
        {
            View.TransactionMenu(accounts[entryAsInt - 1]);
            View.AfterTransactionMenuLoop(entry, accounts[entryAsInt - 1], showMainMenu, mainExit);

            continue;
        }

        else if (entryAsInt == accounts.Count + 1 && entryAsInt > accounts.Count)
            View.CreateAccount(showMainMenu, accounts);


        else if (entryAsInt == (9))
            mainExit = true;

        else
        {
            Console.WriteLine("");
            Console.WriteLine("Try again");
            Console.WriteLine("");
        }
            
    }

    
    else
    {
        Console.WriteLine("");
        Console.WriteLine("Try again");
        Console.WriteLine("");
    }
    

} while (!mainExit);







