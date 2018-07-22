using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AlexanderIvanov.ShanoMVVM
{
    internal static class DependencyObjectExtensions
    {
        static Dictionary<int, Dictionary<string, DependencyProperty>> Properties = new Dictionary<int, Dictionary<string, DependencyProperty>>();

        public static T DPGet<T>(
            this DependencyObject obj,
            Func<T> defaultValue,
            [CallerFilePath]string filePath = null,
            [CallerLineNumber]int line = -1,
            [CallerMemberName]string propertyName = null
        ) =>
            DPGet<T>(
                obj,
                () => new PropertyMetadata(defaultValue: defaultValue()),
                filePath,
                line,
                propertyName
            );

        public static T DPGet<T>(
            this DependencyObject obj,
            Func<PropertyMetadata> metadata = null,
            [CallerFilePath]string filePath = null,
            [CallerLineNumber]int line = -1,
            [CallerMemberName]string propertyName = null
        )
        {
            DependencyProperty property = GetProperty<T>(filePath, line, propertyName, metadata);
            return (T)obj.GetValue(property);
            property.ToString();
        }

        public static void DPSet<T>(
            this DependencyObject obj,
            T value,
            [CallerFilePath]string filePath = null,
            [CallerLineNumber]int line = -1,
            [CallerMemberName]string propertyName = null
        )
        {
            DependencyProperty property = GetProperty<T>(filePath, line, propertyName);
            obj.SetValue(property, value);
        }

        static DependencyProperty GetProperty<T>(string filePath, int lineNumber, string propertyName, Func<PropertyMetadata> metadata = null)
        {
            if (
                Properties.TryGetValue(lineNumber, out Dictionary<string, DependencyProperty> properties) &&
                properties.TryGetValue(filePath, out DependencyProperty property)
            ) return property;

            if (properties == null)
            {
                Properties.Add(lineNumber, properties = new Dictionary<string, DependencyProperty>());
            }

            StackFrame frame = new StackFrame(2);
            MethodBase method = frame.GetMethod();
            Type dependencyObjectType = method.DeclaringType;

            string registeredPropertyName = $"{propertyName}Property";
            properties.Add(
                filePath,
                property = DependencyProperty.Register(
                    registeredPropertyName,
                    typeof(T),
                    dependencyObjectType, 
                    metadata?.Invoke()
                )
            );

            return property;
        }
    }
}
