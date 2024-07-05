using MoneyManagement.Models;

namespace FinanceTracker.MoneyManagement
{
    internal class Mappings
    {
        public static MockCurrency MapToCurrency(string currencyString)
        {
            MockCurrency currency = new();
            switch (currencyString)
            {
                case "b":
                    currency = MockCurrency.Bitcoin;
                    break;

                case "d":
                    currency = MockCurrency.Dollar;
                    break;

                case "e":
                    currency = MockCurrency.EUR;
                    break;

                case "f":
                    currency = MockCurrency.ETF;
                    break;
            }
            return currency;
        }

        public static Category MapToCategory(string categoryString)
        {
            Category category = Category.Empty;
            switch (categoryString)
            {
                case "a": category = Category.ArtSupplies; break;
                case "b": category = Category.Books; break;
                case "f": category = Category.Fees; break;
                case "g": category = Category.Groceries; break;
                case "h": category = Category.Household; break;
                case "i": category = Category.Insurance; break;
                case "o": category = Category.OtherHobbies; break;
                case "p": category = Category.PersonalHealth; break;
                case "s": category = Category.StreamAndTvAndPhone; break;
                case "t": category = Category.Taxes; break;
                case "v": category = Category.VehicleAndFuel; break;
                case "+": category = Category.Income; break;
            }
            return category;
        }

        public static string MapCategoryToString(Category category)
        {
            string categoryString = string.Empty;
            switch (category)
            {
                case Category.ArtSupplies: categoryString = "ArtSupplies"; break;
                case Category.Books: categoryString = "Books"; break;
                case Category.Fees: categoryString = "Fees"; break;
                case Category.Groceries: categoryString = "Groceries"; break;
                case Category.Household: categoryString = "Household"; break;
                case Category.Insurance: categoryString = "Insurance"; break;
                case Category.OtherHobbies: categoryString = "Other Hobbies"; break;
                case Category.PersonalHealth: categoryString = "Personal Health"; break;
                case Category.StreamAndTvAndPhone: categoryString = "Strea, Tv and Phone"; break;
                case Category.Taxes: categoryString = "Taxes"; break;
                case Category.VehicleAndFuel: categoryString = "Vehicle and Fuel"; break;
                case Category.Income: categoryString = "Income"; break;
            }
            return categoryString;
        }
    }
}
