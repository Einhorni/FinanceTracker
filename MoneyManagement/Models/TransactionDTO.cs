using MoneyManagement.Models;

namespace MoneyManagement.Models
{
    public class TransactionDTO
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;
        public Guid AccountId { get; set; }

        //Alt: Sending/ReceivingAccount und nur TransferAccountId
        public Guid? ToAccountId { get; set; }
        public Guid? FromAccountId { get; set; }
    }


    //one time one way transaction (one acc)
    public class IrregularTransaction : TransactionDTO
    {
        public IrregularTransaction(decimal amount, string category, Guid currentAccountId)
        {
            TransactionId = Guid.NewGuid();
            Amount = amount;
            Date = DateTime.Now;
            Category = category;
            AccountId = currentAccountId;
        }
    }
    

    //one time two way transaction (two accs)
    public class IrregularTransfer : TransactionDTO
    {
        public IrregularTransfer(decimal amount, string category, Guid fromAccountId, Guid toAccountId)
        {
            TransactionId = Guid.NewGuid();
            Amount = amount;
            Date = DateTime.Now;
            Category = category;
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            AccountId = fromAccountId;
        }
    }

    #region for later use
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
    #endregion

}
