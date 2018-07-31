using ShanoLibraries.MVVM.Infrastructure;
using System;

namespace ShanoLibraries.MVVM
{
    internal class Injection
    {
        public delegate void Setter(object viewModel, object service);

        public Type ServiceType { get; }
        public Setter SetterAction { get; }
        public bool HasIdentifier { get; }
        public object Identifier { get; }

        public Injection(Type serviceType, Setter setterAction, bool hasIdentifier, object identifier)
        {
            ServiceType = Ensure.NotNull(serviceType, nameof(serviceType));
            SetterAction = Ensure.NotNull(setterAction, nameof(SetterAction));

            HasIdentifier = hasIdentifier;
            Identifier = Identifier;
        }
    }
}
