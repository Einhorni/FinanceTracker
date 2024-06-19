using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinanceTracker.Classes
{
    
    public class Account
    {
        
        public Guid Id;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public Currency Currency { get; set; } //= default!;
        //private List<Transaction> Transactions { get; set; }
           
        

        public decimal AddIncome(decimal amount)
        { return Balance + amount; }

        public decimal SubstractExpense(decimal amount)
        { return Balance - amount; }

        public static Currency CreateCurrency (string currencyString)
        {
            Currency currency = new();
            switch (currencyString)
            {
                case "b":
                    currency = Currency.Bitcoin;
                    break;

                case "d":
                    currency = Currency.Dollar;
                    break;

                case "e":
                    currency = Currency.EUR;
                    break;

                case "f":
                    currency = Currency.ETF;
                    break;
            }
            return currency;
        }
        
    }

    public class Girokonto : Account
    {
        private bool Zinsen { get; set; }
        private decimal Zinssatz {  get; set; }
        private int ZinsIntervall { get; set; }
        private decimal Dispo {  get; set; }//noch nicht eingebaut

        public Girokonto(string name, decimal balance, Currency currency, Guid id)
        {

            if (id == Guid.Empty)
                Id = Guid.NewGuid();
            else Id = id;
            Name = name;
            Balance = balance;
            Currency = currency;
        }
    }

    public class Tagesgeldkonto : Account
    {
        private bool Zinsen { get; set; }
        private decimal Zinssatz { get; set; }
        private int ZinsIntervall { get; set; }
    }

    public class Festgeldkonto : Account
    {
        private TimeSpan Laufzeit { get; set; }
        private bool Zinsen { get; set; }
        private decimal Zinssatz { get; set; }
        private int ZinsIntervall { get; set; }
    }

    public class Brokerkonto : Account
    {
        public decimal AddIncome(decimal amount, decimal kurs)
        { return Balance + amount; }

        private decimal SubstractExpense(decimal amount, decimal kurs)
        { return Balance - amount; }

        //Kursverlauf??
    }

    public class Bargeld : Account
    { 
    }

}
