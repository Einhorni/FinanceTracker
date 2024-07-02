using FinanceTracker.MoneyManagement;

namespace FinanceTracker.MoneyManagement
{
    public class Transaction
    {
        public Guid TransactionId {  get; set; }
        public decimal Amount { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public Category Category { get; set; }
        public Guid AccountId { get; set; }
    }


    public class IrregularTransaction : Transaction
    {
        public IrregularTransaction(decimal amount, Category category, Guid currentAccountId)
        {
            TransactionId = Guid.NewGuid();
            Amount = amount;
            Date = DateTime.Now;
            Category = category;
            AccountId = currentAccountId;
        }
    }

    //class Regular : Transaction
    //{
    //    public decimal Amount { get; set; }
    //    public string Title { get; set; } = String.Empty;
    //    public DateTime Date { get; set; }
    //    public int Interval { get; set; }
    //    public string Category { get; set; } = String.Empty;

    //    //Intervall: Timespan / Date?
    //}


    //class RegularBroker : Transaction
    //{
    //    public decimal Amount { get; set; }
    //    public decimal Kurs { get; set; }
    //    public string Title { get; set; } = String.Empty;
    //    public DateTime Date { get; set; }
    //    public string Category { get; set; } = String.Empty;

    //}

    //class IrregularBroker : Transaction
    //{
    //    public decimal Amount { get; set; }
    //    public decimal Kurs { get; set; }
    //    public string Title { get; set; } = String.Empty;
    //    public DateTime Date { get; set; }
    //    public string Category { get; set; } = String.Empty;

    //    public int Intervall { get; set; }//Monate: Berechnung dann woanders Timespan / Date? (+x Monate)
    //}


}
