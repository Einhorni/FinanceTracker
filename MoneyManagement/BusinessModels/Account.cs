namespace MoneyManagement.Models
{

    public abstract class Account
    {

        public Guid Id;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; } 
        public DateTime DateOfCreation { get; set; }
        public string Currency { get; set; } = string.Empty; 


        public void ChangeAmount(decimal amount)
        { Balance += amount; }

        public abstract bool TransactionValid(Transaction transaction);
    }

    public class Girokonto : Account
    {
        public decimal OverdraftLimit { get; set; } 
        public Girokonto() { }
        public Girokonto(string name, decimal balance, string currency, Guid id, decimal overdraftLimit = 0.0m)
        {
            Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
            Name = name;
            Balance = balance;
            Currency = currency;
            OverdraftLimit = overdraftLimit;
            DateOfCreation = DateTime.UtcNow;
        }


        public override bool TransactionValid(Transaction transaction)
        {
            //valid, if amount positive or balance and overdraftlimit more than amount
            if (transaction.Amount > 0) { return true; }
            else { return (-Balance - OverdraftLimit) <= transaction.Amount; } 
        }
    }

    public class Bargeldkonto : Account
    {
        public Bargeldkonto() { }
        public Bargeldkonto(string name, decimal balance, string currency, Guid id)
        {
            Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
            Name = name;
            Balance = balance;
            Currency = currency;
            DateOfCreation = DateTime.UtcNow; 
        }


        public override bool TransactionValid(Transaction transaction)
        {
            //valid, if amount positive or balance more than amount
            if (transaction.Amount > 0) { return true; }
            else { return (-Balance) <= transaction.Amount; }
        }
    }
}
