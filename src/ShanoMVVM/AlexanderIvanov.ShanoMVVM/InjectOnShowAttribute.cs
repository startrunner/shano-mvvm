using System;

namespace AlexanderIvanov.ShanoMVVM
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class InjectOnShowAttribute : Attribute { }
}
