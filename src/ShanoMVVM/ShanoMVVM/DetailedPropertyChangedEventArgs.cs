using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ShanoMVVM
{
    public class DetailedPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public DetailedPropertyChangedEventArgs(string propertyName) : base(propertyName) { }

        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}
