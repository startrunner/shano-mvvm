using System;
using AlexanderIvanov.ShanoMVVM.Infrastructure;
using NUnit.Framework;

namespace Test.ShanoMVVM.Infrastructure
{
    [TestFixture]
    public class GuardTests
    {
        [TestCase(null)]
        [TestCase("The quick brown fox jumps over the lazy dog.")]
        public void NotNull(object obj)
        {
            if (obj is null == false) Assert.DoesNotThrow(() => Ensure.NotNull(obj, nameof(obj)));
            else Assert.Throws(typeof(NullReferenceException), () => Ensure.NotNull(obj, nameof(obj)));
        }
    }
}
