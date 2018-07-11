using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AlexanderIvanov.ShanoMVVM
{
    public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged
    {
        static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, DependencyProperty>> PropertyToDependencyPropertyIndex =
            new ConcurrentDictionary<Type, ConcurrentDictionary<string, DependencyProperty>>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase()
        {

        }

        public void SetAndNotify<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            field = value;
            NotifyChanged(propertyName);
        }

        public void SetAndNotify<T>(ref T field, T value, params string[] propertyNames)
        {
            field = value;
            foreach (string name in propertyNames) NotifyChanged(name);
        }

        public void NotifyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected static DependencyProperty RegisterDependencyProperty<TOwner, TProperty>(
            Expression<Func<TOwner, TProperty>> propertySelector,
            PropertyMetadata metadata = null,
            ValidateValueCallback validateValueCallback = null,
            [CallerMemberName]string dpName = null
        )
        {
            if (!dpName.EndsWith("Property")) throw new InvalidOperationException("Dependency property names must end with 'Property'.");

            return DependencyProperty.Register(
                dpName,
                typeof(TProperty),
                typeof(TOwner),
                metadata,
                validateValueCallback
            );
        }

        public T GetDp<T>([CallerMemberName] string propertyName = null)
        {
            return default(T);
        }

        public void SetDp<T>(T value) { }

    }
}
