// <copyright file="Form1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Haorui_Zhang
{
    using System;
    using System.Windows.Forms;
    using SpreadSheet_LogicEngine;

    /// <summary>
    /// Form class. Generate front end.
    /// </summary>
    public partial class Form1 : Form
    {
        private Spreadsheet newSheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initial the form 1.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event argument.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // create New spreadsheet
            this.newSheet = new Spreadsheet(50, 26);
            this.dataGridView1.CellValueChanged += this.UpdateSpreadSheetCell;
            this.dataGridView1.CellBeginEdit += this.DataGridView1_CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.DataGridView1_CellEndEdit;

            // clear design rows and cols
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();

            // create new cols A to Z in a loop
            string colNameStr = string.Empty;
            for (char colName = 'A'; colName <= 'Z'; colName++)
            {
                colNameStr = Convert.ToString(colName);
                this.dataGridView1.Columns.Add(colNameStr, colNameStr);
            }

            // create 50 rows from 1 to 50
            for (int index = 0; index < 50; index++)
            {
                this.dataGridView1.Rows.Add();
            }

            foreach (DataGridViewRow curRow in this.dataGridView1.Rows)
            {
                curRow.HeaderCell.Value = string.Format("{0}", curRow.Index + 1);
            }
        }

        /// <summary>
        /// Event handler for cell begin edit event.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">the cell that is editing.</param>
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell editingCell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            editingCell.Value = this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Text;
        }

        /// <summary>
        /// Event handler for cell exit editing.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">finished cell.</param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell finishedCell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string cellText = finishedCell.Value.ToString();
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Value;

            // Change the spreadsheet cell text back to what it was
            this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Text = cellText;

            // Update subscribed cell in UI
            this.UpdateSubscribedCell(e);
        }

        /// <summary>
        /// Update the spread sheet cell.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">cell changed.</param>
        private void UpdateSpreadSheetCell(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
            {
                // if given cell is out of range
                return;
            }

            string newText = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Text = newText;
        }

        /// <summary>
        /// Update subscribed cells in UI.
        /// </summary>
        /// <param name="e">the "master" cell.</param>
        private void UpdateSubscribedCell(DataGridViewCellEventArgs e)
        {
            // update each subscribed cells
            for (int i = 0; i < this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Subscriber.Count; ++i)
            {
                string curSubCellIndex = this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Subscriber[i];
                int col = Convert.ToInt32(curSubCellIndex[0]) - 65;
                int row = Convert.ToInt32(curSubCellIndex.Substring(1)) - 1;
                string subCellText = this.newSheet.Get_Cell(row, col).Text;
                DataGridViewCell curSubCell = this.dataGridView1.Rows[row].Cells[col];
                curSubCell.Value = this.newSheet.Get_Cell(row, col).Value;
                this.newSheet.Get_Cell(row, col).Text = subCellText;
            }
        }
    }
}