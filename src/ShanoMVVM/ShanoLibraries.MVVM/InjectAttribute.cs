using System;

namespace ShanoLibraries.MVVM
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class InjectAttribute : Attribute
    {
        public InjectAttribute() { }
        public InjectAttribute(object identifier)
        {
            mHasIdentifier = true;
            mIdentifier = identifier;
        }

        readonly bool mHasIdentifier = false;
        readonly object mIdentifier = null;

        internal bool HasIdentifier(out object identifier)
        {
            identifier = mIdentifier;
            return mHasIdentifier;
        }
    }
}
