// <copyright file="ExpressionTreeTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using CS321;
    using NUnit.Framework;

    /// <summary>
    /// Test class for expresion tree.
    /// </summary>
    public class ExpressionTreeTest
    {
        /// <summary>
        /// Setup method.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
        }

        /// <summary>
        /// Test the simplified tree.
        /// </summary>
        [Test]
        public void SimplifiedTreeTest()
        {
            // normal test
            ExpressionTree setVariableTestTree = new ExpressionTree("A1+B1+C1");
            setVariableTestTree.SetVariable("A1", 1.0);
            setVariableTestTree.SetVariable("B1", 2.0);
            setVariableTestTree.SetVariable("C1", 3.0);
            Assert.AreEqual(6.0, setVariableTestTree.Evaluate());
            setVariableTestTree = new ExpressionTree("A3+B3+C3");
            Assert.AreEqual(0.0, setVariableTestTree.Evaluate());
            setVariableTestTree = new ExpressionTree("A1-B1-C1");
            setVariableTestTree.SetVariable("A1", 1.0);
            setVariableTestTree.SetVariable("B1", 2.0);
            setVariableTestTree.SetVariable("C1", 3.0);
            Assert.AreEqual(-4.0, setVariableTestTree.Evaluate());

            // Variable not in expression
            setVariableTestTree.SetVariable("A2", 1.0);
            Assert.AreEqual(-4.0, setVariableTestTree.Evaluate());

            // Varible and numerical value
            setVariableTestTree.Expression = "A1+B1+10";
            Assert.AreEqual(13.0, setVariableTestTree.Evaluate());
        }

        /// <summary>
        /// Test the normal tree.
        /// </summary>
        [Test]
        public void NormalTreeTest()
        {
            ExpressionTree setVariableTestTree = new ExpressionTree("A1+B1*C1");
            setVariableTestTree.SetVariable("A1", 1.0);
            setVariableTestTree.SetVariable("B1", 2.0);
            setVariableTestTree.SetVariable("C1", 3.0);
            Assert.AreEqual(7.0, setVariableTestTree.Evaluate());
            setVariableTestTree.Expression = "(A1+B1)*C1";
            Assert.AreEqual(9.0, setVariableTestTree.Evaluate());
            setVariableTestTree.Expression = "(A1/B1)*C1";
            Assert.AreEqual(1.5, setVariableTestTree.Evaluate());
            setVariableTestTree.Expression = "(A1/B1)*10";
            Assert.AreEqual(5, setVariableTestTree.Evaluate());
        }
    }
}