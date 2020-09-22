using System;
using System.ComponentModel;
using CS321;

namespace SpreadSheet_LogicEngine
{
    public class Spreadsheet:BaseCell
    {
        private BaseCell[,] spreadSheet;
        /// <summary>
        /// Spreadsheet constructor
        /// </summary>
        /// <param name="numOfRows"></param>
        /// <param name="numOfCols"></param>
        public Spreadsheet(int numOfRows, int numOfCols):base(numOfRows, numOfCols)
        {
            //make sure 0<=# of cols <= max of #cols
           if (numOfCols>26)
            {
                numOfCols = 26;
            }
           else if (numOfCols<0)
            {
                numOfCols = 0;
            }
           //make sure 0<=the number of rows <= max of # rows
           if(numOfRows>50)
            {
                numOfRows = 50;
            }
           else if (numOfRows<0)
            {
                numOfRows = 0;
            }
           //initili
           spreadSheet = new BaseCell[numOfRows, numOfCols];
           for(int rowIndex = 0; rowIndex<this.spreadSheet.GetLength(0); ++rowIndex)
            {
                for(int colIndex = 0; colIndex < this.spreadSheet.GetLength(1); ++colIndex)
                {
                    this.spreadSheet[rowIndex, colIndex] = new SpreadSheetCell(rowIndex, colIndex);
                    //subscribe to the cell
                    this.spreadSheet[rowIndex, colIndex].PropertyChanged += HandleCellPropertyChanged;
                }
            }
        }
       
        

        /// <summary>
        /// Cell property changed event handler. change of a value of a cell.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">What is changed.</param>
        private void HandleCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BaseCell changedCell = sender as BaseCell;

            if(spreadSheet[changedCell.RowIndex, changedCell.ColumnIndex].Text[0]!='=')
            {
                string newValue = spreadSheet[changedCell.RowIndex, changedCell.ColumnIndex].Text;
                spreadSheet[changedCell.RowIndex, changedCell.ColumnIndex].ChangeValue(newValue);
            }
            else
            {
                string formula = (changedCell.Text.Substring(0)).Trim();
                int copyCellColIndex = Convert.ToInt32(changedCell.Text[1]) - 65;
                int copyCellRowIndex = Convert.ToInt32(changedCell.Text.Substring(2))-1;
                spreadSheet[changedCell.RowIndex, changedCell.ColumnIndex].ChangeValue(spreadSheet[copyCellRowIndex, copyCellColIndex].Text);
            }
            sender = spreadSheet[changedCell.RowIndex, changedCell.ColumnIndex];
        }


        /// <summary>
        /// count cols of spreadsheet
        /// </summary>
        /// <returns>the total number of columns of spreadsheet. </returns>
        public int ColumnCount()
        {

            return this.spreadSheet.GetLength(1);
            
        }

        /// <summary>
        /// count the rows of spreadsheet
        /// </summary>
        /// <returns>Total number of rows of the spreadsheet</returns>
        public int RowCount()
        {
            return this.spreadSheet.GetLength(0);
        }

        /// <summary>
        /// return the cell in the spreadsheet based on given row and col
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">col index</param>
        /// <returns></returns>
        public BaseCell Get_Cell(int row, int col)
        {
           BaseCell cellToFind = null;
            //if given row or col is out of range, return null
           if(spreadSheet.GetLength(0)<row || spreadSheet.GetLength(1)<col)
            {
                return null;
            }
            cellToFind = this.spreadSheet[row, col];
            return cellToFind;
        }

        
    }
}
