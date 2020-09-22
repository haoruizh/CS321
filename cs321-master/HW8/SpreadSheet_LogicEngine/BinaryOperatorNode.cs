// <copyright file="BinaryOperatorNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using CS321;

    /// <summary>
    /// BinaryOperatorNode. Inheritance the BaseNode.
    /// </summary>
    public abstract class BinaryOperatorNode : BaseNode
    {
        /// <summary>
        /// Precedence of the operator.
        /// </summary>
        private int precedence;

        /// <summary>
        /// The char of operator.
        /// </summary>
        private char binaryOperator;

        /// <summary>
        /// Left child of the opeartor.
        /// </summary>
        private BaseNode leftChild;

        /// <summary>
        /// Right child of the operator.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// Constructor of BinaryOperatorNode.
        /// </summary>
        /// <param name="c">input op.</param>
#pragma warning disable SA1201 // Elements should appear in the correct order

        public BinaryOperatorNode(char c)
#pragma warning restore SA1201 // Elements should appear in the correct order
            : base()
        {
            this.binaryOperator = c;
            this.precedence = this.SetPrecedence(c);
            this.leftChild = this.rightChild = null;
        }
    }
}