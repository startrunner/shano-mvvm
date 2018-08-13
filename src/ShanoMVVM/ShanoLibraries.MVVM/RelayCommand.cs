﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Input;

namespace ShanoLibraries.MVVM
{
    public sealed class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public RelayCommand(Action executeAction, Func<bool> canExecutePredicate)
        {
            mExecuteAction = (x => executeAction());
            if (canExecutePredicate != null) mCanExecutePredicate = (x => canExecutePredicate());
        }

        readonly Func<object, bool> mCanExecutePredicate = null;
        readonly Action<object> mExecuteAction;


        public bool CanExecute(object parameter) => mCanExecutePredicate?.Invoke(parameter) ?? true;
        public void Execute(object parameter)
        {
            bool canExecute = CanExecute(parameter);

            if (!CanExecute(parameter)) return;

            mExecuteAction.Invoke(parameter);
        }
    }
}