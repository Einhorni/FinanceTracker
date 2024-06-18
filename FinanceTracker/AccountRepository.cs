using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Classes;
using Newtonsoft.Json;

namespace FinanceTracker.AccountRepository
{

    //internal class AccountDTO
    //{
    //    public Guid Id;
    //    public string Name = string.Empty;
    //    public decimal Balance;
    //    public string Currency = string.Empty;
    //    public string AccountType = string.Empty;

    //    public static string GetAccountTypeString(FinanceTracker.Classes.Account account)
    //    {
    //        string accountType;
    //        switch (account)
    //        {
    //            case Girokonto:
    //                accountType = "Girokonto";
    //                break;
    //            case Tagesgeldkonto:
    //                accountType = "Tagesgeldkonto";
    //                break;
    //            case Festgeldkonto:
    //                accountType = "Festgeldkonto";
    //                break;
    //            case Brokerkonto:
    //                accountType = "Brokerkonto";
    //                break;
    //            case Bargeld:
    //                accountType = "Bargeld";
    //                break;

    //            default:
    //                accountType = "";
    //                break;
    //        }
    //        return accountType;
    //    }

    //constructors

    //public AccountDTO()
    //{ }

    //TODO: nur für Girokonto?? was ist bei einer anderen art von konto?
    //public  AccountDTO(Account account)        
    //{
    //    Id = account.Id;
    //    Name = account.Name;
    //    Balance = account.Balance;
    //    Currency = account.Currency.ToString();
    //    AccountType = GetAccountTypeString(account);
    //}
    //}

    

    internal class AccountRepository
    {

        public static string directory = @"C:\Progammieren\FinanceTracker\FinanceTracker\SavaData\";
        public static string file = "accounts.txt";

        //methods

        //public static Currency GetCurrencyFromString (string currencyString)
        //{
        //    Currency currency = Currency.EUR;
        //    switch (currencyString)
        //    {
        //        case "EUR":
        //            currency = Currency.EUR;
        //            break;
        //        case "Dollar":
        //            currency = Currency.Dollar;
        //            break;
        //        case "Bitcoin":
        //            currency = Currency.Bitcoin;
        //            break;
        //        case "ETF":
        //            currency = Currency.ETF;
        //            break;
        //    }
        //    return currency;
        //}

        //public static Account CreateAccountFromDTO(AccountDTO accountDTO)
        //{
        //    Account newAccount = null;

        //    switch (accountDTO.AccountType)
        //    {
        //        case "Girokonto":

        //            newAccount = new Girokonto(accountDTO.Name, accountDTO.Balance, GetCurrencyFromString(accountDTO.Currency), accountDTO.Id);

        //            break;
        //            //case "Tagesgeldkonto":
        //            //    Tagesgeldkonto account = new(accountDTO.Name, accountDTO.Balance, GetCurrency(accountDTO.Currency));
        //            //    break;
        //            //case "Festgeldkonto":
        //            //    Festgeldkonto account = new(accountDTO.Name, accountDTO.Balance, GetCurrency(accountDTO.Currency));
        //            //    break;
        //            //case "Brokerkonto":
        //            //    Brokerkonto account = new(accountDTO.Name, accountDTO.Balance, GetCurrency(accountDTO.Currency));
        //            //    break;
        //            //case "Bargeld":
        //            //    Bargeld account = new(accountDTO.Name, accountDTO.Balance, GetCurrency(accountDTO.Currency));
        //            //    break;
        //    }
        //    return newAccount;
        //}

        public static List<Account> LoadAccounts()
        {
            string path = $"{directory}{file}";
            bool fileExists = File.Exists(path);

            if (fileExists)
            {
                string jsonFile = File.ReadAllText(path);

                //ALT: List<Account> accounts = System.Text.Json.JsonSerializer.Deserialize<List<Account>>(jsonFile);
                List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(jsonFile);

                //erkennt jetzt nicht, um welchen Kontotyp es sich handelt.
                // if else mit 'is' machen und als else: Girokonto, da es dem normalen Account gleich, wenn keine Zinsen oder Dispo eingegeben sind

                return accounts;
            }

            else
            {
                List<Account>  Accounts = new ();
                return Accounts;
            } 
        }

        //falls 'is' nicht funktioniert
        //public static class Testclass
        //{
        //    public static Account RightAccount(Currency currency) => currency switch
        //    {
        //        Currency.Bitcoin => Brokerkonto, //Instanziieren
        //        Currency.Dollar => new Girokonto("da", 1, Currency.EUR, Guid.NewGuid()),
        //        _ => Girokonto //Instanziieren

        //    };
        //}

        public static void SaveAccounts(List<Account> accounts)
        {
            string path = $"{directory}{file}";
            bool fileExists = File.Exists(path);

            if (!fileExists)            
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);                
            }

            //ALT: string jsonFile = System.Text.Json.JsonSerializer.Serialize(accountsDTO);
            string jsonFile = JsonConvert.SerializeObject(accounts);

            File.WriteAllText(path, jsonFile);

            return;
        }
    }

    internal class TransactionRepository
    {
        public static string directory = @"C:\Progammieren\FinanceTracker\FinanceTracker\SavaData\";
        public static string file = "transactions.txt";

        public List<Transaction> Transactions { get; set; } //notwendig?

        private List<Transaction> LoadTransactions()
        { return Transactions; }

        private void SaveTransactions(List<Transaction> transactions)
        {
            string path = $"{directory}{file}";
            bool fileExists = File.Exists(path);

            if (!fileExists)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
            

            string jsonFile = JsonConvert.SerializeObject(transactions);

            File.WriteAllText(path, jsonFile);

            return;
        }
    }
}
