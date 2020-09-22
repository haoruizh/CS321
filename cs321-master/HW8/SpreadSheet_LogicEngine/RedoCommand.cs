namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using CS321;

    /// <summary>
    /// The command for redo the action.
    /// </summary>
    public class RedoCommand : ICommandInterface
    {
        private CommandType iCommandType;
        private BaseCell iCell;
        private string redoContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedoCommand"/> class.
        /// </summary>
        /// <param name="spreadsheet">Reference spreadsheet.</param>
        /// <param name="commandType">What is changed on the cell.</param>
        /// <param name="cell">Cell that is need to redo.</param>
        public RedoCommand(CommandType commandType, BaseCell cell, string content)
        {
            this.iCommandType = commandType;
            this.iCell = cell;
            this.redoContent = content;
        }

        /// <summary>
        /// Execute command part for ICell.
        /// </summary>
        public void Action()
        {
            if (this.iCommandType == CommandType.Color)
            {
                // change ICell color to undoContent.
                this.iCell.Color = Convert.ToInt32(this.redoContent);
            }
            else if (this.iCommandType == CommandType.Text)
            {
                // change ICell text to undoContent.
                this.iCell.Text = this.redoContent;
            }
        }

        public CommandType CommandType
        {
            get
            {
                return this.iCommandType;
            }
        }

        public BaseCell Cell
        {
            get
            {
                return this.iCell;
            }
        }

        public string Content
        {
            get
            {
                return this.redoContent;
            }
        }
    }
}
