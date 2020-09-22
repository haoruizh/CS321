namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CS321;

    /// <summary>
    /// Divide op constructor.
    /// </summary>
    internal class Divide : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Divide"/> class.
        /// Constructor of Divide node.
        /// </summary>
        public Divide()
            : base('/')
        { }
    }
}
