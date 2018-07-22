using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;

namespace AlexanderIvanov.ShanoMVVM
{
    public sealed class Bind : MarkupExtension
    {
        private static bool IsInDesignTime { get; } = DesignerProperties.GetIsInDesignMode(new DependencyObject());


        class DPWrapper : FrameworkElement
        {
            public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
                nameof(Value),
                typeof(object),
                typeof(DPWrapper)
            );

            public object Value { get => GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        }

        public Bind(Binding wrapee) => this.mWrapee = (Binding)wrapee;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var wrapper = new DPWrapper();
            wrapper.SetValue(DPWrapper.ValueProperty, mWrapee.ProvideValue(serviceProvider));

            object val = wrapper.Value; ;

            if (IsInDesignTime)
            {
                if (DesignTimeValue != null) return DesignTimeValue;
                else return mWrapee.ElementName ?? mWrapee?.Path?.Path;
            }
            else
            {
                object value = mWrapee.ProvideValue(serviceProvider);
                return value;
            }
        }

        public Binding Wrapee => mWrapee;
        readonly Binding mWrapee;

        public object DesignTimeValue { get; set; }

        public bool ConvertToCommand { get; set; } = true;

        public UpdateSourceTrigger UpdateSourceTrigger { get => mWrapee.UpdateSourceTrigger; set => mWrapee.UpdateSourceTrigger = value; }
        public bool NotifyOnSourceUpdated { get => mWrapee.NotifyOnSourceUpdated; set => mWrapee.NotifyOnSourceUpdated = value; }
        public bool NotifyOnTargetUpdated { get => mWrapee.NotifyOnTargetUpdated; set => mWrapee.NotifyOnTargetUpdated = value; }
        public bool NotifyOnValidationError { get => mWrapee.NotifyOnValidationError; set => mWrapee.NotifyOnValidationError = value; }
        public IValueConverter Converter { get => mWrapee.Converter; set => mWrapee.Converter = value; }
        public object ConverterParameter { get => mWrapee.ConverterParameter; set => mWrapee.ConverterParameter = value; }
        public CultureInfo ConverterCulture { get => mWrapee.ConverterCulture; set => mWrapee.ConverterCulture = value; }
        public object Source { get => mWrapee.Source; set => mWrapee.Source = value; }
        public RelativeSource RelativeSource { get => mWrapee.RelativeSource; set => mWrapee.RelativeSource = value; }
        public string ElementName { get => mWrapee.ElementName; set => mWrapee.ElementName = value; }
        public bool IsAsync { get => mWrapee.IsAsync; set => mWrapee.IsAsync = value; }
        public object AsyncState { get => mWrapee.AsyncState; set => mWrapee.AsyncState = value; }
        public BindingMode Mode { get => mWrapee.Mode; set => mWrapee.Mode = value; }
        public string XPath { get => mWrapee.XPath; set => mWrapee.XPath = value; }
        public bool ValidatesOnDataErrors { get => mWrapee.ValidatesOnDataErrors; set => mWrapee.ValidatesOnDataErrors = value; }
        public bool ValidatesOnNotifyDataErrors { get => mWrapee.ValidatesOnNotifyDataErrors; set => mWrapee.ValidatesOnNotifyDataErrors = value; }
        public bool BindsDirectlyToSource { get => mWrapee.BindsDirectlyToSource; set => mWrapee.BindsDirectlyToSource = value; }
        public bool ValidatesOnExceptions { get => mWrapee.ValidatesOnExceptions; set => mWrapee.ValidatesOnExceptions = value; }
        public Collection<ValidationRule> ValidationRules => mWrapee.ValidationRules;
        public PropertyPath Path { get => mWrapee.Path; set => mWrapee.Path = value; }
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter { get => mWrapee.UpdateSourceExceptionFilter; set => mWrapee.UpdateSourceExceptionFilter = value; }

        public static void AddSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler) =>
            System.Windows.Data.Binding.AddSourceUpdatedHandler(element, handler);
        public static void AddTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler) =>
            System.Windows.Data.Binding.AddTargetUpdatedHandler(element, handler);
        public static XmlNamespaceManager GetXmlNamespaceManager(DependencyObject target) =>
            System.Windows.Data.Binding.GetXmlNamespaceManager(target);
        public static void RemoveSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler) =>
            System.Windows.Data.Binding.RemoveSourceUpdatedHandler(element, handler);
        public static void RemoveTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler) =>
            System.Windows.Data.Binding.RemoveSourceUpdatedHandler(element, handler);
        public static void SetXmlNamespaceManager(DependencyObject target, XmlNamespaceManager value) =>
            System.Windows.Data.Binding.SetXmlNamespaceManager(target, value);
        public bool ShouldSerializePath() => mWrapee.ShouldSerializePath();
        public bool ShouldSerializeSource() => mWrapee.ShouldSerializeSource();
        public bool ShouldSerializeValidationRules() => mWrapee.ShouldSerializeValidationRules();
    }
}
