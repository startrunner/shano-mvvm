using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ShanoLibraries.MVVM.ValueConverters
{
    /// <summary>
    /// Base class for an <see cref="IValueConverter"/> to be used as a <see cref="MarkupExtension"/> instead of as a static resource
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

    /// <summary>
    /// Base class for an <see cref="IValueConverter"/> to be used as a <see cref="MarkupExtension"/> instead of as a static resource
    /// </summary>
    public abstract class MarkupExtensionValueConverter<TFrom, TTo> : MarkupExtension, IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            Convert((TFrom)value, parameter, culture);
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            ConvertBack((TTo)value, parameter, culture);

        public abstract TTo Convert(TFrom value, object parameter, CultureInfo culture);

        public virtual TFrom ConvertBack(TTo value, object parameter, CultureInfo culture) =>
            throw MarkupExtensionValueConverter.NotSupportedException(GetType());

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }

}
