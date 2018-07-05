using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ShanoMVVM
{
    public sealed class DesignTimeConverter : IValueConverter
    {
        bool IsInDesignTime { get; } = DesignerProperties.GetIsInDesignMode(new DependencyObject());
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => IsInDesignTime && parameter != null ? parameter : value;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}
