using FinanceTracker;
using FinanceTracker.Classes;
using FinanceTracker.Utilities;
using System.ComponentModel.Design;
using System.Security.Principal;
using FinanceTracker.AccountRepository;
using System.Xml.Linq;



bool mainExit = false;
bool showMainMenu = false;

List<Account> accounts = AccountRepository.LoadAccounts();

do
{
    if (accounts.Count > 0 )
    {
        foreach ( var account in accounts )
        {
            Console.WriteLine($"Account: {account.Name}, Balance {account.Balance} {account.Currency.ToString()}");
        }
        
    }
     
    



    string entry = View.MainMenu(accounts);
    int entryAsInt;
    bool isInt = Int32.TryParse(entry, out entryAsInt);


    if (isInt)
    {
        if (entryAsInt <= accounts.Count)
        {
            View.TransactionMenu(accounts[entryAsInt - 1]);
            View.AfterTransactionMenuLoop(entry, accounts[entryAsInt - 1], showMainMenu, mainExit);

            break;
        }

        else if (entryAsInt == accounts.Count + 1 && entryAsInt > accounts.Count)
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

        else if (entryAsInt == 4)
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







