using System;
using System.Windows.Forms;
using SpreadSheet_LogicEngine;

namespace Spreadsheet_Haorui_Zhang
{
    public partial class Form1 : Form
    {
        private Spreadsheet newSheet;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //New spreadsheet
            newSheet = new Spreadsheet(50, 26);
            dataGridView1.CellValueChanged += UpdateSpreadSheetCell;
            //clear design rows and cols
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            //create new cols A to Z in a loop
            string colNameStr = "";
            for (char colName = 'A'; colName <= 'Z'; colName++)
            {
                colNameStr = Convert.ToString(colName);
                dataGridView1.Columns.Add(colNameStr, colNameStr);
            }
            //create 50 rows from 1 to 50
            for (int index = 0; index < 50; index++)
            {
                dataGridView1.Rows.Add();
            }

            foreach (DataGridViewRow curRow in dataGridView1.Rows)
            {
                curRow.HeaderCell.Value = String.Format("{0}", curRow.Index+1 );
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// Do a demo when button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            randomCellTextChange();
            ChangeColBText();
            ChangeColAText();

        }

        /// <summary>
        /// Update the spread sheet cell
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">cell changed</param>
        private void UpdateSpreadSheetCell(object sender, DataGridViewCellEventArgs e)
        {
            //create the copy of the sender
            DataGridView copy = sender as DataGridView;
            if(e.RowIndex==-1||e.ColumnIndex==-1)
            {
                return;
            }
            string newText = copy.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            Form1 form1 = this;
            form1.newSheet.Get_Cell(e.RowIndex, e.ColumnIndex).Text = newText;
        }

        /// <summary>
        /// pick 50 random cells from spreadsheet and change their text to "HelloWorld"
        /// Display the grid view
        /// </summary>
        private void randomCellTextChange()
        {
            Random randomRow = new Random();
            Random randomCol = new Random();
            int currentRow = 0;
            int currentCol = 0;
            int randomCellCount = 0;
            newSheet.RowCount();
            newSheet.ColumnCount();
            for (randomCellCount = 0; randomCellCount <= 50; ++randomCellCount)
            {
                currentRow = randomRow.Next(1, newSheet.RowCount());
                currentCol = randomCol.Next(1, newSheet.ColumnCount());
                newSheet.Get_Cell(currentRow, currentCol).Text = "HelloWorld";
                string newValue = this.newSheet.Get_Cell(currentRow, currentCol).Value;
                this.dataGridView1.Rows[currentRow].Cells[currentCol].Value = GetValue(currentRow, currentCol);
            }

        }

        private string GetValue(int currentRow, int currentCol)
        {
            return this.newSheet.Get_Cell(currentRow, currentCol).Value;
        }

        /// <summary>
        /// do a loop to set the text in every cell in column B to “This is cell B#”, where #
        ///number is the row number for the cell.
        /// </summary>
        private void ChangeColBText()
        {
            for (int i = 0; i < newSheet.RowCount(); ++i)
            {
                newSheet.Get_Cell(i, 1).Text = ("This is cell B"+(i+1));
                dataGridView1.Rows[i].Cells[1].Value = newSheet.Get_Cell(i, 1).Value;
            }
        }

        private void ChangeColAText()
        {
            for (int i = 0; i < newSheet.RowCount(); ++i)
            {
                newSheet.Get_Cell(i, 0).Text = "=B" + (i + 1);
                dataGridView1.Rows[i].Cells[0].Value = this.newSheet.Get_Cell(i, 0).Value;
            }
        }
    }
}
