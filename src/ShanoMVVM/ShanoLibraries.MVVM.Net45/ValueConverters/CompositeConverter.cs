using ShanoLibraries.MVVM.DependencyObjectHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using C = System.Windows.Data.IValueConverter;

namespace ShanoLibraries.MVVM.ValueConverters
{
    public class CompositeConverter : MarkupExtensionValueConverter
    {
        readonly C[] converters;

        public CompositeConverter() => converters = new C[] { };
        public CompositeConverter(C c1) => converters = new[] { c1 };
        public CompositeConverter(C c1, C c2) => converters = new[] { c1, c2 };
        public CompositeConverter(C c1, C c2, C c3) => converters = new[] { c1, c2, c3 };
        public CompositeConverter(C c1, C c2, C c3, C c4) => converters = new[] { c1, c2, c3, c4 };
        public CompositeConverter(C c1, C c2, C c3, C c4, C c5) => converters = new[] { c1, c2, c3, c4, c5 };
        public CompositeConverter(C c1, C c2, C c3, C c4, C c5, C c6) => converters = new[] { c1, c2, c3, c4, c5, c6 };
        public CompositeConverter(C c1, C c2, C c3, C c4, C c5, C c6, C c7) => converters = new[] { c1, c2, c3, c4, c5, c6, c7 };
        public CompositeConverter(C c1, C c2, C c3, C c4, C c5, C c6, C c7, C c8) => converters = new[] { c1, c2, c3, c4, c5, c6, c7, c8 };

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (C converter in converters)
            {
                value = converter.Convert(value, targetType, parameter, culture);
            }
            if (targetType.IsEnum) return Enum.Parse(targetType, value.ToString());
            else if (targetType.IsPrimitive || targetType == typeof(string))
            {
                try
                {
                    return PrimitiveConversions[targetType](value);
                }
                catch (Exception e)
                {
                    throw new Exception($"cannot convert '{value}' to '{targetType.Name}'! \n Converters: {string.Join(", ", converters.Select(x => x.GetType().Name))}");
                }
            }
            return value;
        }

        private static readonly Dictionary<Type, Func<object, object>> PrimitiveConversions = new Dictionary<Type, Func<object, object>> {
            {typeof(byte), x=>byte.Parse(x.ToString()) },
            {typeof(sbyte), x=>sbyte.Parse(x.ToString()) },
            {typeof(short), x=>short.Parse(x.ToString()) },
            {typeof(ushort), x=>ushort.Parse(x.ToString()) },
            {typeof(int), x=>int.Parse(x.ToString()) },
            {typeof(uint), x=>uint.Parse(x.ToString()) },
            {typeof(long), x=>long.Parse(x.ToString()) },
            {typeof(ulong), x=>ulong.Parse(x.ToString()) },
            {typeof(float), x=>float.Parse(x.ToString()) },
            {typeof(double), x=>double.Parse(x.ToString()) },
            {typeof(decimal), x=>decimal.Parse(x.ToString()) },
            {typeof(bool), x=>bool.Parse(x.ToString())},
            {typeof(string), x=>x.ToString()},
        };

        object RunThroughBinding(object value, Type targetType)
        {
            return value;
        }
    }
}
