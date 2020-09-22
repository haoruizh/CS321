namespace CS321
{
    using System.ComponentModel;

    /// <summary>
    /// abstract base cell class for spreadsheet.
    /// </summary>
    public abstract class BaseCell : INotifyPropertyChanged
    {
        /// <summary>
        /// cell text change event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Text propery.
        /// </summary>
        protected string text;

        /// <summary>
        /// Value Property.
        /// </summary>
        protected string value;
        private readonly int rowIndex;
        private readonly int columnIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCell"/> class.
        /// Basecell constructor.
        /// </summary>
        /// <param name="curRowIndex">Cell row index.</param>
        /// <param name="curColIndex">Cell col index.</param>
        protected BaseCell(int curRowIndex, int curColIndex)
        {
            this.rowIndex = curRowIndex;
            this.columnIndex = curColIndex;
        }

        /// <summary>
        /// Gets rowIndex. rowIndex is read only.
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
        /// Gets or sets for text.
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
        /// change local value property to newValue.
        /// </summary>
        /// <param name="newValue">newValue passed in</param>
        internal void ChangeValue(string newValue)
        {
            this.value = newValue;
        }

        /// <summary>
        /// gets for value.
        /// </summary>
        public string Value => this.value;
    }
}
