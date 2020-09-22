namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using CS321;

    /// <summary>
    /// Class for undo action.
    /// </summary>
    public class UndoCommand : ICommandInterface
    {
        private CommandType iCommandType;
        private BaseCell iCell;
        private string undoContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoCommand"/> class.
        /// </summary>
        /// <param name="spreadsheet">Reference spreadsheet.</param>
        /// <param name="commandType">What is changed on the cell.</param>
        /// <param name="cell">Cell that need to undo.</param>
        public UndoCommand(CommandType commandType, BaseCell cell, string content)
        {
            this.iCommandType = commandType;
            this.iCell = cell;
            this.undoContent = content;
        }

        /// <summary>
        /// Execute undo action for ICell.
        /// </summary>
        public void Action()
        {
            if (this.iCommandType == CommandType.Text)
            {
                // change ICell text to undoContent.
                this.iCell.Text = this.undoContent;
            }
            else if (this.iCommandType == CommandType.Color)
            {
                // change ICell color to undoContent.
                this.iCell.Color = Convert.ToInt32(this.undoContent);
            }
        }

        public CommandType CommandType
        {
            get
            {
                return this.iCommandType;
            }
        }

        public string Content
        {
            get
            {
                return this.undoContent;
            }
        }

        public BaseCell Cell
        {
            get
            {
                return this.iCell;
            }
        }
    }
}
