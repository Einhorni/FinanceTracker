using MoneyManagement.Models;

namespace FinanceTracker.UIMappings
{
    internal class UIMappings
    {
        public static string MapToCurrencyString(string currencyString)
        {
            switch (currencyString.ToUpperInvariant())
            {
                case "E":
                    return "Euro";
                case "D":
                    return "Dollar";
            }
            return "";
        }
    }
}
