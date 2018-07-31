using System;
using ShanoLibraries.MVVM.Infrastructure;

namespace ShanoLibraries.MVVM
{
    /// <summary>
    /// An abstraction over an IOC container to be used by <see cref="InjectingViewModel"./>
    /// </summary>
    /// <typeparam name="TContainer">The type of the IOC container</typeparam>
    public class DependencyManager<TContainer> : IDependencyManager
    {
        public delegate object Injection(TContainer container, Type serviceType);
        public delegate object KeyedInjection(TContainer container, Type serviceType, object key);

        public DependencyManager(
            TContainer container,
            Injection injectionOnConstruct,
            KeyedInjection injectOnConstructKeyed
        )
        {
            Ensure.NotNull<object>(container, nameof(container));

            mContainer = container;
            mInjectionOnConstruct = Ensure.NotNull(injectionOnConstruct, nameof(injectionOnConstruct));
            mInjectionOnConstructByIdentifier = Ensure.NotNull(injectOnConstructKeyed, nameof(injectOnConstructKeyed));
        }

        readonly TContainer mContainer;
        readonly Injection mInjectionOnConstruct;
        readonly KeyedInjection mInjectionOnConstructByIdentifier;

        public TService InjectOnConstruct<TService>() => 
            (TService)mInjectionOnConstruct(mContainer, typeof(TService));

        public TService InjectOnConstruct<TService>(object key) => 
            (TService)mInjectionOnConstructByIdentifier(mContainer, typeof(TService), key);

        public object InjectOnConstruct(Type serviceType) =>
            mInjectionOnConstruct(mContainer, serviceType);

        public object InjectOnConstruct(Type serviceType, object key) => 
            mInjectionOnConstructByIdentifier(mContainer, serviceType, key);
    }
}
