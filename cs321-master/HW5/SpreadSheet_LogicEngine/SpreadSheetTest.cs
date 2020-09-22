using NUnit.Framework;

namespace SpreadSheet_LogicEngine
{
    class SpreadSheetTest
    {
        /// <summary>
        /// Setup method for testing class
        /// </summary>
        [SetUp]
        public void Setup()
        {}

        /// <summary>
        /// Test method for ColumCount method in Spreadsheet class
        /// </summary>
        [Test]
        public void ColNumTest()
        {
            // Normal Case
            Spreadsheet normalTest = new Spreadsheet(10, 10);
            Assert.AreEqual(10, normalTest.ColumnCount());
            normalTest = new Spreadsheet(24, 20);
            Assert.AreEqual(20, normalTest.ColumnCount());
            // Larger than 26
            Spreadsheet tooBigTest = new Spreadsheet(10, 30);
            Assert.AreEqual(26, tooBigTest.ColumnCount());
            // Negative and zero col test
            Spreadsheet negativeTest = new Spreadsheet(10, -10);
            Assert.AreEqual(0, negativeTest.ColumnCount());
            Spreadsheet zeroTest = new Spreadsheet(10, 0);
            Assert.AreEqual(0, negativeTest.ColumnCount());

        }

        /// <summary>
        /// Test method for RowCount method in SpreadSheet class
        /// </summary>
        [Test]
        public void RowCountTest()
        {
            // Normal Case
            Spreadsheet normalTest = new Spreadsheet(10, 10);
            Assert.AreEqual(10, normalTest.RowCount());
            normalTest = new Spreadsheet(50, 10);
            Assert.AreEqual(50, normalTest.RowCount());
            // Larger than 50
            Spreadsheet tooBigTest = new Spreadsheet(70, 10);
            Assert.AreEqual(50, tooBigTest.RowCount());
            // Negative and zero col test
            Spreadsheet negativeTest = new Spreadsheet(-10, 10);
            Assert.AreEqual(0, negativeTest.RowCount());
            Spreadsheet zeroTest = new Spreadsheet(0, 10);
            Assert.AreEqual(0, negativeTest.RowCount());
        }

        /// <summary>
        /// Getcell test.
        /// </summary>
        [Test]
        public void GetCellTest()
        {
            // Normal case test
            Spreadsheet normalTest = new Spreadsheet(10,10);
            Assert.IsNotNull(normalTest.Get_Cell(1, 1));
            Assert.IsNotNull(normalTest.Get_Cell(9, 9));
            // Out of range test
            Spreadsheet outOfRangeTest = new Spreadsheet(10, 10);
            Assert.IsNull(outOfRangeTest.Get_Cell(20, 0));
            Assert.IsNull(outOfRangeTest.Get_Cell(0, 20));
        }
    }
}
