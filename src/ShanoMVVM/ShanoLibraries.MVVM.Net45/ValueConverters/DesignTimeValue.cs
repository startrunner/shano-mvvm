using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShanoLibraries.MVVM.ValueConverters
{
    public class DesignTimeValue : MarkupExtensionValueConverter
    {
        static readonly bool IsInDesignTime = DesignerProperties.GetIsInDesignMode(new DependencyObject());

        public object Value { get; set; }

        public DesignTimeValue() : this(null) { }
        public DesignTimeValue(object value) => Value = value;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsInDesignTime) return Value;
            else return value;
        }
    }
}
