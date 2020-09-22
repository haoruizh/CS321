namespace SpreadSheet_LogicEngine
{
    using CS321;

    /// <summary>
    /// Spreadsheet cell class. Inheritance Basecell.
    /// </summary>
    internal class SpreadSheetCell : BaseCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadSheetCell"/> class.
        /// cell constructor.
        /// </summary>
        /// <param name="row">cell row.</param>
        /// <param name="col">row cel.</param>
        public SpreadSheetCell(int row, int col)
            : base(row, col)
        {
        }
    }
}
