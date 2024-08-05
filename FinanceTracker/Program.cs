using FinanceTracker.Utilities;
using MoneyManagement;
using MoneyManagement.DataAccess;


bool mainExit = false;

//MoneyManagementFileService accountManager = new (new FileAccountRepository());
//List<AccountDTO> accounts = accountManager.Accounts ?? [];


MoneyManagementService accountManager = MoneyManagementService.Create();
//var categories = moneymanager.GetCategories();
var accountDtos = await accountManager.LoadAccounts();

do
{    
    string entry = View.MainMenu(accountDtos);
    int entryAsInt;
    bool mainMenuEntryIsInt = Int32.TryParse(entry, out entryAsInt);

    if (mainMenuEntryIsInt)
    {
        if (entryAsInt <= accountDtos.Count)
        {
            View.AccountMenu(accountDtos[entryAsInt - 1], accountDtos, accountManager);
            continue;
        }

        else if (entryAsInt == accountDtos.Count + 1 && entryAsInt > accountDtos.Count)
        {
            //View.TransferMenuLoop(showMainMenu, accounts);
            var newaccounts = View.TransferMenu(accountDtos, accountManager);
            accountDtos = newaccounts;
        }

        else if (entryAsInt == accountDtos.Count + 2 && entryAsInt > accountDtos.Count)
            View.CreateAccountMenu(["Dollar", "Euro"], accountManager);

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







