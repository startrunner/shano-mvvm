using System;
using System.Windows.Input;

namespace AlexanderIvanov.ShanoMVVM
{
    class CallCommand : ICommand
    {
        internal string MethodName { get; set; }
        internal object DataContext { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => throw new NotImplementedException();
    }
}
