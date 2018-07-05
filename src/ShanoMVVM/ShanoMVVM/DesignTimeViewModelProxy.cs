using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace ShanoMVVM
{
    [Obsolete]
    class DesignTimeViewModelProxy : RealProxy
    {
        readonly Lazy<IReadOnlyDictionary<string, object>> mPropertyDesignTimeValues;
        readonly Type mType;

        public DesignTimeViewModelProxy(Type type) : base(type)
        {
            mType = type;

            mPropertyDesignTimeValues = new Lazy<IReadOnlyDictionary<string, object>>(
                () => GetPropertyDesignTimeValues(type),
                isThreadSafe: false
            );
        }

        public string Alpha => "the fucking alpha";

        public IReadOnlyDictionary<string, object> PropertyDesignTimeValues =>
            mPropertyDesignTimeValues.Value;

        public override IMessage Invoke(IMessage msg)
        {
            switch (msg)
            {
                case IMethodCallMessage methodCall:
                    return Invoke(methodCall);
                default:
                    throw new NotImplementedException();
            }
        }

        private IMessage Invoke(IMethodCallMessage message)
        {
            var methodInfo = (MethodInfo)MethodInfo.GetMethodFromHandle(message.MethodBase.MethodHandle);
            string methodName = message.MethodName;
            ;

            if (methodName == nameof(GetType))
            {
                return new ReturnMessage(
                    mType, null, 0, message.LogicalCallContext, message
                );
            }

            if (PropertyDesignTimeValues.TryGetValue(methodName, out object returnValue)) { }
            else returnValue = GetDefaultValue(methodInfo.ReturnType);

            return new ReturnMessage(
                returnValue, null, 0, message.LogicalCallContext, message
            );
        }

        private static object GetDefaultValue(Type type)
        {
            if (type == typeof(void)) return null;
            if (type.IsValueType) return Activator.CreateInstance(type);
            else return null;
        }

        private static Dictionary<string, object> GetPropertyDesignTimeValues(Type type) =>
            type
            .GetRuntimeProperties()
            .Select(x => (property: x, attribute: x.GetCustomAttribute<DesignTimeValueAttribute>())).ToArray()
            .Where(x => x.attribute != null)
            .ToDictionary(x => $"get_{x.property.Name}", x => x.attribute.Value);
    }
}
