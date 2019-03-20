using System;

namespace ShanoLibraries.MVVM.DependencyInjection
{
    /// <summary>
    /// An abstraction over an IOC container to be used by <see cref="InjectingViewModel"./>
    /// </summary>
    /// <typeparam name="TContainer">The type of the IOC container</typeparam>
    public class DependencyProvider<TContainer> : IDependencyProvider where TContainer:class
    {
        public delegate object UnkeyedInjection(TContainer container, Type serviceType);
        public delegate object KeyedInjection(TContainer container, Type serviceType, object key);

        public DependencyProvider(
            TContainer container,
            UnkeyedInjection unkeyedInjection,
            KeyedInjection keyedInjection
        )
        {

            if (container is null) throw new ArgumentNullException(nameof(container));
            if (unkeyedInjection is null) throw new ArgumentNullException(nameof(unkeyedInjection));
            if (keyedInjection is null) throw new ArgumentNullException(nameof(keyedInjection));

            this.container = container;
            this.unkeyedInjection = unkeyedInjection;
            this.keyedInjection = keyedInjection;
        }

        readonly TContainer container;
        readonly UnkeyedInjection unkeyedInjection;
        readonly KeyedInjection keyedInjection;

        public TService Resolve<TService>() => 
            (TService)unkeyedInjection(container, typeof(TService));

        public TService Resolve<TService>(object key) => 
            (TService)keyedInjection(container, typeof(TService), key);

        public object Resolve(Type serviceType) =>
            unkeyedInjection(container, serviceType);

        public object Resolve(Type serviceType, object key) => 
            keyedInjection(container, serviceType, key);
    }
}
