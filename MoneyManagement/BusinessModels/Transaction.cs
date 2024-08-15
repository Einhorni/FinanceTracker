using MoneyManagement.Models;

namespace MoneyManagement.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;
        public Guid AccountId { get; set; }

        public Guid? ReceivingAccountId { get; set; } = Guid.Empty;
        public Guid? SendingAccountId { get; set; } = Guid.Empty;

        public Transaction(decimal amount, string category, Guid accountId)
        {
            TransactionId = Guid.NewGuid();
            Amount = amount;
            Date = DateTime.UtcNow;
            Category = category;
            AccountId = accountId;
        }

        public Transaction(decimal amount, string category, Guid fromAccountId, Guid toAccountId, Guid accountId)
        {
            TransactionId = Guid.NewGuid();
            Amount = amount;
            Date = DateTime.UtcNow;
            Category = category;
            SendingAccountId = fromAccountId;
            ReceivingAccountId = toAccountId;
            AccountId = accountId;
        }
    }
}
