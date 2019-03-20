using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ShanoLibraries.MVVM.ValueConverters
{
    /// <summary>
    /// Base class for an <see cref="C"/> to be used as a <see cref="MarkupExtension"/> instead of as a static resource
    /// </summary>
    public abstract class MarkupExtensionValueConverter : MarkupExtension, IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw NotSupportedException(GetType());

        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        internal static NotSupportedException NotSupportedException(Type converterType) => 
            new NotSupportedException($"{nameof(MarkupExtensionValueConverter)} of type {converterType.Name} does not support {nameof(ConvertBack)}.");
    }
}
