﻿using System;
using System.Drawing;

namespace ThreatsManager.Extensions.Dialogs
{
    // Source adapted from https://www.codeproject.com/Articles/106467/How-to-Display-Images-in-ComboBox-in-5-Minutes

    [Serializable]
    public class ComboBoxItem
    {
        private object _value;
        private Image _image;

        /// <summary>
        /// ComobBox Item.
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }


        /// <summary>
        /// Item image.
        /// </summary>
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }


        public ComboBoxItem()
        {
            _value = String.Empty;
            _image = new Bitmap(1, 1);
        }


        /// <summary>
        /// Constructor item without image.
        /// </summary>
        /// <param name="value">Item value.</param>
        public ComboBoxItem(object value)
        {
            _value = value;
            _image = new Bitmap(1, 1);

        }


        /// <summary>
        ///  Constructor item with image.
        /// </summary>
        /// <param name="value">Item value.</param>
        /// <param name="image">Item image.</param>
        public ComboBoxItem(object value, Image image)
        {
            _value = value;
            _image = image;
        }


        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
