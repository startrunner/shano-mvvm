using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AlexanderIvanov.ShanoMVVM
{
    public abstract class ValueConverter : MarkupExtension, IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException($"{nameof(ValueConverter)} of type {GetType().Name} does not support {nameof(ConvertBack)}.");
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
