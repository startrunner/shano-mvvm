using System;

namespace ShanoMVVM
{
    [Obsolete]
    public static class DesignTimeViewModel
    {
        public static T Create<T>() where T : ViewModelBase =>
            (T)new DesignTimeViewModelProxy(typeof(T)).GetTransparentProxy();
    }
}
