using System;

namespace ShanoMVVM
{
    [Obsolete]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DesignTimeValueAttribute : Attribute
    {
        public object Value { get; }
        public DesignTimeValueAttribute(object value) => Value = value;
    }
}
