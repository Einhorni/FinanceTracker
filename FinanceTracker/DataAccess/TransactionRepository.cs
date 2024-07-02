using Newtonsoft.Json;
using FinanceTracker.MoneyManagement;

namespace FinanceTracker.DataAccess
{
    public class TransactionRepository : ITransactionRepository
    {
        //string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static string directory =  @"C:\Progammieren\FinanceTracker\FinanceTracker\SavaData\";
        private static string file = "transactions.txt";

        public static string path = $"{directory}{file}";
        //bool fileExists = File.Exists(path);

        public List<Transaction> LoadTransactions()
        { 
            bool fileExists = File.Exists(path);

            if (fileExists)
            {
                string jsonFile = File.ReadAllText(path);

                //ALT: List<Account> accounts = System.Text.Json.JsonSerializer.Deserialize<List<Account>>(jsonFile);
                //mit dem zusätzlichen Parameter unterscheidet JsonConvert zwischen den Typen
                List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonFile, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                return transactions;
            }

            else
            {
                List<Transaction> transactions = new();
                return transactions;
            }
        }

        //private void SaveTransactions(List<Transaction> transactions)
        //{
        //    bool fileExists = File.Exists(path);

        //    if (!fileExists)
        //    {
        //        if (!Directory.Exists(directory))
        //            Directory.CreateDirectory(directory);
        //    }

        //    string jsonFile = JsonConvert.SerializeObject(transactions, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

        //    File.WriteAllText(path, jsonFile);

        //    return;
        //}

        public void SaveTransaction(Transaction transaction)
        {
            bool fileExists = File.Exists(path);

            if (!fileExists)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }

            List<Transaction> transactions = LoadTransactions();

            transactions.Add(transaction);

            string jsonFile = JsonConvert.SerializeObject(transactions, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            File.WriteAllText(path, jsonFile);

            return;
        }

    }
}
