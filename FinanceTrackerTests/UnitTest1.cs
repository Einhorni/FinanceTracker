using MoneyManagement.DataAccess;
using MoneyManagement;
using MoneyManagement.Models;
using System.Xml.Linq;

namespace FinanceTrackerTests
{

    public class UnitTest1
    {
        //EF + Repp Kram muss man nicht testen

        //Mappings testen
        //BusinessZeug


        //    Girokonto giro1 = new Girokonto("testacc1", 100.7m, MockCurrency.EUR, Guid.NewGuid(), DateTime.Now, 0.0m);
        //    Girokonto giro2 = new Girokonto("testacc2", 10.765m, MockCurrency.EUR, Guid.NewGuid(), DateTime.Now, 10.0m);
        //    Girokonto giro3 = new Girokonto("testacc3", 0.0m, MockCurrency.EUR, Guid.NewGuid(), DateTime.Now, 0.05m);
        //    Girokonto giro4 = new Girokonto("testacc4", -50.0m, MockCurrency.EUR, Guid.NewGuid(), DateTime.Now, 0.0m);
        //    Girokonto giro5 = new Girokonto("testacc4", 50.0m, MockCurrency.EUR, Guid.NewGuid(), DateTime.Now, 0.0m);
        //    Girokonto giro6 = new Girokonto("testacc5", 50.0m, MockCurrency.Dollar, Guid.NewGuid(), DateTime.Now, 0.0m);
        //    Girokonto giro7 = new Girokonto("testacc6", 50.0m, MockCurrency.ETF, Guid.NewGuid(), DateTime.Now, 0.0m);
        //    Girokonto giro8 = new Girokonto("testacc7", 50.0m, MockCurrency.Bitcoin, Guid.NewGuid(), DateTime.Now, 0.0m);
        //    //Bar accs


        //    MoneyManagementFileService accService = new MoneyManagementService(new FileAccountRepository());
        //    MoneyManagementFileService transactionService = new MoneyManagementService(new FileTransactionRepository());

        //??
        [Fact]
        ////objekte kann ich nicht eingeben, nur primitive Typen
        //[InlineData(13.0m, 100.83m)]
        //[InlineData(46.865m, 147.565m)]
        //[InlineData(-12.6m, 113.3m)]
        public void Test1()
        {
            //Arrange
            Account acc = new Girokonto("name", 70.99m, "Euro", Guid.NewGuid(), DateTime.Now);
            //Act
            acc.ChangeAmount(10.0m);
            //Assert
            Assert.Equal(80.99m, acc.Balance);
        }



        ////ein Testcode mit unterschiedlichen Daten
        //[Theory]
        ////objekte kann ich nicht eingeben, nur primitive Typen
        //[InlineData(1, 3, 4)]
        //[InlineData(4, 2, 6)]
        //[InlineData(1, 8, 9)]
        //[InlineData(10, 10, 20)]
        //public void Test(int x, int y, int exVal)

        //{
        //    { Assert.Equal(x, y); }
        //    var zahl = x - y;
        //    Assert.Equal(zahl, exVal);
        //}
    }
}