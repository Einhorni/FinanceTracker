using MoneyManagement.Models;

namespace FinanceTracker.MoneyManagement
{
    internal class UIMappings
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

        public static CategoryDTO MapToCategory(string categoryString)
        {
            CategoryDTO category = CategoryDTO.Empty;
            switch (categoryString)
            {
                case "a": category = CategoryDTO.ArtSupplies; break;
                case "b": category = CategoryDTO.Books; break;
                case "f": category = CategoryDTO.Fees; break;
                case "g": category = CategoryDTO.Groceries; break;
                case "h": category = CategoryDTO.Household; break;
                case "i": category = CategoryDTO.Insurance; break;
                case "o": category = CategoryDTO.OtherHobbies; break;
                case "p": category = CategoryDTO.PersonalHealth; break;
                case "s": category = CategoryDTO.StreamAndTvAndPhone; break;
                case "t": category = CategoryDTO.Taxes; break;
                case "v": category = CategoryDTO.VehicleAndFuel; break;
                case "+": category = CategoryDTO.Income; break;
            }
            return category;
        }

        public static string MapCategoryToString(CategoryDTO category)
        {
            string categoryString = string.Empty;
            switch (category)
            {
                case CategoryDTO.ArtSupplies: categoryString = "ArtSupplies"; break;
                case CategoryDTO.Books: categoryString = "Books"; break;
                case CategoryDTO.Fees: categoryString = "Fees"; break;
                case CategoryDTO.Groceries: categoryString = "Groceries"; break;
                case CategoryDTO.Household: categoryString = "Household"; break;
                case CategoryDTO.Insurance: categoryString = "Insurance"; break;
                case CategoryDTO.OtherHobbies: categoryString = "Other Hobbies"; break;
                case CategoryDTO.PersonalHealth: categoryString = "Personal Health"; break;
                case CategoryDTO.StreamAndTvAndPhone: categoryString = "Strea, Tv and Phone"; break;
                case CategoryDTO.Taxes: categoryString = "Taxes"; break;
                case CategoryDTO.VehicleAndFuel: categoryString = "Vehicle and Fuel"; break;
                case CategoryDTO.Income: categoryString = "Income"; break;
            }
            return categoryString;
        }
    }
}
