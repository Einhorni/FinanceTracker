using FinanceTracker.Classes;
using FinanceTracker.Utilities;
using FinanceTracker.DataAccess;
using FinanceTracker.MoneyManagement;



bool mainExit = false;
bool showMainMenu = false;

AccountManager accountManager = new AccountManager(new AccountRepository());
List<Account> accounts = accountManager.Accounts;

do
{
    if (accounts.Count > 0) 
        View.ShowAccounts(accounts);
     
    string entry = View.MainMenu(accounts);
    int entryAsInt;
    bool mainMenuEntryIsInt = Int32.TryParse(entry, out entryAsInt);

    if (mainMenuEntryIsInt)
    {
        if (entryAsInt <= accounts.Count)
        {
            View.TransactionMenu(accounts[entryAsInt - 1], accounts);
            View.AfterTransactionMenuLoop(entry, accounts[entryAsInt - 1], showMainMenu, mainExit, accounts);

            continue;
        }

        else if (entryAsInt == accounts.Count + 1 && entryAsInt > accounts.Count)
            View.TransferMenu(showMainMenu, accounts);

        else if (entryAsInt == accounts.Count + 2 && entryAsInt > accounts.Count)
            View.CreateAccountMenu(showMainMenu, accounts);

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







