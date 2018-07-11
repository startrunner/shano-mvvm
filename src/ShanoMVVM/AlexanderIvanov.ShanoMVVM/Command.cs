using System;
using System.Windows.Input;

namespace AlexanderIvanov.ShanoMVVM
{
    public sealed class Command : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }

        Action<object> ExecuteAction { get; }
        Func<object, bool> CanExecutePredicate { get; }

        public Command(Action executeAction, Func<bool> canExecutePredicate=null) 
        {
            ExecuteAction = x=> executeAction();
            if(canExecutePredicate!=null)
            {
                CanExecutePredicate = x => canExecutePredicate();
            }
        }

        public Command(Action<object> executeAction, Func<object, bool> canExecutePredicate = null)
        {
            ExecuteAction = executeAction;
            CanExecutePredicate = canExecutePredicate ;
        }

        public bool CanExecute(object parameter) => CanExecutePredicate?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => ExecuteAction.Invoke(parameter);
    }
}
