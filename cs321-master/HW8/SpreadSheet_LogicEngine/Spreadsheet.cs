// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
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
            // if given num of cols is negative
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
        /// cell begin editing event handler.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">data grid view cell that start editing.</param>
        public void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView editingDataGridView = sender as DataGridView;
            editingDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.spreadSheet[e.RowIndex, e.ColumnIndex].Text;
        }

        /// <summary>
        /// Cell finish editing handler.
        /// </summary>
        /// <param name="sender">sender of object.</param>
        /// <param name="e">data grid view cell that finish editing.</param>
        public void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView editedDataGridView = sender as DataGridView;
            BaseCell spreadSheetCell = this.spreadSheet[e.RowIndex, e.ColumnIndex];

            //// update spreadsheetcell in UI
            string origionalText = spreadSheetCell.Text;
            editedDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = spreadSheetCell.Value;
            this.spreadSheet[e.RowIndex, e.ColumnIndex].Text = origionalText;

            // update subscriber cells to this spreadsheetcell in UI
            this.UpdateSubCells(editedDataGridView, spreadSheetCell);
        }

        /// <summary>
        /// Cell property changed event handler. change of a value of a cell.
        /// </summary>
        /// <param name="sender">Sender of event.</param>
        /// <param name="e">What is changed.</param>
        public void UICellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
            {
                // if given cell is out of range
                return;
            }

            DataGridView changedDataGridView = sender as DataGridView;
            DataGridViewCell newCell = changedDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            BaseCell spreadSheetCell = this.spreadSheet[e.RowIndex, e.ColumnIndex];

            ICommandInterface newCommand;

            // push new command to undo stack for latter call
            if (spreadSheetCell.Text == null)
            {
                // if the content of the cell is empty.
                newCommand = new UndoCommand(CommandType.Text, spreadSheetCell, string.Empty);
            }
            else
            {
                newCommand = new UndoCommand(CommandType.Text, spreadSheetCell, spreadSheetCell.Text);
            }

            this.undoStack.Push(newCommand);

            // change text to new cell value.
            spreadSheetCell.Text = newCell.Value.ToString();
        }

        /// <summary>
        /// event handler for cell back color change.
        /// </summary>
        /// <param name="dataGridViewCell">datagridview cell.</param>
        /// <param name="newColorCode">new color code.</param>
        public void BackColorChangeHandler(DataGridViewCell dataGridViewCell, int newColorCode)
        {
            // create new command and push it
            ICommandInterface newCommand = new UndoCommand(CommandType.Color, this.spreadSheet[dataGridViewCell.RowIndex, dataGridViewCell.ColumnIndex], this.spreadSheet[dataGridViewCell.RowIndex, dataGridViewCell.ColumnIndex].Color.ToString());
            this.undoStack.Push(newCommand);

            // change cell's color to given color
            this.spreadSheet[dataGridViewCell.RowIndex, dataGridViewCell.ColumnIndex].Color = newColorCode;
            dataGridViewCell.Style.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(this.spreadSheet[dataGridViewCell.RowIndex, dataGridViewCell.ColumnIndex].Color));
        }

        /// <summary>
        /// execute redo command.
        /// </summary>
        /// <param name="dataGridView">datagridview that trigger redo event.</param>
        public void RedoExecute(DataGridView dataGridView)
        {
            // execute command in the redo stack.
            RedoCommand newCommand = this.SpreadSheetRedo();

            // update data grid view cell.
            if (newCommand.CommandType == CommandType.Text)
            {
                dataGridView.Rows[newCommand.Cell.RowIndex].Cells[newCommand.Cell.ColumnIndex].Value = this.spreadSheet[newCommand.Cell.RowIndex, newCommand.Cell.ColumnIndex].Text;
                this.undoStack.Pop();
            }
            else if (newCommand.CommandType == CommandType.Color)
            {
                dataGridView.Rows[newCommand.Cell.RowIndex].Cells[newCommand.Cell.ColumnIndex].Style.BackColor = System.Drawing.Color.FromArgb(this.spreadSheet[newCommand.Cell.RowIndex, newCommand.Cell.ColumnIndex].Color);
            }
        }

        private RedoCommand SpreadSheetRedo()
        {
            ICommandInterface command = this.redoStack.Pop();
            RedoCommand newCommand = command as RedoCommand;

            if (newCommand.CommandType == CommandType.Text)
            {
                ICommandInterface newUndoCommand = new UndoCommand(newCommand.CommandType, newCommand.Cell, newCommand.Cell.Text);
                this.UndoStack.Push(newUndoCommand);
            }
            else if (newCommand.CommandType == CommandType.Color)
            {
                ICommandInterface newUndoCommand = new UndoCommand(newCommand.CommandType, newCommand.Cell, newCommand.Cell.Color.ToString());
                this.undoStack.Push(newUndoCommand);
            }

            command.Action();
            return newCommand;
        }

        /// <summary>
        /// Undo execution event.
        /// </summary>
        public void UndoExecute(DataGridView dataGridView)
        {
            // execute command in the undo stack
            UndoCommand newCommand = this.SpreadSheetUndo();

            // update data grid view cell
            if (newCommand.CommandType == CommandType.Text)
            {
                dataGridView.Rows[newCommand.Cell.RowIndex].Cells[newCommand.Cell.ColumnIndex].Value = this.spreadSheet[newCommand.Cell.RowIndex, newCommand.Cell.ColumnIndex].Text;
                this.undoStack.Pop();
            }
            else if (newCommand.CommandType == CommandType.Color)
            {
                dataGridView.Rows[newCommand.Cell.RowIndex].Cells[newCommand.Cell.ColumnIndex].Style.BackColor = System.Drawing.Color.FromArgb(this.spreadSheet[newCommand.Cell.RowIndex, newCommand.Cell.ColumnIndex].Color);
            }
        }

        /// <summary>
        /// Cell color change handler.Triggered by property change.
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
        /// Event handler for text property change.
        /// </summary>
        /// <param name="sender">The cell that been changed.</param>
        private void TextChangeHandler(object sender)
        {
            // cell that been changed in UI.
            BaseCell spreadSheetCell = sender as BaseCell;

            // the spreadsheet cell that corresponds to the changed cell.
            string newFormula = spreadSheetCell.Text;
            newFormula = Regex.Replace(newFormula, @"s", string.Empty);

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

        private void UpdateSubCells(DataGridView editedDataGridView, BaseCell spreadSheetCell)
        {
            for (int i = 0; i < spreadSheetCell.Subscriber.Count; ++i)
            {
                string curSubCellIndex = spreadSheetCell.Subscriber[i];
                int col = Convert.ToInt32(curSubCellIndex[0]) - 65;
                int row = Convert.ToInt32(curSubCellIndex.Substring(1)) - 1;
                string origText = this.spreadSheet[row, col].Text;
                editedDataGridView.Rows[row].Cells[col].Value = this.spreadSheet[row, col].Value;
                this.spreadSheet[row, col].Text = origText;
            }
        }

        private UndoCommand SpreadSheetUndo()
        {
            ICommandInterface command = this.undoStack.Pop();
            UndoCommand newCommand = command as UndoCommand;
            while (newCommand.Content == null)
            {
                command = this.undoStack.Pop();
                newCommand = command as UndoCommand;
            }

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
            return newCommand;
        }

        /// <summary>
        /// Gets undoStack.
        /// </summary>
        public Stack<ICommandInterface> UndoStack
        {
            get
            {
                return this.undoStack;
            }
        }

        /// <summary>
        /// Gets redoStack.
        /// </summary>
        public Stack<ICommandInterface> RedoStack
        {
            get
            {
                return this.redoStack;
            }
        }
    }
}