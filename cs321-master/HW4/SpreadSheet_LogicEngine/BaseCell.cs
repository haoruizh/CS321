using System.ComponentModel;

namespace CS321
{
    public abstract class BaseCell : INotifyPropertyChanged
    {
        //event
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        //cell property
        protected string text;
        protected string value;
        private readonly int rowIndex;
        private readonly int columnIndex;


        protected BaseCell(int curRowIndex, int curColIndex)
        {
            this.rowIndex = curRowIndex;
            this.columnIndex = curColIndex;
            
        }

        /// <summary>
        /// rowIndex getter. rowIndex is read only
        /// </summary>
        public int RowIndex
        {
            get
            {
                return this.rowIndex;
            }
        }

        /// <summary>
        /// columnIndex getter. columnIndex is read only.
        /// </summary>
        public int ColumnIndex
        {
            get
            {
                return this.columnIndex;
            }
        }

        /// <summary>
        /// Setter and getter for text
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
                PropertyChanged(this, new PropertyChangedEventArgs("Text"));
               
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
        /// getter of value property
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
