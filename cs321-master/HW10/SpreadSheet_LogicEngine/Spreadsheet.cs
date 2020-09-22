// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Xml;
    using CS321;

    /// <summary>
    /// Spreadsheet class.
    /// </summary>
    public class Spreadsheet : BaseCell
    {
        private Stack<ICommandInterface> undoStack = new Stack<ICommandInterface>();
        private Stack<ICommandInterface> redoStack = new Stack<ICommandInterface>();

        /// <summary>
        /// The spreadsheet.
        /// </summary>
        private BaseCell[,] spreadSheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// Spreadsheet constructor.
        /// </summary>
        /// <param name="numOfRows">Total number of rows in spreadsheet.</param>
        /// <param name="numOfCols">Total number of cols in spreadsheet.</param>
        public Spreadsheet(int numOfRows, int numOfCols)
            : base(numOfRows, numOfCols)
        {
            // if given number of cols is negative
            if (numOfCols < 0)
            {
                numOfCols = 0;
            }

            // if given rows is negative
            if (numOfRows < 0)
            {
                numOfRows = 0;
            }

            this.spreadSheet = new BaseCell[numOfRows, numOfCols];

            // Initialize all spreadsheet cells.
            for (int rowIndex = 0; rowIndex < this.spreadSheet.GetLength(0); ++rowIndex)
            {
                for (int colIndex = 0; colIndex < this.spreadSheet.GetLength(1); ++colIndex)
                {
                    this.spreadSheet[rowIndex, colIndex] = new SpreadSheetCell(rowIndex, colIndex);

                    // subscribe to the cell event handler
                    this.spreadSheet[rowIndex, colIndex].PropertyChanged += this.HandleCellPropertyChanged;
                }
            }
        }

        /// <summary>
        /// count cols of spreadsheet.
        /// </summary>
        /// <returns>the total number of columns of spreadsheet. </returns>
        public int ColumnCount()
        {
            return this.spreadSheet.GetLength(1);
        }

        /// <summary>
        /// count the rows of spreadsheet.
        /// </summary>
        /// <returns>Total number of rows of the spreadsheet.</returns>
        public int RowCount()
        {
            return this.spreadSheet.GetLength(0);
        }

        /// <summary>
        /// return the cell in the spreadsheet based on given row and col.
        /// </summary>
        /// <param name="row">row index.</param>
        /// <param name="col">col index.</param>
        /// <returns>the spreadsheet cell has coresponding row and col.</returns>
        public BaseCell Get_Cell(int row, int col)
        {
            BaseCell cellToFind = null;

            if (this.spreadSheet.GetLength(0) < row || this.spreadSheet.GetLength(1) < col)
            {
                // if given row or col is out of range
                throw new System.ArgumentException("Input out of range");
            }

            // find the cell in the spreadsheet
            cellToFind = this.spreadSheet[row, col];
            return cellToFind;
        }

        /// <summary>
        /// Handle UICell value change event.
        /// </summary>
        /// <param name="newText">Given new text.</param>
        /// <param name="row">row num.</param>
        /// <param name="col">col num.</param>
        public void UICellValueChanged(string newText, int row, int col)
        {
            if (row == -1 || col == -1)
            {
                // if given cell is out of range
                return;
            }

            this.spreadSheet[row, col].Text = newText;
        }

        /// <summary>
        /// Add undo command to undo stack.
        /// </summary>
        /// <param name="row">row number of cell.</param>
        /// <param name="col">col number of cell.</param>
        /// <param name="commandType">command type of command.</param>
        /// <param name="content">the content of command.</param>
        public void AddUndo(int row, int col, CommandType commandType, string content)
        {
            ICommandInterface newCommand = new UndoCommand(commandType, this.spreadSheet[row, col], content);
            this.undoStack.Push((UndoCommand)newCommand);
        }

        /// <summary>
        /// event handler for cell back color change.
        /// </summary>
        /// <param name="newColorCode">int code for new color.</param>
        /// <param name="row">cell row num.</param>
        /// <param name="col">cell col num.</param>
        public void BackColorChangeHandler(int newColorCode, int row, int col)
        {
            if (row < 0 || col < 0)
            {
                return;
            }

            // change cell's color to given color
            this.spreadSheet[row, col].Color = newColorCode;
        }

        /// <summary>
        /// Do redo function.
        /// </summary>
        public void SpreadSheetRedo()
        {
            ICommandInterface command = this.redoStack.Pop();
            RedoCommand newCommand = (RedoCommand)command;

            if (newCommand.CommandType == CommandType.Text)
            {
                ICommandInterface newUndoCommand = new UndoCommand(newCommand.CommandType, newCommand.Cell, newCommand.Cell.Text);
                this.undoStack.Push(newUndoCommand);
            }
            else if (newCommand.CommandType == CommandType.Color)
            {
                ICommandInterface newUndoCommand = new UndoCommand(newCommand.CommandType, newCommand.Cell, newCommand.Cell.Color.ToString());
                this.undoStack.Push(newUndoCommand);
            }

            command.Action();
        }

        /// <summary>
        /// Spreadsheet undo execute.
        /// </summary>
        public void SpreadSheetUndo()
        {
            ICommandInterface command = this.undoStack.Pop();
            UndoCommand newCommand = command as UndoCommand;

            if (newCommand.CommandType == CommandType.Text)
            {
                ICommandInterface newRedoCommand = new RedoCommand(newCommand.CommandType, newCommand.Cell, newCommand.Cell.Text);
                this.redoStack.Push(newRedoCommand);
            }
            else if (newCommand.CommandType == CommandType.Color)
            {
                ICommandInterface newRedoCommand = new RedoCommand(newCommand.CommandType, newCommand.Cell, newCommand.Cell.Color.ToString());
                this.redoStack.Push(newRedoCommand);
            }

            command.Action();
        }

        /// <summary>
        /// Saving the spreadsheet content to the given stream in xml.
        /// </summary>
        /// <param name="stream">File location where file is saved.</param>
        public void Saving(Stream stream)
        {
            XmlWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.WriteStartDocument();
            writer.WriteStartElement("spreadsheet");
            for (int row = 0; row < this.spreadSheet.GetLength(0); ++row)
            {
                for (int col = 0; col < this.spreadSheet.GetLength(1); ++col)
                {
                    if (!string.IsNullOrEmpty(this.spreadSheet[row, col].Text))
                    {
                        BaseCell curCell = this.spreadSheet[row, col];
                        string cellIndex = Convert.ToChar(this.spreadSheet[row, col].ColumnIndex + 65) + Convert.ToString(this.spreadSheet[row, col].RowIndex + 1);
                        writer.WriteStartElement("cell", cellIndex);
                        writer.WriteStartElement("bgColor");
                        writer.WriteString(curCell.Color.ToString());
                        writer.WriteEndElement(); // bgcolor
                        writer.WriteStartElement("text");
                        writer.WriteString(curCell.Text);
                        writer.WriteEndElement(); // text
                        writer.WriteEndElement(); // cell
                    }
                }
            }

            writer.WriteEndElement(); // spreadsheet
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// Clear the spreadsheet.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < this.spreadSheet.GetLength(0); ++i)
            {
                for (int j = 0; j < this.spreadSheet.GetLength(1); ++j)
                {
                    this.spreadSheet[i, j].Text = string.Empty;
                    this.spreadSheet[i, j].Color = 0;
                }
            }
        }

        /// <summary>
        /// Function for outside class to pop undo stack.
        /// </summary>
        public void UndostackPop()
        {
            this.undoStack.Pop();
        }

        /// <summary>
        /// Function for outside class to clear undo stack.
        /// </summary>
        public void UndoStackClear()
        {
            this.undoStack.Clear();
        }

        /// <summary>
        /// Function for outside class to clear redo stack.
        /// </summary>
        public void RedoStackClear()
        {
            this.redoStack.Clear();
        }

        /// <summary>
        /// Function for outside to count undo stack.
        /// </summary>
        /// <returns>the count of undo stack.</returns>
        public int CountUndoStack()
        {
            return this.undoStack.Count;
        }

        /// <summary>
        /// Function for outside class to count redo stack.
        /// </summary>
        /// <returns>the count of redo stack.</returns>
        public int CountRedoStack()
        {
            return this.redoStack.Count;
        }

        /// <summary>
        /// Function for outside class to peek redo stack.
        /// </summary>
        /// <returns>the peek of redo stack.</returns>
        public ICommandInterface RedoStackPeek()
        {
            return this.redoStack.Peek();
        }

        /// <summary>
        /// Function for outside class to peek undo stack.
        /// </summary>
        /// <returns>the peek of undo stack.</returns>
        public ICommandInterface UndoStackPeek()
        {
            return this.undoStack.Peek();
        }

        /// <summary>
        /// Cell color change handler.Triggered by cell property change.
        /// </summary>
        /// <param name="sender">sender.</param>
        private void ColorChangeHandler(object sender)
        {
            BaseCell changedCell = sender as BaseCell;

            // the corresponding cell of changed cell in spreadsheet.
            if (changedCell.Color == 0)
            {
                unchecked
                {
                    this.spreadSheet[changedCell.RowIndex, changedCell.ColumnIndex].Color = (int)0xFFFFFFFF;
                }
            }
            else
            {
                this.spreadSheet[changedCell.RowIndex, changedCell.ColumnIndex].Color = changedCell.Color;
            }
        }

        /// <summary>
        /// Get Variable names from given expression.
        /// </summary>
        /// <param name="expression">Given expression.</param>
        /// <returns>Queue with variable name.</returns>
        private Queue<string> GetVarNames(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                // if the given expression is empty or null
                throw new System.ArgumentNullException("expression", "Invalid empty argument");
            }

            Queue<string> variableQueue = new Queue<string>();
            string currentPart = string.Empty;

            for (int i = 0; i < expression.Length; ++i)
            {
                if (char.IsLetterOrDigit(expression[i]))
                {
                    // If current reading is a letter or digit
                    currentPart += expression[i];
                }
                else
                {
                    // read to the operator
                    if (char.IsLetter(currentPart[0]))
                    {
                        // if the currentPart is a variable
                        variableQueue.Enqueue(currentPart);
                    }

                    currentPart = string.Empty;
                }
            }

            // handle the end part
            if (char.IsLetter(currentPart[0]))
            {
                // if the end part is a valid variable name
                variableQueue.Enqueue(currentPart);
            }

            return variableQueue;
        }

        /// <summary>
        /// Event handler for cell text property change.
        /// </summary>
        /// <param name="sender">The cell that been changed.</param>
        private void TextChangeHandler(object sender)
        {
            // cell that been changed in UI.
            BaseCell spreadSheetCell = sender as BaseCell;

            // the spreadsheet cell that corresponds to the changed cell.
            string newFormula = spreadSheetCell.Text;
            if (!string.IsNullOrEmpty(newFormula))
            {
                newFormula = Regex.Replace(newFormula, @"s", string.Empty);
            }

            ExpressionTree expTree;
            if (string.IsNullOrEmpty(newFormula))
            {
                spreadSheetCell.ChangeValue(string.Empty);
            }
            else if (newFormula[0] != '=')
            {
                // if the new formula is a  value
                // change the cell value to changed cell text.
                double number;
                string newValue = string.Empty;
                if (double.TryParse(newFormula, out number))
                {
                    // if the new value is a double
                    newValue = newFormula;
                }
                else
                {
                    // if the new value is not a double
                    newValue = "0";
                }

                spreadSheetCell.ChangeValue(newValue);
            }
            else
            {
                // if the new formula is an expression.
                // pass the new formula into the expression tree to evaluate it
                string expression = newFormula.Substring(1);

                // if the text reference to cell itself, throw an exception and set text to "circular Reference".
                string spreadSheetCellIndex = Convert.ToChar(spreadSheetCell.ColumnIndex + 65) + (spreadSheetCell.RowIndex + 1).ToString();

                if (expression.Contains(spreadSheetCellIndex))
                {
                    spreadSheetCell.Text = "CircularRef";
                    throw new System.ArgumentException("Circular reference occurs", "text");
                }

                Queue<string> varQueue = this.GetVarNames(expression);
                expTree = new ExpressionTree(expression);
                this.UpdateExpTree(spreadSheetCell, expTree, varQueue);

                // change cell value to the evaluated result of given expression.
                spreadSheetCell.ChangeValue(expTree.Evaluate().ToString());
            }

            // update cells that subscribe to this cell in spreadsheet
            this.UpdateSubCells(spreadSheetCell);
        }

        /// <summary>
        /// Update the subscribe cells to given spread sheet cell.
        /// </summary>
        /// <param name="spreadSheetCell">given spreadsheet cell.</param>
        private void UpdateSubCells(BaseCell spreadSheetCell)
        {
            for (int i = 0; i < spreadSheetCell.Subscriber.Count; ++i)
            {
                string curSubCellIndex = spreadSheetCell.Subscriber[i];
                int col = Convert.ToInt32(curSubCellIndex[0]) - 65;
                int row = Convert.ToInt32(curSubCellIndex.Substring(1)) - 1;
                BaseCell curSubCell = this.spreadSheet[row, col];
                string exp = curSubCell.Text.Substring(1);
                Queue<string> varQueue = this.GetVarNames(exp);
                ExpressionTree expressionTree = new ExpressionTree(exp);
                this.UpdateExpTree(curSubCell, expressionTree, varQueue);
                curSubCell.ChangeValue(expressionTree.Evaluate().ToString());
            }
        }

        /// <summary>
        /// Event handler for cell property change.
        /// </summary>
        /// <param name="sender">the spreadsheet cell that has changed.</param>
        /// <param name="e">The property of the cell that is changed.</param>
        private void HandleCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                this.TextChangeHandler(sender);
            }
            else if (e.PropertyName == "Color")
            {
                this.ColorChangeHandler(sender);
            }
        }

        /// <summary>
        /// Update expression tree.
        /// </summary>
        /// <param name="spreadSheetCell">Cell that is changed.</param>
        /// <param name="expTree">Expression tree.</param>
        /// <param name="varQueue">Variable queue.</param>
        private void UpdateExpTree(BaseCell spreadSheetCell, ExpressionTree expTree, Queue<string> varQueue)
        {
            // get the variable name and corresponding value in the spreadsheet
            while (varQueue.Count > 0)
            {
                string curVar = varQueue.Dequeue();
                int col = Convert.ToInt32(curVar[0]) - 65;
                int row = Convert.ToInt32(curVar.Substring(1)) - 1;
                string sheetValue = this.spreadSheet[row, col].Value;

                // Throw an exception when corresponding cell value is null
                if (string.IsNullOrEmpty(sheetValue))
                {
                    throw new System.ArgumentNullException("expression", "Invalid empty argument");
                }

                // add the spreadsheet cell to curVar subscriber list
                string sheetIndex = Convert.ToChar(spreadSheetCell.ColumnIndex + 65) + Convert.ToString(spreadSheetCell.RowIndex + 1);
                if (!this.spreadSheet[row, col].Subscriber.Contains(sheetIndex) && curVar != sheetIndex)
                {
                    // if circular reference occurs.
                    if (this.spreadSheet[row, col].Text.Contains(sheetIndex))
                    {
                        spreadSheetCell.Text = "CurcularRef";
                        throw new System.ArgumentException("Circular reference occurs", "text");
                    }

                    this.spreadSheet[row, col].Subscriber.Add(sheetIndex);
                }

                double cellValue = Convert.ToDouble(this.spreadSheet[row, col].Value);
                expTree.SetVariable(curVar, cellValue);
            }
        }

        /// <summary>
        /// Update expression tree.
        /// </summary>
        /// <param name="spreadSheetCell">The cell that been changed.</param>
        /// <param name="expTree">Expression tree.</param>
        /// <param name="col">col of the cell that spreadsheetcell subscribed to.</param>
        /// <param name="row">row of the cell that spreadsheetcell subscribed to.</param>
        /// <param name="varQueue">Variable queue.</param>
        private void UpdateExpTree(BaseCell spreadSheetCell, ExpressionTree expTree, ref int col, ref int row, Queue<string> varQueue)
        {
            while (varQueue.Count > 0)
            {
                // Get spreadsheet cell value.
                string curVar = varQueue.Dequeue();
                col = Convert.ToInt32(curVar[0]) - 65;
                row = Convert.ToInt32(curVar.Substring(1)) - 1;
                string sheetValue = this.spreadSheet[row, col].Value;

                // Throw an exception when corresponding cell value is null
                if (string.IsNullOrEmpty(sheetValue))
                {
                    throw new System.ArgumentNullException("expression", "Invalid empty argument");
                }

                // add the spreadsheet cell to curVar subscriber list
                string sheetIndex = Convert.ToChar(spreadSheetCell.ColumnIndex + 65) + Convert.ToString(spreadSheetCell.RowIndex + 1);
                if (!this.spreadSheet[row, col].Subscriber.Contains(sheetIndex) && curVar != sheetIndex)
                {
                    this.spreadSheet[row, col].Subscriber.Add(sheetIndex);
                }

                double cellValue = Convert.ToDouble(this.spreadSheet[row, col].Value);
                expTree.SetVariable(curVar, cellValue);
            }
        }
    }
}