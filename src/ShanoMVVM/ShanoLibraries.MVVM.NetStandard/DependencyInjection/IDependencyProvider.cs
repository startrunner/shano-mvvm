using System;

namespace ShanoLibraries.MVVM.DependencyInjection
{
    /// <summary>
    /// An abstraction over an IOC container to be used by <see cref="InjectingViewModel"./>
    /// Implemented by <see cref="DependencyProvider{TContainer}"/>
    /// </summary>
    public interface IDependencyProvider
    {

        TService Resolve<TService>();
        TService Resolve<TService>(object key);
        object Resolve(Type serviceType);
        object Resolve(Type serviceType, object identifier);
    }

    public static class DependencyProviderExtensions
    {
        public static IDependencyProvider ResolveKeyed<TService, TKey>(this IDependencyProvider provider, TKey key, out TService service)
        {
            service = provider.Resolve<TService>(key: key);
            return provider;
        }

        public static IDependencyProvider Resolve<TService>(this IDependencyProvider provider, out TService service)
        {
            service = provider.Resolve<TService>();
            return provider;
        }
        public static IDependencyProvider Resolve<T1, T2>(this IDependencyProvider provider, out T1 s1, out T2 s2) { provider.Resolve(out s1); provider.Resolve(out s2); return provider; }
        public static IDependencyProvider Resolve<T1, T2, T3>(this IDependencyProvider provider, out T1 s1, out T2 s2, out T3 s3) { provider.Resolve(out s1); provider.Resolve(out s2, out s3); return provider; }
        public static IDependencyProvider Resolve<T1, T2, T3, T4>(this IDependencyProvider provider, out T1 s1, out T2 s2, out T3 s3, out T4 s4) { provider.Resolve(out s1); provider.Resolve(out s2, out s3, out s4); return provider; }
        public static IDependencyProvider Resolve<T1, T2, T3, T4, T5>(this IDependencyProvider provider, out T1 s1, out T2 s2, out T3 s3, out T4 s4, out T5 s5) { provider.Resolve(out s1); provider.Resolve(out s2, out s3, out s4, out s5); return provider; }
        public static IDependencyProvider Resolve<T1, T2, T3, T4, T5, T6>(this IDependencyProvider provider, out T1 s1, out T2 s2, out T3 s3, out T4 s4, out T5 s5, out T6 s6) { provider.Resolve(out s1); provider.Resolve(out s2, out s3, out s4, out s5, out s6); return provider; }
    }
}
