// <copyright file="Form1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Haorui_Zhang
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using NUnit.Framework.Internal.Execution;
    using SpreadSheet_LogicEngine;

    /// <summary>
    /// Form class. Generate front end.
    /// </summary>
    public partial class Form1 : Form
    {
        private Spreadsheet newSheet;
        private MainMenu mainMenu = new MainMenu();

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

            // Load the menu
            MenuItem color = this.mainMenu.MenuItems.Add("Cell");
            color.MenuItems.Add(new MenuItem("Change cell background color", new EventHandler(this.ShowColorMenu)));
            MenuItem edit = this.mainMenu.MenuItems.Add("Edit");
            edit.MenuItems.Add(new MenuItem("Redo", new EventHandler(this.HandleRedo)));
            edit.MenuItems.Add(new MenuItem("Undo", new EventHandler(this.HandleUndo)));
            this.Menu = this.mainMenu;

            // create New spreadsheet
            this.newSheet = new Spreadsheet(50, 26);
            this.dataGridView1.CellValueChanged += this.newSheet.UICellValueChanged;
            this.dataGridView1.CellBeginEdit += this.newSheet.CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.newSheet.CellEndEdit;

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

            // set all cell's background in datagrid white.
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                foreach (DataGridViewColumn col in this.dataGridView1.Columns)
                {
                    unchecked
                    {
                        this.dataGridView1[col.Index, row.Index].Style.BackColor = System.Drawing.Color.FromArgb((int)0xFFFFFFFF);
                    }
                }
            }
        }

        /// <summary>
        /// Event handler when color button is clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ShowColorMenu(object sender, EventArgs e)
        {
            ColorDialog cellColor = new ColorDialog();
            cellColor.AllowFullOpen = false;
            this.UpdateCellColor(cellColor);
        }

        private void UpdateCellColor(ColorDialog cellColor)
        {
            if (cellColor.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewCell i in this.dataGridView1.SelectedCells)
                {
                    if (i.RowIndex == -1 || i.ColumnIndex == -1)
                    {
                        // if cell is out of range
                        return;
                    }

                    this.newSheet.BackColorChangeHandler(i, cellColor.Color.ToArgb());
                }
            }
        }

        /// <summary>
        /// Handle redo event in menu.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event args.</param>
        private void HandleRedo(object sender, EventArgs e)
        {
            if (this.newSheet.RedoStack.Count > 0)
            {
                this.newSheet.RedoExecute(this.dataGridView1);
            }
        }

        /// <summary>
        /// Handle undo item in menu.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event args.</param>
        private void HandleUndo(object sender, EventArgs e)
        {
            if (this.newSheet.UndoStack.Count > 0)
            {
                this.newSheet.UndoExecute(this.dataGridView1);
            }
        }

        private void UndoRedoControl(object sender, EventArgs e)
        {
            if (this.newSheet.UndoStack.Count > 0)
            {
               this.mainMenu.MenuItems[1].MenuItems[1].Enabled = true;
            }
            else
            {
                this.mainMenu.MenuItems[1].MenuItems[1].Enabled = false;
            }

            if (this.newSheet.RedoStack.Count > 0)
            {
                this.mainMenu.MenuItems[1].MenuItems[0].Enabled = true;
            }
            else
            {
                this.mainMenu.MenuItems[1].MenuItems[1].Enabled = false;
            }
        }
    }
}