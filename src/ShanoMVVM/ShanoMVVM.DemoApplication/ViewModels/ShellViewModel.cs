using System;
using System.Windows;
using AlexanderIvanov.ShanoMVVM;

namespace ShanoMVVM.DemoApplication.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public string Alpha { get; set; } = "A";

        public string Beta { get; set; } = "B";

        public string Gamma => throw new NotImplementedException();

        static readonly DependencyProperty LalaProperty = RegisterDependencyProperty((ShellViewModel x) => x.Lala);
        public int Lala { get => (int)GetValue(LalaProperty); set => SetValue(LalaProperty, value); }

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
