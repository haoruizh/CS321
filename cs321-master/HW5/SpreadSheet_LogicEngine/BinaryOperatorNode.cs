namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CS321;

    /// <summary>
    /// operator node class.
    /// </summary>
    public class BinaryOperatorNode : BaseNode
    {
        private int precedence;
        private char binaryOperator;
        private BaseNode leftChild;
        private BaseNode rightChild;

        /// <summary>
        /// Return the precedence based on input operator.
        /// </summary>
        /// <param name="c">current character.</param>
        /// <returns>return precedence</returns>
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
        /// <param name="c">operator passed in.</param>
        public BinaryOperatorNode(char c)
            : base()
        {
            this.binaryOperator = c;
            this.precedence = this.SetPrecedence(c);
            this.leftChild = this.rightChild = null;
        }

        /// <summary>
        /// Gets get precedence.
        /// </summary>
        public int Precedence
        {
            get
            {
                return this.precedence;
            }
        }

        /// <summary>
        /// Gets get binaryoperator.
        /// </summary>
        public char BinaryOperator
        {
            get
            {
                return this.binaryOperator;
            }
        }

        /// <summary>
        /// gets or sets left child.
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
        /// Gets or sets right child.
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
