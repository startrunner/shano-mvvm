using System;
using System.Globalization;

namespace ShanoLibraries.MVVM.DemoApplication.ValueConverters
{
    class TextToUpperConverter : ValueConverter
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value.ToString().ToUpper();
    }
}
