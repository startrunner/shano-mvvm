using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShanoMVVM
{
    public abstract class ViewModelBase : MarshalByRefObject, INotifyPropertyChanged
    {
        public event EventHandler<DetailedPropertyChangedEventArgs> PropertyChangedWithDetails;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Lazy<DesignTimeConverter> mDesignTimeConverter = new Lazy<DesignTimeConverter>();

        public DesignTimeConverter DesignTimeConverter
        {
            get => mDesignTimeConverter.Value;
            set { }
        }

        public ViewModelBase()
        {
            PropertyChangedWithDetails += (sender, e) => PropertyChanged?.Invoke(sender, e);
        }

        public ViewModelBase NotifyChange([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName)
            );
            return this;
        }

        protected ViewModelBase NotifyChangeDetailed<T>(T oldValue, T newValue, [CallerMemberName]string propertyNameAutomatic = null) => NotifyChangeDetailed(
            propertyName: propertyNameAutomatic,
            oldValue: oldValue,
            newValue: newValue
        );

        protected ViewModelBase NotifyChangeDetailed<T>(string propertyName, T oldValue, T newValue)
        {
            PropertyChangedWithDetails?.Invoke(
                this,
                new DetailedPropertyChangedEventArgs(propertyName) {
                    OldValue = oldValue,
                    NewValue = newValue
                }
            );
            return this;
        }

        protected ViewModelBase SetAndNotify<T>(ref T field, T newValue, [CallerMemberName] string automaticPropertyName = null) => SetAndNotify(
            propertyName: automaticPropertyName,
            field: ref field,
            newValue: newValue
        );

        protected ViewModelBase SetAndNotify<T>(string propertyName, ref T field, T newValue)
        {
            field = newValue;
            NotifyChange(propertyName);
            return this;
        }

        
    }
}
