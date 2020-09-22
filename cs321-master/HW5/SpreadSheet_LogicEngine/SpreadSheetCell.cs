namespace SpreadSheet_LogicEngine
{
    using CS321;

    /// <summary>
    /// Spreadsheet cell.
    /// </summary>
    internal class SpreadSheetCell : BaseCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadSheetCell"/> class.
        /// spreadsheetcell constructor.
        /// </summary>
        /// <param name="row">rowindex. </param>
        /// <param name="col">colindex.</param>
        public SpreadSheetCell(int row, int col)
            : base(row, col)
        {
        }
    }
}
