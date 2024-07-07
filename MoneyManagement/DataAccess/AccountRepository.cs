using MoneyManagement.Models;
using Newtonsoft.Json;

namespace FinanceTracker.DataAccess
{
    public class AccountRepository : IAccountRepository
    {
        //for production use:
        //string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static string directory = @"C:\Progammieren\FinanceTracker\FinanceTracker\SavaData\";
        private static string file = "accounts.txt";
        string path = $"{directory}{file}";

        public List<Account> LoadAccounts()
        {            
            bool fileExists = File.Exists(path);

            if (fileExists)
            {
                string jsonFile = File.ReadAllText(path);

                //ALT: List<Account> accounts = System.Text.Json.JsonSerializer.Deserialize<List<Account>>(jsonFile);
                //mit dem zusätzlichen Parameter unterscheidet JsonConvert zwischen den Typen
                List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(jsonFile, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                return accounts;
            }

            else
            {
                List<Account> Accounts = new();
                return Accounts;
            }
        }


        public void SaveAccounts(List<Account> accounts)
        {
            bool fileExists = File.Exists(path);

            if (!fileExists)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }

            //Accounts laden
            //falls accounts vorhanden, dann ersetzen, wenn nicht, dann anhängen
            var savedAccounts = LoadAccounts();
            //alle gespeicherten accs, die nicht die id der accs haben
            savedAccounts.Where(sa => accounts.Any(a => a.Id != sa.Id));
            savedAccounts.AddRange(accounts);


            //ALT: string jsonFile = System.Text.Json.JsonSerializer.Serialize(accountsDTO);
            //mit dem zusätzlichen Parameter unterscheidet JsonConvert zwischen den Typen
            string jsonFile = JsonConvert.SerializeObject(accounts, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            File.WriteAllText(path, jsonFile);

            return;
        }
    }  
}
