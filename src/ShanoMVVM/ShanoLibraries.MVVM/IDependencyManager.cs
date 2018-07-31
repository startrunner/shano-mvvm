using System;

namespace ShanoLibraries.MVVM
{
    /// <summary>
    /// An abstraction over an IOC container to be used by <see cref="InjectingViewModel"./>
    /// Implemented by <see cref="DependencyManager{TContainer}"/>
    /// </summary>
    public interface IDependencyManager
    {
        TService InjectOnConstruct<TService>();
        TService InjectOnConstruct<TService>(object identifier);
        object InjectOnConstruct(Type serviceType);
        object InjectOnConstruct(Type serviceType, object identifier);
    }
}
