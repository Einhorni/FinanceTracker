using FinanceTracker.Utilities;
using MoneyManagement;
using MoneyManagement.DataAccess.FileAccess;
using MoneyManagement.Models;


bool mainExit = false;

MoneyManagementFileService accountManager = new (new FileAccountRepository());
List<AccountDTO> accounts = accountManager.Accounts ?? [];
//Load Categories
//var categories = 

do
{    
    string entry = View.MainMenu(accounts);
    int entryAsInt;
    bool mainMenuEntryIsInt = Int32.TryParse(entry, out entryAsInt);

    if (mainMenuEntryIsInt)
    {
        if (entryAsInt <= accounts.Count)
        {
            View.AccountMenu(accounts[entryAsInt - 1], accounts);
            continue;
        }

        else if (entryAsInt == accounts.Count + 1 && entryAsInt > accounts.Count)
        {
            //View.TransferMenuLoop(showMainMenu, accounts);
            var newaccounts = View.TransferMenu(accounts);
            accounts = newaccounts;
        }

        else if (entryAsInt == accounts.Count + 2 && entryAsInt > accounts.Count)
            View.CreateAccountMenu(accounts);

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







