using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyManagement.DataAccess.FileAccess;
using MoneyManagement.Models;


namespace MoneyManagement
{
    public class MoneyManagementFileService
    {

        private readonly IFileAccountRepository? _accountRepository;
        public List<Account>? Accounts;
        private readonly IFileTransactionRepository? _transactionRepository;
        public List<Transaction>? Transactions;



        public MoneyManagementFileService(IFileAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            Accounts = LoadAccounts();
        }

        public MoneyManagementFileService(IFileTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
            Transactions = LoadTransactions();
        }

        public MoneyManagementFileService(IFileAccountRepository accountRepository, IFileTransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            Accounts = LoadAccounts();
            _transactionRepository = transactionRepository;
            Transactions = LoadTransactions();
        }


        public List<Account> LoadAccounts()
        { return _accountRepository.LoadAccounts(); }

        public void SaveAccounts(List<Account> accounts)
        { _accountRepository.SaveAccounts(accounts); }

        public List<Transaction> LoadTransactions()
        {
            return _transactionRepository.LoadTransactions();
        }

        public void SaveTransaction(Transaction transaction)
        {
            _transactionRepository.SaveTransaction(transaction);
        }

    }
}
