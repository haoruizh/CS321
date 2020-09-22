namespace SpreadSheet_LogicEngine
{
    using CS321;

    /// <summary>
    /// Constant numerical node.
    /// </summary>
    public class ConstantNumbericalNode : BaseNode
    {
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNumbericalNode"/> class.
        /// Constructor of constantNumbericalNode.
        /// </summary>
        /// <param name="value">passed in value.</param>
        public ConstantNumbericalNode(double value)
            : base()
        {
            this.value = value;
        }

        /// <summary>
        /// Gets or sets value.
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
