// <copyright file="BinaryOperatorNode.cs" company=Haorui Zhang>
// Copyright (c) Haorui Zhang. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CS321;

    /// <summary>
    /// BinaryOperatorNode. Inheritance the BaseNode.
    /// </summary>
    public abstract class BinaryOperatorNode : BaseNode
    {
        private int precedence;
        private char binaryOperator;
        private BaseNode leftChild;
        private BaseNode rightChild;

        /// <summary>
        /// Calculate the left and right based on the operator.
        /// </summary>
        /// <param name="left">Left node.</param>
        /// <param name="right"> Right node.</param>
        /// <returns>Return the result.</returns>
        public double Cal(BaseNode left, BaseNode right)
        {
            double res = 0;
            switch (this.binaryOperator)
            {
                default:
                    break;
            }

            return res;
        }

        /// <summary>
        /// Return the precedence based on input operator.
        /// </summary>
        /// <param name="c">Input character.</param>
        /// <returns>return precedence.</returns>
        private int SetPrecedence(char c)
        {
            if (c == '*' | c == '/')
            {
                return 2;
            }
            else if (c == '+' | c == '-')
            {
                return 1;
            }
            else if (c == '^')
            {
                return 3;
            }

            return -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// Constructor of BinaryOperatorNode.
        /// </summary>
        /// <param name="c">input op.</param>
        public BinaryOperatorNode(char c)
            : base()
        {
            this.binaryOperator = c;
            this.precedence = this.SetPrecedence(c);
            this.leftChild = this.rightChild = null;
        }

        /// <summary>
        /// gets precedence.
        /// </summary>
        public int Precedence
        {
            get
            {
                return this.precedence;
            }
        }

        /// <summary>
        /// gets BinaryOperator.
        /// </summary>
        public char BinaryOperator
        {
            get
            {
                return this.binaryOperator;
            }
        }

        /// <summary>
        /// Gets or sets left child.
        /// </summary>
        public BaseNode Left
        {
            get
            {
                return this.leftChild;
            }

            set
            {
                this.leftChild = value;
            }
        }

        /// <summary>
        /// gets or sets right child.
        /// </summary>
        public BaseNode Right
        {
            get
            {
                return this.rightChild;
            }

            set
            {
                this.rightChild = value;
            }
        }
    }
}
