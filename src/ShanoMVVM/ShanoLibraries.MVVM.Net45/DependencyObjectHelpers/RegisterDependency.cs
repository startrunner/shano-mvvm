using ShanoLibraries.MVVM.DependencyObjectHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShanoLibraries.MVVM.DependencyObjectHelpers
{
    public static class RegisterDependency
    {
        public static DependencyProperty<TProperty> Property<TOwner, TProperty>(
            [CallerMemberName]string propertyName = null,
            TProperty defaultValue = default(TProperty),
            PropertyChangedCallback propertyChangedCallback = null,
            CoerceValueCallback coerceValueCallback = null
        )
            where TOwner : DependencyObject
        {
            const string PropertySuffix = "Property";
            if (propertyName.EndsWith(PropertySuffix))
            {
                propertyName = propertyName.Substring(0, propertyName.Length - PropertySuffix.Length);
            }

            return new DependencyProperty<TProperty>(DependencyProperty.Register(
                propertyName,
                typeof(TProperty),
                typeof(TOwner),
                new PropertyMetadata(
                    defaultValue,
                    propertyChangedCallback,
                    coerceValueCallback
                )
            ));
        }
    }
}
