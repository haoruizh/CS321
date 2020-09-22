// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet_LogicEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using CS321;

    /// <summary>
    /// Spreadsheet class.
    /// </summary>
    public class Spreadsheet : BaseCell
    {
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
        /// Cell property changed event handler. change of a value of a cell.
        /// </summary>
        /// <param name="sender">Sender of event.</param>
        /// <param name="e">What is changed.</param>
        private void HandleCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // cell that been changed in UI.
            BaseCell uICell = sender as BaseCell;

            // the spreadsheet cell that corresponds to the changed cell.
            BaseCell spreadSheetCell = this.spreadSheet[uICell.RowIndex, uICell.ColumnIndex];
            string newFormula = spreadSheetCell.Text;
            ExpressionTree expTree;

            // remove all whitespace in the input formula:
            newFormula = Regex.Replace(newFormula, @"s", string.Empty);
            if (newFormula[0] != '=')
            {
                // if the newformula is a  value
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

                // refresh subscriber cells
                for (int i = 0; i < spreadSheetCell.Subscriber.Count; ++i)
                {
                    string curSubCellIndex = spreadSheetCell.Subscriber[i];
                    int col = Convert.ToInt32(curSubCellIndex[0]) - 65;
                    int row = Convert.ToInt32(curSubCellIndex.Substring(1)) - 1;
                    BaseCell curSubscriber = this.spreadSheet[row, col];
                    string spreadSheetCellIndex = Convert.ToChar(spreadSheetCell.ColumnIndex + 65) + Convert.ToString(spreadSheetCell.RowIndex + 1);

                    if (curSubscriber.Text.Contains(spreadSheetCellIndex))
                    {
                        // if spread sheet cell is part of subscriber expression.
                        Queue<string> varQueue = this.GetVarNames(curSubscriber.Text.Substring(1));
                        expTree = new ExpressionTree(curSubscriber.Text.Substring(1));

                        // get the variable name and corresponding value in the spreadsheet
                        this.UpdateExpTree(spreadSheetCell, expTree, ref col, ref row, varQueue);
                        curSubscriber.ChangeValue(expTree.Evaluate().ToString());
                    }
                }
            }
            else
            {
                // if the newformula is an expression.
                // pass the newformula into the expression tree to evaulate it
                string expression = newFormula.Substring(1);
                Queue<string> varQueue = this.GetVarNames(expression);
                expTree = new ExpressionTree(expression);
                this.UpdateExpTree(spreadSheetCell, expTree, varQueue);

                // change cell value to the evaluated result of given expression.
                spreadSheetCell.ChangeValue(expTree.Evaluate().ToString());
            }
        }

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

                // add the spreadsheetcell to curVar subscriber list
                string sheetIndex = Convert.ToChar(spreadSheetCell.ColumnIndex + 65) + Convert.ToString(spreadSheetCell.RowIndex + 1);
                if (!this.spreadSheet[row, col].Subscriber.Contains(sheetIndex) && curVar != sheetIndex)
                {
                    this.spreadSheet[row, col].Subscriber.Add(sheetIndex);
                }

                double cellValue = Convert.ToDouble(this.spreadSheet[row, col].Value);
                expTree.SetVariable(curVar, cellValue);
            }
        }

        private void UpdateExpTree(BaseCell spreadSheetCell, ExpressionTree expTree, ref int col, ref int row, Queue<string> varQueue)
        {
            while (varQueue.Count > 0)
            {
                string curVar = varQueue.Dequeue();
                col = Convert.ToInt32(curVar[0]) - 65;
                row = Convert.ToInt32(curVar.Substring(1)) - 1;
                string sheetValue = this.spreadSheet[row, col].Value;

                // Throw an exception when corresponding cell value is null
                if (string.IsNullOrEmpty(sheetValue))
                {
                    throw new System.ArgumentNullException("expression", "Invalid empty argument");
                }

                // add the spreadsheetcell to curVar subscriber list
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