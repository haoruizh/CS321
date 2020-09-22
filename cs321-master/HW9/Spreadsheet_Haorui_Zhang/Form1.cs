// <copyright file="Form1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Haorui_Zhang
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
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
            MenuItem file = this.mainMenu.MenuItems.Add("File");
            file.MenuItems.Add(new MenuItem("Save", new EventHandler(this.SavingToFile)));
            file.MenuItems.Add(new MenuItem("Load", new EventHandler(this.LoadingFromFile)));
            edit.MenuItems.Add(new MenuItem("Redo", new EventHandler(this.HandleRedo)));
            edit.MenuItems.Add(new MenuItem("Undo", new EventHandler(this.HandleUndo)));
            this.Menu = this.mainMenu;

            // create New spreadsheet
            this.newSheet = new Spreadsheet(50, 26);
            this.dataGridView1.CellValueChanged += this.CellValueChangedHandler;
            this.dataGridView1.CellBeginEdit += this.CellBeginEditHandler;
            this.dataGridView1.CellEndEdit += this.CellEndEditHandler;

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

            // set all cell's background in data grid view white.
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
        /// Cell value change event handler.
        /// </summary>
        /// <param name="sender">datagrid view.</param>
        /// <param name="e">datagrid view cell that has changed.</param>
        private void CellValueChangedHandler(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
            {
                return;
            }

            // add new undo command
            if (string.IsNullOrEmpty(this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Text))
            {
                this.newSheet.AddUndo(e.RowIndex, e.ColumnIndex, CommandType.Text, string.Empty);
            }
            else
            {
                this.newSheet.AddUndo(e.RowIndex, e.ColumnIndex, CommandType.Text, this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Text);
            }

            string newText = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            this.newSheet.UICellValueChanged(newText, e.RowIndex, e.ColumnIndex);
        }

        /// <summary>
        /// Cell begin editing event handler.
        /// </summary>
        /// <param name="sender">Datagridview that begin editing.</param>
        /// <param name="e">data grid view cell that begin editing.</param>
        private void CellBeginEditHandler(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView UI = (DataGridView)sender;
            UI.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Text;
        }

        /// <summary>
        /// Cell end editing event handler.
        /// </summary>
        /// <param name="sender">Datagridview that end editing.</param>
        /// <param name="e">data grid view cell that end editing.</param>
        private void CellEndEditHandler(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView UI = (DataGridView)sender;
            string oldText = this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Text;
            UI.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Value;
            this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Text = oldText;

            // update sub cell
            foreach (string i in this.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Subscriber)
            {
                int col = Convert.ToInt32(i[0]) - 65;
                int row = Convert.ToInt32(i.Substring(1)) - 1;
                string oldSubCellText = this.newSheet.Get_Cell(row, col).Text;
                UI.Rows[row].Cells[col].Value = this.newSheet.Get_Cell(row, col).Value;
                this.newSheet.Get_Cell(row, col).Text = oldSubCellText;
                this.newSheet.UndostackPop();
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

        /// <summary>
        /// Update cell value.
        /// </summary>
        /// <param name="cellColor">color panel that can choose color.</param>
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

                    // add new undo command
                    this.newSheet.AddUndo(i.RowIndex, i.ColumnIndex, CommandType.Color, Convert.ToString(this.newSheet.Get_Cell(i.RowIndex, i.ColumnIndex).Color));

                    // change cell back color
                    this.newSheet.BackColorChangeHandler(cellColor.Color.ToArgb(), i.RowIndex, i.ColumnIndex);
                    this.dataGridView1[i.RowIndex, i.ColumnIndex].Style.BackColor = System.Drawing.Color.FromArgb(this.newSheet.Get_Cell(i.RowIndex, i.ColumnIndex).Color);
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
            if (this.newSheet.CountRedoStack() > 0)
            {
                // if the redo stack is not empty.
                RedoCommand newCommand = (RedoCommand)this.newSheet.RedoStackPeek();
                this.newSheet.SpreadSheetRedo();
                if (newCommand.CommandType == CommandType.Color)
                {
                    // color redo command.
                    this.dataGridView1.Rows[newCommand.Cell.RowIndex].Cells[newCommand.Cell.ColumnIndex].Style.BackColor = System.Drawing.Color.FromArgb(this.newSheet.Get_Cell(newCommand.Cell.RowIndex, newCommand.Cell.ColumnIndex).Color);
                }
                else if (newCommand.CommandType == CommandType.Text)
                {
                    // text redo command.
                    this.dataGridView1.Rows[newCommand.Cell.RowIndex].Cells[newCommand.Cell.ColumnIndex].Value = this.newSheet.Get_Cell(newCommand.Cell.RowIndex, newCommand.Cell.ColumnIndex).Value;
                }

                // pop the extra command.
                this.newSheet.UndostackPop();
            }
        }

        /// <summary>
        /// Saving the new sheet to file.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event args.</param>
        private void SavingToFile(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Open";
            saveFileDialog1.InitialDirectory = @"c:\";
            saveFileDialog1.Filter = "XML Files (*.xml)|*.xml";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = File.OpenWrite(saveFileDialog1.FileName);
                this.newSheet.Saving(fs);
            }
        }

        /// <summary>
        /// Loading the content from file.
        /// </summary>
        /// <param name="sender">Menu.</param>
        /// <param name="e">event args.</param>
        private void LoadingFromFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Open";
            openFileDialog1.InitialDirectory = @"c:\";
            openFileDialog1.Filter = "XML Files (*.xml)|*.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream sr = File.OpenRead(openFileDialog1.FileName);
                string textContent = string.Empty;
                string cellIndex = string.Empty;
                string color = string.Empty;

                // clear the spreadsheet.
                this.newSheet.Clear();
                XmlReader reader = new XmlTextReader(sr);
                XmlNodeType type;
                while (reader.Read())
                {
                    type = reader.NodeType;

                    if (type == XmlNodeType.Element)
                    {
                        if (reader.Name == "spreadsheet")
                        {
                            // cell attributes
                            int col = 0;
                            int row = 0;
                            DataGridViewCell cell;

                            while (reader.Read())
                            {
                                type = reader.NodeType;

                                if (type == XmlNodeType.Element)
                                {
                                    if (reader.Name == "cell")
                                    {
                                        reader.Read();
                                        cellIndex = reader.NamespaceURI;
                                    }

                                    if (reader.Name == "bgColor")
                                    {
                                        reader.Read();
                                        color = reader.Value;
                                    }

                                    if (reader.Name == "text")
                                    {
                                        reader.Read();
                                        textContent = reader.Value;
                                    }

                                    // update UI cell
                                    col = Convert.ToInt32(cellIndex[0]) - 65;
                                    row = Convert.ToInt32(cellIndex.Substring(1)) - 1;
                                    cell = this.dataGridView1.Rows[row].Cells[col];
                                    if (Convert.ToInt32(color) == 0)
                                    {
                                        unchecked
                                        {
                                            cell.Style.BackColor = System.Drawing.Color.FromArgb((int)0xFFFFFFFF);
                                        }
                                    }
                                    else
                                    {
                                        cell.Style.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(color));
                                    }

                                    cell.Value = textContent;
                                    if (this.newSheet.Get_Cell(row, col).Text != this.newSheet.Get_Cell(row, col).Value)
                                    {
                                        string oldText = this.newSheet.Get_Cell(row, col).Text;
                                        cell.Value = this.newSheet.Get_Cell(row, col).Value;
                                        this.newSheet.Get_Cell(row, col).Text = oldText;

                                        // update sub cell
                                        foreach (string i in this.newSheet.Get_Cell(row, col).Subscriber)
                                        {
                                            int newCol = Convert.ToInt32(i[0]) - 65;
                                            int newRow = Convert.ToInt32(i.Substring(1)) - 1;
                                            string oldSubCellText = this.newSheet.Get_Cell(row, col).Text;
                                            this.dataGridView1.Rows[newRow].Cells[newCol].Value = this.newSheet.Get_Cell(row, col).Value;
                                            this.newSheet.Get_Cell(row, col).Text = oldSubCellText;
                                            this.newSheet.UndostackPop();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                reader.Close();

                // pop out all undo stack and redo stack content
                this.newSheet.UndoStackClear();
                this.newSheet.RedoStackClear();
            }
        }

        /// <summary>
        /// Handle undo item in menu.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event args.</param>
        private void HandleUndo(object sender, EventArgs e)
        {
            if (this.newSheet.CountUndoStack() > 0)
            {
                UndoCommand newCommand = (UndoCommand)this.newSheet.UndoStackPeek();
                this.newSheet.SpreadSheetUndo();
                if (newCommand.CommandType == CommandType.Color)
                {
                    this.dataGridView1.Rows[newCommand.Cell.RowIndex].Cells[newCommand.Cell.ColumnIndex].Style.BackColor = System.Drawing.Color.FromArgb(this.newSheet.Get_Cell(newCommand.Cell.RowIndex, newCommand.Cell.ColumnIndex).Color);
                }
                else if (newCommand.CommandType == CommandType.Text)
                {
                    this.dataGridView1.Rows[newCommand.Cell.RowIndex].Cells[newCommand.Cell.ColumnIndex].Value = this.newSheet.Get_Cell(newCommand.Cell.RowIndex, newCommand.Cell.ColumnIndex).Value;
                }

                this.newSheet.UndostackPop();
            }
        }

        /// <summary>
        /// control undo and redo button.
        /// </summary>
        /// <param name="sender">Menu.</param>
        /// <param name="e">Event args.</param>
        private void UndoRedoControl(object sender, EventArgs e)
        {

            if (this.newSheet.CountUndoStack() > 0)
            {
               this.mainMenu.MenuItems[1].MenuItems[1].Enabled = true;
            }
            else
            {
                this.mainMenu.MenuItems[1].MenuItems[1].Enabled = false;
            }

            if (this.newSheet.CountRedoStack() > 0)
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