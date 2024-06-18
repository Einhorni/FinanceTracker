using FinanceTracker;
using FinanceTracker.Classes;
using FinanceTracker.Utilities;
using System.ComponentModel.Design;
using System.Security.Principal;
using FinanceTracker.AccountRepository;
using System.Xml.Linq;


//const: werte, die sich nie ändern, mathematische Dinger
//readonly, z. B. connections string

//Accounttyp mit "is" erkennen, als else: Girokonto
//In CreateAccountFromDTO() testen, ob ich auch einfach die Klasse erkennen kann mit 
//if (account is Girokonto)
//oder switch (account) {case is Girokonto: ...} 

/*      TODO: 
 *      Log-In
 *      Begrüßung
 *      try catch blocks in IOs   
 *      Balance not valid - else Block: 2 mal falsche Eingabe, dann MainMenu mit Hinweis: falsche Eingabe
 *Accounts
 *      2! max. 7 accounts
 *      bei weiteren muss ein anderes gelöscht werden
 *      1! Anlegen unterschiedlicher Konten
 *      Beschränken der Currency je nach Ktotyp
 *Transaktionen
 *      Addieren und Subtrahieren als 2 verschiedene Operationen
 *      3! Transaktionen in separater Datei speichern (mit Kto Id)
 *      Transaktionen laden und mit richtigem Konto zusammenbringen -> neue Balance berechnen
 *      Transactions zwischen Accounts, wenn Curreny passt - im Fall D/E, Währung anpassen?
 *Beenden
 *      Automatisches Speichern der Transaktionen vor Programmbeendigung
 *      
 *Tests
 *      
 *      
 * TODO: Transaction Types und Transkationen irgendwann in DB speichern
*/



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







