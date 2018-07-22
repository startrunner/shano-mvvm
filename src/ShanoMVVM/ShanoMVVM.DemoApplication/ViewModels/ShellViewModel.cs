using System;
using System.Windows;
using AlexanderIvanov.ShanoMVVM;

namespace ShanoMVVM.DemoApplication.ViewModels
{
    public class ShellViewModel : ViewModel
    {
        public string Alpha { get; set; } = "A";

        public string Beta { get; set; } = "B";

        public string Gamma => throw new NotImplementedException();

        public Action Foo => ExecuteFoo;
        public Action Bar => ExecuteBar;

        private void ExecuteOkay()
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
