using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShanoLibraries.MVVM.ValueConverters
{
    public class BooleanToVisibility : MarkupExtensionValueConverter
    {
        public Visibility ValueIfFalse { get; set; } = Visibility.Collapsed;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is true ? Visibility.Visible : ValueIfFalse;
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is Visibility == false || (Visibility)value != Visibility.Visible;
    }
}
