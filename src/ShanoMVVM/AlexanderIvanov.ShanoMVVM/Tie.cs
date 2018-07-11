using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;

namespace AlexanderIvanov.ShanoMVVM
{
    public sealed class Tie : MarkupExtension
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

        public Tie(Binding wrapee) => Wrapee = (Binding)wrapee;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var wrapper = new DPWrapper();
            wrapper.SetValue(DPWrapper.ValueProperty, Wrapee.ProvideValue(serviceProvider));

            var val = wrapper.Value; ;

            if (IsInDesignTime)
            {
                if (DesignTimeValue != null) return DesignTimeValue;
                else return Wrapee.ElementName ?? Wrapee?.Path?.Path;
            }
            else
            {
                object value = Wrapee.ProvideValue(serviceProvider);
                if (TryConvertCommand(value, serviceProvider, out ICommand convertedCommand))
                {
                    return convertedCommand;
                }
                else
                {
                    return value;
                }
            }
        }

        private bool TryConvertCommand(object bindingReturnedObject, IServiceProvider serviceProvider, out ICommand convertedCommand)
        {
            convertedCommand = null;
            if (!ConvertToCommand) { return false; }

            GetTargets(serviceProvider, out DependencyObject targetObject, out DependencyProperty targetProperty);
            if (targetProperty.PropertyType != typeof(ICommand) && targetProperty.PropertyType != typeof(Command)) return false;

            if (bindingReturnedObject is BindingExpression expression)
            {
                object propertyValue = GetValue(expression, expression.DataItem); ;
                
                if (bindingReturnedObject is Action a1) convertedCommand = new Command(a1);
                else if (bindingReturnedObject is Action<object> a2) convertedCommand = new Command(a2);
            }

            return convertedCommand != null;
        }

        static void GetTargets(IServiceProvider provider, out DependencyObject targetObject, out DependencyProperty targetProperty)
        {
            IProvideValueTarget pvt = (IProvideValueTarget)provider.GetService(typeof(IProvideValueTarget)); ;
            targetObject = (DependencyObject)pvt.TargetObject;
            targetProperty = (DependencyProperty)pvt.TargetProperty;
        }

        static void GetSources(IServiceProvider provider)
        {
            
        }

        public static Object GetValue(BindingExpression expression, object dataItem)
        {
            if (expression == null || dataItem == null)
            {
                return null;
            }

            string bindingPath = expression.ParentBinding.Path.Path;
            string[] properties = bindingPath.Split('.');

            object currentObject = dataItem;
            Type currentType = null;

            for (int i = 0; i < properties.Length; i++)
            {
                currentType = currentObject.GetType();
                PropertyInfo property = currentType.GetProperty(properties[i]);
                if (property == null)
                {
                    currentObject = null;
                    break;
                }
                currentObject = property.GetValue(currentObject, null);
                if (currentObject == null)
                {
                    break;
                }
            }

            return currentObject;
        }

        public System.Windows.Data.Binding Wrapee { get; }

        public object DesignTimeValue { get; set; }

        public bool ConvertToCommand { get; set; } = true;

        public UpdateSourceTrigger UpdateSourceTrigger { get => Wrapee.UpdateSourceTrigger; set => Wrapee.UpdateSourceTrigger = value; }
        public bool NotifyOnSourceUpdated { get => Wrapee.NotifyOnSourceUpdated; set => Wrapee.NotifyOnSourceUpdated = value; }
        public bool NotifyOnTargetUpdated { get => Wrapee.NotifyOnTargetUpdated; set => Wrapee.NotifyOnTargetUpdated = value; }
        public bool NotifyOnValidationError { get => Wrapee.NotifyOnValidationError; set => Wrapee.NotifyOnValidationError = value; }
        public IValueConverter Converter { get => Wrapee.Converter; set => Wrapee.Converter = value; }
        public object ConverterParameter { get => Wrapee.ConverterParameter; set => Wrapee.ConverterParameter = value; }
        public CultureInfo ConverterCulture { get => Wrapee.ConverterCulture; set => Wrapee.ConverterCulture = value; }
        public object Source { get => Wrapee.Source; set => Wrapee.Source = value; }
        public RelativeSource RelativeSource { get => Wrapee.RelativeSource; set => Wrapee.RelativeSource = value; }
        public string ElementName { get => Wrapee.ElementName; set => Wrapee.ElementName = value; }
        public bool IsAsync { get => Wrapee.IsAsync; set => Wrapee.IsAsync = value; }
        public object AsyncState { get => Wrapee.AsyncState; set => Wrapee.AsyncState = value; }
        public BindingMode Mode { get => Wrapee.Mode; set => Wrapee.Mode = value; }
        public string XPath { get => Wrapee.XPath; set => Wrapee.XPath = value; }
        public bool ValidatesOnDataErrors { get => Wrapee.ValidatesOnDataErrors; set => Wrapee.ValidatesOnDataErrors = value; }
        public bool ValidatesOnNotifyDataErrors { get => Wrapee.ValidatesOnNotifyDataErrors; set => Wrapee.ValidatesOnNotifyDataErrors = value; }
        public bool BindsDirectlyToSource { get => Wrapee.BindsDirectlyToSource; set => Wrapee.BindsDirectlyToSource = value; }
        public bool ValidatesOnExceptions { get => Wrapee.ValidatesOnExceptions; set => Wrapee.ValidatesOnExceptions = value; }
        public Collection<ValidationRule> ValidationRules => Wrapee.ValidationRules;
        public PropertyPath Path { get => Wrapee.Path; set => Wrapee.Path = value; }
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter { get => Wrapee.UpdateSourceExceptionFilter; set => Wrapee.UpdateSourceExceptionFilter = value; }

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
        public bool ShouldSerializePath() => Wrapee.ShouldSerializePath();
        public bool ShouldSerializeSource() => Wrapee.ShouldSerializeSource();
        public bool ShouldSerializeValidationRules() => Wrapee.ShouldSerializeValidationRules();
    }
}
