using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Classes;
using Newtonsoft.Json;

namespace FinanceTracker.DataAccess
{


    internal class AccountRepository : IAccountRepository
    {

        public static string directory = @"C:\Progammieren\FinanceTracker\FinanceTracker\SavaData\";
        public static string file = "accounts.txt";

        public List<Account> LoadAccounts()
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
                List<Account> Accounts = new();
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

        public void SaveAccounts(List<Account> accounts)
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

        public Account LoadAccountById(Guid id)
        {
            throw new NotImplementedException();
        }

        //public void SaveAccountById(Account account)
        //{
        //    throw new NotImplementedException();
        //}
    }

    
}
