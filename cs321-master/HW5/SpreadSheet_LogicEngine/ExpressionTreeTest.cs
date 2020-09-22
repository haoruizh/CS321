using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS321;

namespace SpreadSheet_LogicEngine
{
    using NUnit.Framework;

    /// <summary>
    /// Test part for expression tree.
    /// </summary>
    public class ExpressionTreeTest
    {
        /// <summary>
        /// set up method for testing.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
        }

        /// <summary>
        /// Test part.
        /// </summary>
        [Test]
        public void TreeTest()
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

    }
}
