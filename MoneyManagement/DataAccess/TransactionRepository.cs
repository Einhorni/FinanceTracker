using Newtonsoft.Json;
using MoneyManagement.Models;
using System.Collections.Generic;

namespace FinanceTracker.DataAccess
{
    public class TransactionRepository : ITransactionRepository
    {
        //for production use:
        //string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static string directory =  @"C:\Progammieren\FinanceTracker\FinanceTracker\SavaData\";
        private static string file = "transactions.txt";
        public static string path = $"{directory}{file}";

        public List<Transaction> LoadTransactions()
        { 
            bool fileExists = File.Exists(path);

            if (fileExists)
            {
                try
                {
                    string jsonFile = File.ReadAllText(path);

                    //ALT: List<Account> accounts = System.Text.Json.JsonSerializer.Deserialize<List<Account>>(jsonFile);
                    //mit dem zusätzlichen Parameter unterscheidet JsonConvert zwischen den Typen
                    List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonFile, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                    return transactions;
                }
                catch (JsonException ex)
                { 
                    throw new Exception(ex.Message); 
                }
                catch (FileLoadException ex)
                { 
                    Console.WriteLine(ex.Message);
                    List<Transaction> transactions = new();
                    return transactions;
                }
            }

            else
            {
                List<Transaction> transactions = new();
                return transactions;
            }
        }


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
            { throw new Exception(ex.Message);}

        }

    }
}
