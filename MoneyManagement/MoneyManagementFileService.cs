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

        private readonly IAccount? _account;
        private readonly IFileAccountRepository? _accountRepository;
        public List<AccountDTO>? Accounts;
        private readonly IFileTransactionRepository? _transactionRepository;
        public List<TransactionDTO>? Transactions;



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

        public MoneyManagementFileService(IAccount account, IFileAccountRepository accountRepository, IFileTransactionRepository transactionRepository)
        {
            _account =  account;
            _accountRepository = accountRepository;
            Accounts = LoadAccounts();
            _transactionRepository = transactionRepository;
            Transactions = LoadTransactions();
        }


        public decimal AddAmount(decimal amount)
        { return _account.AddAmount(amount); }
        public decimal SubstractAmount(decimal amount)
        { return _account.SubstractAmount(amount); }


        public List<AccountDTO> LoadAccounts()
        { return _accountRepository.LoadAccounts(); }

        public void SaveAccounts(List<AccountDTO> accounts)
        { _accountRepository.SaveAccounts(accounts); }

        public List<TransactionDTO> LoadTransactions()
        {
            return _transactionRepository.LoadTransactions();
        }

        public void SaveTransaction(TransactionDTO transaction)
        {
            _transactionRepository.SaveTransaction(transaction);
        }

    }
}
