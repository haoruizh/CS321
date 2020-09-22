﻿// <copyright file="BaseCell.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CS321
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// abstract base cell class for spreadsheet.
    /// </summary>
    public abstract class BaseCell : INotifyPropertyChanged
    {
        /// <summary>
        /// row index of the cell.
        /// </summary>
        private readonly int rowIndex;

        /// <summary>
        /// col index of the cell.
        /// </summary>
        private readonly int columnIndex;

        /// <summary>
        /// List of cells that use this cell.
        /// </summary>
        private List<string> subscriber = new List<string>();

        /// <summary>
        /// cell text change event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Text propery.
        /// </summary>
        private string text;

        /// <summary>
        /// The value of this cell.
        /// </summary>
        private string value;

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
                this.text = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }

        /// <summary>
        /// change local value property to newValue.
        /// </summary>
        /// <param name="newValue">newValue passed in.</param>
        internal void ChangeValue(string newValue)
        {
            this.value = newValue;
        }

        /// <summary>
        /// gets for value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Gets or sets cellsubscribed list.
        /// </summary>
        public List<string> Subscriber
        {
            get
            {
                return this.subscriber;
            }

            set
            {
                this.subscriber = value;
            }
        }
    }
}