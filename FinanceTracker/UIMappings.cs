using MoneyManagement.Models;

namespace FinanceTracker.UIMappings
{
    internal class UIMappings
    {
        public static string MapToCurrencyString(string currencyString)
        {
            switch (currencyString)
            {
                case "e":
                    return "Euro";
                case "d":
                    return "Dollar";
            }
            return "";
        }
    }
}
