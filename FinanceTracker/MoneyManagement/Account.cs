namespace FinanceTracker.Classes
{
    
    public class Account
    {
        
        public Guid Id;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public Currency Currency { get; set; } //= default!;
           
        public decimal AddIncome(decimal amount)
        { return Balance + amount; }

        public decimal SubstractExpense(decimal amount)
        { return Balance - amount; } 
    }

    public class Girokonto : Account
    {
        //private decimal Zinssatz {  get; set; }
        public decimal OverdraftLimit {  get; set; }
        //BankName
        //IBAN
        //BIC
        public Girokonto () { }
        public Girokonto(string name, decimal balance, Currency currency, Guid id, decimal overdraftLimit)
        {

            if (id == Guid.Empty)
                Id = Guid.NewGuid();
            else Id = id;
            Name = name;
            Balance = balance;
            Currency = currency;
            OverdraftLimit = overdraftLimit;
        }
    }

    public class Bargeldkonto : Account
    {
        public Bargeldkonto() { }
        public Bargeldkonto(string name, decimal balance, Currency currency, Guid id)
        {

            if (id == Guid.Empty)
                Id = Guid.NewGuid();
            else Id = id;
            Name = name;
            Balance = balance;
            Currency = currency;
        }
    }

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
    //    //Portfolio List[Name;]
    //    public decimal AddIncome(decimal amount, decimal kurs)
    //    { return Balance + amount; }

    //    private decimal SubstractExpense(decimal amount, decimal kurs)
    //    { return Balance - amount; }

    //    //Kursverlauf??
    //}



}
