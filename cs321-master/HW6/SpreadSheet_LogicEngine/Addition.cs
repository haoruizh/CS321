namespace SpreadSheet_LogicEngine
{
    /// <summary>
    /// Addition operator node.
    /// </summary>
    internal class Addition : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Addition"/> class.
        /// Constructor of addtion node.
        /// </summary>
        public Addition()
            : base('+')
        {
        }

        /// <summary>
        /// Addition evaluate. Add two values together.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>Sum of left and right operand.</returns>
        public double Evaluate(BinaryOperatorNode left, BinaryOperatorNode right)
        {
            return left.Cal(left.Left, left.Right) + right.Cal(right.Left, right.Right);
        }
    }
}
