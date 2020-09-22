// <copyright file="Addition.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CS321;

    /// <summary>
    /// Addition operation. Inheritance from BinaryOperatorNode.
    /// </summary>
    internal class Addition : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Addition"/> class.
        /// Addition constructor.
        /// </summary>
        public Addition()
            : base('+')
        {
        }
    }
}
