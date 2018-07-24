using AlexanderIvanov.ShanoMVVM.Infrastructure;
using System;

namespace AlexanderIvanov.ShanoMVVM
{
    internal class Injection
    {
        public delegate void Setter(object viewModel, object service);

        public Type ServiceType { get; }
        public Setter SetterAction { get; }

        public Injection(Type serviceType, Setter setterAction)
        {
            ServiceType = Ensure.NotNull(serviceType, nameof(serviceType));
            SetterAction = Ensure.NotNull(setterAction, nameof(SetterAction));
        }
    }
}
