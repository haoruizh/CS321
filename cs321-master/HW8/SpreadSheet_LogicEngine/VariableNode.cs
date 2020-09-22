// <copyright file="VariableNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using CS321;

    /// <summary>
    /// Variable node class.
    /// </summary>
    public class VariableNode : BaseNode
    {
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// constructor of variable node.
        /// </summary>
        /// <param name="value">variable name.</param>
        public VariableNode(string value)
            : base()
        {
            this.name = value;
        }

        /// <summary>
        /// gets or sets the variable of node.
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