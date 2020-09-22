namespace CS321
{
    using System.ComponentModel;
    /// <summary>
    /// Basecell class of spreadsheet cell.
    /// </summary>
    public abstract class BaseCell : INotifyPropertyChanged
    {
        // event
        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        // cell property

        /// <summary>
        /// Text of a cell.
        /// </summary>
        protected string text;

        /// <summary>
        /// value of a cell
        /// </summary>
        protected string value;
        private readonly int rowIndex;
        private readonly int columnIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCell"/> class.
        /// Constructor of base cell class.
        /// </summary>
        /// <param name="curRowIndex"></param>
        /// <param name="curColIndex"></param>
        protected BaseCell(int curRowIndex, int curColIndex)
        {
            this.rowIndex = curRowIndex;
            this.columnIndex = curColIndex;
        }

        /// <summary>
        /// Gets rowIndex getter. rowIndex is read only.
        /// </summary>
        public int RowIndex
        {
            get
            {
                return this.rowIndex;
            }
        }

        /// <summary>
        /// Gets columnIndex getter. columnIndex is read only.
        /// </summary>
        public int ColumnIndex
        {
            get
            {
                return this.columnIndex;
            }
        }

        /// <summary>
        /// Gets or sets setter and getter for text
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text == value){return;}
                this.text = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }

        /// <summary>
        /// change local value property to newValue
        /// </summary>
        /// <param name="newValue">newValue passed in</param>
        internal void ChangeValue(string newValue)
        {
            this.value = newValue;
        }

        /// <summary>
        /// Gets getter of value property.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
