using Newtonsoft.Json;
using FinanceTracker.Classes;

namespace FinanceTracker.DataAccess
{
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
