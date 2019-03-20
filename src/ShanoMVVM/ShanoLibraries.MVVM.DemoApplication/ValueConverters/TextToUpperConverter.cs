using ShanoLibraries.MVVM.ValueConverters;
using System;
using System.Globalization;

namespace ShanoLibraries.MVVM.DemoApplication.ValueConverters
{
    class TextToUpperConverter : MarkupExtensionValueConverter<string, string>
    {
        public override string Convert(string value, object parameter, CultureInfo culture) => value.ToUpper();
    }
}
