using MoneyManagement.Models;
using Newtonsoft.Json;

namespace MoneyManagement.DataAccess.FileAccess
{
    public class FileAccountRepository : IFileAccountRepository
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
                try
                {
                    string jsonFile = File.ReadAllText(path);

                    //ALT: List<Account> accounts = System.Text.Json.JsonSerializer.Deserialize<List<Account>>(jsonFile);
                    //mit dem zusätzlichen Parameter unterscheidet JsonConvert zwischen den Typen
                    List<Account> accounts =
                        JsonConvert.DeserializeObject<List<Account>>(jsonFile, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }) ?? throw new NullReferenceException();

                    return accounts;
                }

                catch (JsonException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (FileLoadException ex)
                {
                    Console.WriteLine(ex.Message);
                    List<Account> Accounts = new();
                    return Accounts;
                }
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

            var savedAccounts = LoadAccounts();

            //alle accs die nicht verändert wurden / allte accounts und neue accounts anhängen
            savedAccounts.Where(sa => accounts.Any(a => a.Id != sa.Id));
            savedAccounts.AddRange(accounts);


            //ALT: string jsonFile = System.Text.Json.JsonSerializer.Serialize(accountsDTO);
            //mit dem zusätzlichen Parameter unterscheidet JsonConvert zwischen den Typen

            try
            {
                string jsonFile = JsonConvert.SerializeObject(accounts, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                File.WriteAllText(path, jsonFile);

                return;
            }

            catch (IOException ex)
            { Console.WriteLine(ex.Message); return; }
            catch (JsonException ex)
            { throw new Exception(ex.Message); }


        }
    }
}
