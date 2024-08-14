namespace MoneyManagement.Models
{

    public abstract class Account
    {

        public Guid Id;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; } 
        public DateTime DateOfCreation { get; set; }
        public string Currency { get; set; } // CodeReview: defaultWert erwartet, der nicht null ist


        public void ChangeAmount(decimal amount)
        { Balance += amount; }

        //public abstract bool TransactionNotValid(Transaction transaction); // CodeReview: auskommentierter Code!
        public abstract bool TransactionValid(Transaction transaction);
    }

    public class Girokonto : Account
    {
        //for later use
        public decimal OverdraftLimit { get; set; } 
        public Girokonto() { }
        public Girokonto(string name, decimal balance, string currency, Guid id, decimal overdraftLimit = 0.0m)
        {

            if (id == Guid.Empty) // CodeReview: Möglichkeit der If-Expression: Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
                Id = Guid.NewGuid();
            else Id = id;

            

            Name = name;
            Balance = balance;
            Currency = currency;
            OverdraftLimit = overdraftLimit;
            DateOfCreation = DateTime.Now;
        }


        public override bool TransactionValid(Transaction transaction)
        {
            if (transaction.Amount > 0) { return true; }
            else { return (-Balance - OverdraftLimit) <= transaction.Amount; } 
        }
    }

    public class Bargeldkonto : Account
    {
        public Bargeldkonto() { }
        public Bargeldkonto(string name, decimal balance, string currency, Guid id)
        {

            if (id == Guid.Empty) // CodeReview: Möglichkeit der If-Expression: Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
                Id = Guid.NewGuid();
            else Id = id;
            Name = name;
            Balance = balance;
            Currency = currency;
            DateOfCreation = DateTime.Now; // CodeReview: Wegen Zeitzonen, überlegen man UtcNow nutzen kann.
        }


        public override bool TransactionValid(Transaction transaction)
        {
            if (transaction.Amount > 0) { return true; }
            else { return (-Balance) <= transaction.Amount; }
        }
    }

    #region for later use
    //public class Tagesgeldkonto : Account
    //{
    //    private decimal Zinssatz { get; set; }
    //    private int ZinsIntervall { get; set; }

    //    public Tagesgeldkonto(string name, decimal balance, Currency currency, Guid id)
    //    {

    //        if (id == Guid.Empty)
    //            Id = Guid.NewGuid();
    //        else Id = id;
    //        Name = name;
    //        Balance = balance;
    //        Currency = currency;
    //    }
    //}

    //public class Festgeldkonto : Account
    //{
    //    private DateTime Anlagedatum { get; set; }
    //    private TimeSpan Laufzeit { get; set; }
    //    private decimal Zinssatz { get; set; }
    //    private int ZinsIntervall { get; set; }

    //    public Festgeldkonto(string name, decimal balance, Currency currency, Guid id)
    //    {

    //        if (id == Guid.Empty)
    //            Id = Guid.NewGuid();
    //        else Id = id;
    //        Name = name;
    //        Balance = balance;
    //        Currency = currency;
    //    }
    //}


    //public class Brokerkonto : Account
    //{
    //    //Transferkonto
    //neue DB Tabelle für Investments
    //    //Portfolio List [Name;]
    //    public decimal AddIncome(decimal amount, decimal kurs)
    //    { return Balance + amount; }

    //    private decimal SubstractExpense(decimal amount, decimal kurs)
    //    { return Balance - amount; }

    //    //Kursverlauf??
    //}

    #endregion

}
