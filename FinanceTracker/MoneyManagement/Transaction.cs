using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Classes
{
    public class Transaction
    {
        Guid Account {  get; set; }
        decimal Amount { get; set; }
        string Title { get; set; } = String.Empty;
        DateTime Date { get; set; }
        string Category { get; set; } = String.Empty;
    }


    class Regular : Transaction
    {
        public Regular (decimal Amount, string Title, string Category)
        {
            Guid account = Guid.NewGuid();
            decimal amount = Amount;
            string title = Title;
            DateTime date = DateTime.Now;
            string category = Category;
        }
    }

    class Irregular : Transaction
    {
        public decimal Amount { get; set; }
        public string Title { get; set; } = String.Empty;
        public DateTime Date { get; set; }
        public int Interval { get; set; }
        public string Category { get; set; } = String.Empty;

        //Intervall: Timespan / Date?
    }


    class RegularBroker : Transaction
    {
        public decimal Amount { get; set; }
        public decimal Kurs { get; set; }
        public string Title { get; set; } = String.Empty;
        public DateTime Date { get; set; }
        public string Category { get; set; } = String.Empty;

    }

    class IrregularBroker : Transaction
    {
        public decimal Amount { get; set; }
        public decimal Kurs { get; set; }
        public string Title { get; set; } = String.Empty;
        public DateTime Date { get; set; }
        public string Category { get; set; } = String.Empty;

        public int Intervall { get; set; }//Monate: Berechnung dann woanders Timespan / Date? (+x Monate)
    }


}
