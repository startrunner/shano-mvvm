using System;
using NUnit.Framework;
using ShanoMVVM.Infrastructure;

namespace Test.ShanoMVVM.Infrastructure
{
    [TestFixture]
    public class GuardTests
    {
        [TestCase(null)]
        [TestCase("The quick brown fox jumps over the lazy dog.")]
        public void NotNull(object obj)
        {
            if (obj is null == false) Assert.DoesNotThrow(() => Guard.NotNull(obj, nameof(obj)));
            else Assert.Throws(typeof(NullReferenceException), () => Guard.NotNull(obj, nameof(obj)));
        }
    }
}
