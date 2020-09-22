namespace SpreadSheet_LogicEngine
{
    /// <summary>
    /// Multiply operator node.
    /// </summary>
    internal class Multiple : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Multiple"/> class.
        /// </summary>
        public Multiple()
            : base('*')
        {
        }

        /// <summary>
        /// Evaluate of the mutilply of two operands.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>left*right.</returns>
        public double Evaluate(BinaryOperatorNode left, BinaryOperatorNode right)
        {
            return left.Cal(left.Left, left.Right) * right.Cal(right.Left, right.Right);
        }
    }
}
