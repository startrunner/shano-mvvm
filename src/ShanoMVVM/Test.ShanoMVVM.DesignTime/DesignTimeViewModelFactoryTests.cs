using System;
using NUnit.Framework;
using ShanoMVVM;

namespace Test.ShanoMVVM.DesignTime
{
    [Obsolete]
    [TestFixture]
    public class DesignTimeViewModelFactoryTests
    {
        class MyVM : ViewModelBase
        {
            object Model { get; } = null;

            [DesignTimeValue("Design-time model :)")]
            public string AnotherProp => Model.ToString();
        }

        [Test]
        public void TestPishka()
        {
            MyVM mock = DesignTimeViewModel.Create<MyVM>();
            Assert.AreEqual("Design-time model :)", mock.AnotherProp);
        }
    }
}
