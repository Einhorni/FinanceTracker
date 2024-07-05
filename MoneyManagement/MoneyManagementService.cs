using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinanceTracker.DataAccess;
using MoneyManagement.Models;


namespace MoneyManagement
{
    public class MoneyManagementService
    {

        private readonly IAccount? _account;
        private readonly IAccountRepository? _accountRepository;
        public List<Account>? Accounts;
        private readonly ITransactionRepository? _transactionRepository;
        public List<Transaction>? Transactions;



        public MoneyManagementService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            Accounts = LoadAccounts();
        }

        public MoneyManagementService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
            Transactions = LoadTransactions();
        }

        public MoneyManagementService(IAccount account, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
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
