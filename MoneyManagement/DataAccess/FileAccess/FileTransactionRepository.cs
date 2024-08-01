using Newtonsoft.Json;
using MoneyManagement.Models;
using System.Collections.Generic;

namespace MoneyManagement.DataAccess.FileAccess
{
    public class FileTransactionRepository : IFileTransactionRepository
    {
        //for production use:
        //string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static string directory = @"C:\Progammieren\FinanceTracker\FinanceTracker\SavaData\";
        private static string file = "transactions.txt";
        public static string path = $"{directory}{file}";

        public List<TransactionDTO> LoadTransactions()
        {
            bool fileExists = File.Exists(path);

            if (fileExists)
            {
                try
                {
                    string jsonFile = File.ReadAllText(path);

                    //ALT: List<Account> accounts = System.Text.Json.JsonSerializer.Deserialize<List<Account>>(jsonFile);
                    //mit dem zusätzlichen Parameter unterscheidet JsonConvert zwischen den Typen
                    List<TransactionDTO> transactions = JsonConvert.DeserializeObject<List<TransactionDTO>>(jsonFile, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                    return transactions;
                }
                catch (JsonException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (FileLoadException ex)
                {
                    Console.WriteLine(ex.Message);
                    List<TransactionDTO> transactions = new();
                    return transactions;
                }
            }

            else
            {
                List<TransactionDTO> transactions = new();
                return transactions;
            }
        }


        public void SaveTransaction(TransactionDTO transaction)
        {
            bool fileExists = File.Exists(path);

            if (!fileExists)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }

            List<TransactionDTO> transactions = LoadTransactions();

            transactions.Add(transaction);

            try
            {
                //ALT: string jsonFile = System.Text.Json.JsonSerializer.Serialize(accountsDTO);
                //mit dem zusätzlichen Parameter unterscheidet JsonConvert zwischen den Typen
                string jsonFile = JsonConvert.SerializeObject(transactions, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

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
