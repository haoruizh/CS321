namespace SpreadSheet_LogicEngine
{
    /// <summary>
    /// divide operator.
    /// </summary>
    internal class Divide : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Divide"/> class.
        /// Divide constructor.
        /// </summary>
        public Divide()
            : base('/')
        {
        }

        /// <summary>
        /// Evaluate the left and right.
        /// </summary>
        /// <param name="left">left operand.</param>
        /// <param name="right">right operand.</param>
        /// <returns>left/right.</returns>
        public double Evaluate(BinaryOperatorNode left, BinaryOperatorNode right)
        {
            return left.Cal(left.Left, left.Right) / right.Cal(right.Left, right.Right);
        }
    }
}
