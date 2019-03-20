using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ShanoLibraries.MVVM.Interaction
{
    public sealed class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public RelayCommand(Action executeAction, Func<bool> canExecutePredicate = null)
        {
            if (executeAction == null) throw new ArgumentNullException(nameof(executeAction));

            this.executeAction = (x => executeAction());
            if (canExecutePredicate != null) this.canExecutePredicate = (x => canExecutePredicate());
        }

        public RelayCommand(Action<object> executeAction, Func<object, bool> canExecutePredicate = null)
        {
            if (executeAction == null) throw new ArgumentNullException(nameof(executeAction));

            this.canExecutePredicate = canExecutePredicate;
            if (canExecutePredicate != null) this.executeAction = executeAction;
        }

        readonly Func<object, bool> canExecutePredicate = null;
        readonly Action<object> executeAction;


        public bool CanExecute(object parameter) => 
            canExecutePredicate?.Invoke(parameter) ?? true;

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;
            executeAction.Invoke(parameter);
        }
    }
}
