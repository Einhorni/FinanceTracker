using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.MoneyManagement
{
    //exchange rates and prices would normally come over an API
    internal class MockCurrencyExchangeRates
    {        
        public readonly decimal euroToDollar = 0.93m;
        public readonly decimal dollarToEuro = 1.08m;

        public readonly decimal bitcoinToEuro = 1.08m;
        public readonly decimal euroToBitcoin = 53016.30m;

        public readonly decimal bitcoinToDollar = 0.00001887m;
        public readonly decimal dollarToBitcoin = 53107.77m;

        public readonly decimal etfToEuro = 0.0096m;
        public readonly decimal euroToEtf = 104.06m;

        public readonly decimal dollarToEtf = 0.00889m;
        public readonly decimal etfToDollar = 112.43m;

        public readonly decimal bitcoinToEtf = 0.00212m;
        public readonly decimal etfToBitcoin = 472.363m;

    }
}
