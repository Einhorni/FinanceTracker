using FinanceTracker.Classes;
using FinanceTracker.DataAccess;
using FinanceTracker.MoneyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.MoneyManagement
{
    public class TransactionManager
    {
        private readonly ITransactionRepository _transactionRepository;
        public List<Transaction> Transactions;

        public TransactionManager(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
            Transactions = LoadTransactions();
        }

        public List<Transaction> LoadTransactions()
        {
            return _transactionRepository.LoadTransactions();
        }

        //public void SaveTransactions(List<Transaction> transactions)
        //{
        //    _transactionRepository.SaveTransactions(transactions);
        //}

        public void SaveTransaction(Transaction transaction)
        {
            _transactionRepository.SaveTransaction(transaction);
        }

    }

    
}


