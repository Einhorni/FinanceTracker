namespace FinanceTracker.MoneyManagement
{

    public class Account
    {

        public Guid Id;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }

        //better use 2 different methods: adding and substracting

        //single transaction
        public decimal CalculateNewBalance(decimal amount)
        { return Balance + amount; }


        //transfer between 2 accounts
        public decimal CalculateNewBalanceSendingAccount(Currency currencyTo, decimal amount)
        {
            Balance = Balance - amount - GetTransferFee(currencyTo, amount); 
            return Balance;
        }

        //transfer between 2 accounts
        public decimal CalculateNewBalanceReceivingAccount(Currency currencyTo, decimal amountFromSender)
        {
            decimal amount = CalculateAmountReveivingAccount(currencyTo, amountFromSender); 
            Balance = Balance + amountFromSender;
            return Balance;
        }

        //currency conversion
        public decimal CalculateAmountReveivingAccount(Currency currencyTo, decimal amount)
        {
            decimal receivingAmout = default;
            var exchangeRates = new MockCurrencyExchangeRates();
            if (Currency == Currency.EUR)
            switch (Currency)
            {
                case Currency.EUR:
                    switch (currencyTo)
                    {
                        case Currency.Dollar:
                            return receivingAmout = exchangeRates.euroToDollar * amount;
                        case Currency.Bitcoin:
                            return receivingAmout = exchangeRates.euroToBitcoin * amount;
                        case Currency.ETF:
                            return receivingAmout = exchangeRates.euroToEtf * amount;
                        }
                    break;
                case Currency.Dollar:
                    switch (currencyTo)
                    {
                        case Currency.EUR:
                            return receivingAmout = exchangeRates.dollarToEuro * amount;
                        case Currency.Bitcoin:
                            return receivingAmout = exchangeRates.dollarToBitcoin * amount;
                        case Currency.ETF:
                            return receivingAmout = exchangeRates.dollarToEtf * amount;
                        }
                    break;
                case Currency.ETF:
                    switch (currencyTo)
                    {
                        case Currency.EUR:
                            return receivingAmout = exchangeRates.etfToEuro * amount;
                        case Currency.Bitcoin:
                            return receivingAmout = exchangeRates.etfToBitcoin * amount;
                        case Currency.Dollar:
                            return receivingAmout = exchangeRates.etfToDollar * amount;
                    }
                    break;
                case Currency.Bitcoin:
                    switch (currencyTo)
                    {
                        case Currency.EUR:
                            return receivingAmout = exchangeRates.bitcoinToEuro * amount;
                        case Currency.Dollar:
                            return receivingAmout = exchangeRates.bitcoinToDollar * amount;
                        case Currency.ETF:
                            return receivingAmout = exchangeRates.bitcoinToEtf* amount;
                    }
                    break;
                default: break;
            }
            return receivingAmout;
        }

        public decimal GetTransferFee(Currency currencyTo, decimal amount)
        {
            if (Currency == currencyTo) return 0.0m;
            else
                return (amount * 0.01m);
        }

        //for later use
        //public decimal SubstractExpense(decimal amount)
        //{ return Balance - amount; } 
    }

    public class Girokonto : Account
    {
        //for later use
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
    //    //Portfolio List[Name;]
    //    public decimal AddIncome(decimal amount, decimal kurs)
    //    { return Balance + amount; }

    //    private decimal SubstractExpense(decimal amount, decimal kurs)
    //    { return Balance - amount; }

    //    //Kursverlauf??
    //}

    #endregion

}
