// <copyright file="ICommandInterface.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Command interface.
    /// </summary>
    public interface ICommandInterface
    {
        /// <summary>
        /// how command to execute.
        /// </summary>
        void Action();
    }
}
