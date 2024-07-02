using FinanceTracker.MoneyManagement;
using Newtonsoft.Json;

namespace FinanceTracker.DataAccess
{


    internal class AccountRepository : IAccountRepository
    {
        //string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
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
            string path = $"{directory}{file}";
            bool fileExists = File.Exists(path);

            if (!fileExists)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }

            //ALT: string jsonFile = System.Text.Json.JsonSerializer.Serialize(accountsDTO);
            //mit dem zusätzlichen Parameter unterscheidet JsonConvert zwischen den Typen
            string jsonFile = JsonConvert.SerializeObject(accounts, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

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
