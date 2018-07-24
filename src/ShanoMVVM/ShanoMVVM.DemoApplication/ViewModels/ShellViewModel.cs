using System;
using System.Collections.Generic;
using System.Windows;
using AlexanderIvanov.ShanoMVVM;

namespace ShanoMVVM.DemoApplication.ViewModels
{
    public class ShellViewModel : ViewModel
    {
        public string Alpha { get; set; } = "A";

        public string Beta { get; set; } = "B";

        public string Gamma => "C";

        [InjectOnShow]
        public IReadOnlyList<int> Integers { get; private set; }

        [InjectOnShow]
        readonly IReadOnlyList<int> mIntegers;

        public Action Foo => ExecuteFoo;
        public Action Bar => ExecuteBar;

        private void ExecuteOkay()
        {
            
        }

        protected override void OnShowing()
        {

        }

        public void ExecuteFoo()
        {
        }

        public void ExecuteBar()
        {

        }
    }
}
