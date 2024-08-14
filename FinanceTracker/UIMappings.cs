using MoneyManagement.Models;

namespace FinanceTracker.UIMappings
{
    internal class UIMappings
    {
        public static string MapToCurrencyString(string currencyString) // CodeReview: input werte ggf normalisieren
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
