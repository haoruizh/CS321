namespace SpreadSheet_LogicEngine
{
    /// <summary>
    /// Minus operator node.
    /// </summary>
    internal class Minus : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Minus"/> class.
        /// Minus constructor.
        /// </summary>
        public Minus()
            : base('-')
        {
        }

        /// <summary>
        /// Evaluate the result.
        /// </summary>
        /// <param name="left">left operand.</param>
        /// <param name="right">right operand.</param>
        /// <returns>left - right.</returns>
        public double Evaluate(BinaryOperatorNode left, BinaryOperatorNode right)
        {
            return left.Cal(left.Left, left.Right) - right.Cal(right.Left, right.Right);
        }
    }
}
