namespace SpreadSheet_LogicEngine
{
    using CS321;

    /// <summary>
    /// variable node class.
    /// </summary>
    public class VariableNode : BaseNode
    {
        private string name;
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// constructor of node.
        /// </summary>
        /// <param name="value">value of node.</param>
        public VariableNode(string value)
            : base()
        {
            this.name = value;
        }

        /// <summary>
        /// Gets or sets of name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }
    }
}
