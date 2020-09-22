// <copyright file="SpreadSheetTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using NUnit.Framework;

    /// <summary>
    /// test class for spreadsheet.
    /// </summary>
    public class SpreadSheetTest
    {
        /// <summary>
        /// Setup method for testing class.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Test method for ColumCount method in Spreadsheet class.
        /// </summary>
        [Test]
        public void ColNumTest()
        {
            // Normal Case
            Spreadsheet normalTest = new Spreadsheet(10, 10);
            Assert.AreEqual(10, normalTest.ColumnCount());
            normalTest = new Spreadsheet(24, 20);
            Assert.AreEqual(20, normalTest.ColumnCount());

            // Negative and zero col test
            Spreadsheet negativeTest = new Spreadsheet(10, -10);
            Assert.AreEqual(0, negativeTest.ColumnCount());
            Spreadsheet zeroTest = new Spreadsheet(10, 0);
            Assert.AreEqual(0, negativeTest.ColumnCount());
        }

        /// <summary>
        /// Test method for RowCount method in SpreadSheet class.
        /// </summary>
        [Test]
        public void RowCountTest()
        {
            // Normal Case
            Spreadsheet normalTest = new Spreadsheet(10, 10);
            Assert.AreEqual(10, normalTest.RowCount());
            normalTest = new Spreadsheet(50, 10);
            Assert.AreEqual(50, normalTest.RowCount());

            // Negative and zero col test
            Spreadsheet negativeTest = new Spreadsheet(-10, 10);
            Assert.AreEqual(0, negativeTest.RowCount());
            Spreadsheet zeroTest = new Spreadsheet(0, 10);
            Assert.AreEqual(0, negativeTest.RowCount());
        }

        /// <summary>
        /// get cell test for spreadsheet.
        /// </summary>
        [Test]
        public void GetCellTest()
        {
            // Normal case test
            Spreadsheet normalTest = new Spreadsheet(10, 10);
            Assert.IsNotNull(normalTest.Get_Cell(1, 1));
            Assert.IsNotNull(normalTest.Get_Cell(9, 9));

            // Out of range test
            Spreadsheet outOfRangeTest = new Spreadsheet(10, 10);
            Assert.Throws<System.ArgumentException>(() => outOfRangeTest.Get_Cell(20, 0));
            Assert.Throws<System.ArgumentException>(() => outOfRangeTest.Get_Cell(0, 20));
        }

        /// <summary>
        /// Test GetVarNames method.
        /// </summary>
        [Test]
        public void GetVarTest()
        {
            // Value test
            Spreadsheet varTest = new Spreadsheet(10, 10);
            varTest.Get_Cell(0, 0).Text = "10";
            varTest.Get_Cell(1, 1).Text = "=A1";
            Assert.AreEqual("10", varTest.Get_Cell(1, 1).Value);
        }

        /// <summary>
        /// Test evaluate functionality in spreadsheet.
        /// </summary>
        [Test]
        public void EvaExpTest()
        {
            // variable with value
            Spreadsheet expTest = new Spreadsheet(10, 10);
            expTest.Get_Cell(0, 0).Text = "10";
            expTest.Get_Cell(1, 1).Text = "=A1*2";
            Assert.AreEqual("20", expTest.Get_Cell(1, 1).Value);

            // variable with variable
            expTest.Get_Cell(0, 2).Text = "10";
            expTest.Get_Cell(1, 1).Text = "=A1*C1";
            Assert.AreEqual("100", expTest.Get_Cell(1, 1).Value);
        }

        /// <summary>
        /// Test cell update function in spreadsheet.
        /// </summary>
        [Test]
        public void ChangeTextTest()
        {
            // variable with value
            Spreadsheet expTest = new Spreadsheet(10, 10);
            expTest.Get_Cell(0, 0).Text = "10";
            expTest.Get_Cell(1, 1).Text = "=A1*2";
            expTest.Get_Cell(0, 0).Text = "20";
            Assert.AreEqual("40", expTest.Get_Cell(1, 1).Value);
        }

        /// <summary>
        /// Test cell circular reference.
        /// </summary>
        [Test]
        public void CircularRefTest()
        {
            Spreadsheet circularRefTest = new Spreadsheet(5, 5);
            Assert.Throws<System.ArgumentException>(() => circularRefTest.Get_Cell(0, 0).Text = "=A1");
            Assert.AreEqual("CircularRef", circularRefTest.Get_Cell(0, 0).Text);
            circularRefTest.Get_Cell(0, 0).Text = "10";
            circularRefTest.Get_Cell(0, 1).Text = "=A1";
            Assert.Throws<System.ArgumentException>(() => circularRefTest.Get_Cell(0, 0).Text = "=B1");
        }
    }
}