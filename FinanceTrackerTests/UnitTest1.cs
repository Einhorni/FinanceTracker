namespace FinanceTrackerTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        //ein Testcode mit unterschiedlichen Daten
        [Theory]
        //objekte kann ich nicht eingeben, nur primitive Typen
        [InlineData(1, 3, 4)]
        [InlineData(4, 2, 6)]
        [InlineData(1, 8, 9)]
        [InlineData(10, 10, 20)]
        public void Test(int x, int y, int exVal)

        {
            { Assert.Equal(x, y); }
            var zahl = x - y;
            Assert.Equal(zahl, exVal);
        }
    }
}