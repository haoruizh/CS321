namespace SpreadSheet_LogicEngine
{
    using CS321;

    /// <summary>
    /// Constant numical number node.
    /// </summary>
    public class ConstantNumbericalNode : BaseNode
    {
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNumbericalNode"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="value">Input value.</param>
        public ConstantNumbericalNode(double value)
            : base()
        {
            this.value = value;
        }

        /// <summary>
        /// gets or sets value peoperty.
        /// </summary>
        public double Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}
