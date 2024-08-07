using FinanceTracker.Utilities;
using MoneyManagement;
using MoneyManagement.DataAccess;


//TODO: Schleife auslagern, Program.cs soll nur aufrufen

bool mainExit = false;

//MoneyManagementFileService accountManager = new (new FileAccountRepository());
//List<AccountDTO> accounts = accountManager.Accounts ?? [];


MoneyManagementService accountManager = MoneyManagementService.Create();
//var categories = moneymanager.GetCategories();



//TODO: If else verschlanken!
do
{
    var accountDtos = accountManager.LoadAccounts().Result;
    string entry = View.MainMenu(accountDtos);
    int entryAsInt;
    bool mainMenuEntryIsInt = Int32.TryParse(entry, out entryAsInt);

    if (mainMenuEntryIsInt)
    {
        //Show Account Menu
        if (entryAsInt <= accountDtos.Count)
        {
            View.AccountMenu(accountDtos[entryAsInt - 1], accountDtos, accountManager);
            continue;
        }

        //Show Transer between two accounts Menu only if there are at least 2 accounts
        else if (entryAsInt == accountDtos.Count + 1 && accountDtos.Count > 1)
        {
            var newaccounts = View.TransferMenu(accountDtos, accountManager);
            accountDtos = newaccounts;
        }

        else if (entryAsInt == accountDtos.Count + 1 && accountDtos.Count == 1)
            View.CreateAccountMenu(["Dollar", "Euro"], accountManager);

        else if (entryAsInt == accountDtos.Count + 2 && accountDtos.Count > 1)
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







