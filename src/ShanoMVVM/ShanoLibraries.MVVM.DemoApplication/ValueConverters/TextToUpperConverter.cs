using ShanoLibraries.MVVM.ValueConverters;
using System;
using System.Globalization;

namespace ShanoLibraries.MVVM.DemoApplication.ValueConverters
{
    class TextToUpperConverter : MarkupExtensionValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value.ToString().ToUpper();
    }
}
