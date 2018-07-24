using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace AlexanderIvanov.ShanoMVVM
{
    public abstract class ViewModel : DependencyObject, INotifyPropertyChanged, IViewModel
    {
        static long IdCounter = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<ViewModel, bool?> TryingToClose;

        public long UniqueId { get; } = Interlocked.Increment(ref IdCounter);
        Dictionary<string, RelayCommand> mCommandsByName = null;
        Dictionary<string, RelayCommand> CommandsByName =>
            mCommandsByName ?? (mCommandsByName = new Dictionary<string, RelayCommand>());

        protected void SetAndNotify<T>(
            ref T field,
            T value,
            [CallerMemberName]string propertyName = null,
            params string[] propertyNames
        )
        {
            field = value;
            NotifyChanged(propertyName);
            if (propertyNames != null) NotifyChanged(propertyNames);
        }

        protected void NotifyChanged([CallerMemberName] string propertyName = null) =>
            NotifyChanged(new[] { propertyName });

        protected void NotifyChanged(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void NotifyCanExecuteChanged(params string[] commandNames)
        {
            foreach (string command in commandNames)
            {
                if (CommandsByName.TryGetValue(command, out RelayCommand relay))
                {
                    relay.NotifyCanExecuteChanged();
                }
            }
        }

        protected ICommand CloseTrueCommand(Func<bool> canExecutePredicate = null, [CallerMemberName]string commandName = null) =>
            Command(() => TryClose(true), canExecutePredicate, commandName);
        protected ICommand CloseFalseCommand(Func<bool> canExecutePredicate = null, [CallerMemberName]string commandName = null) =>
            Command(() => TryClose(false), canExecutePredicate, commandName);

        protected ICommand Command(
            Action executeAction,
            Func<bool> canExecutePredicate = null,
            [CallerMemberName] string commandName = null)
        {
            if (!CommandsByName.TryGetValue(commandName, out RelayCommand command))
            {
                command = new RelayCommand(executeAction, canExecutePredicate, commandName);
                CommandsByName.Add(commandName, command);
            }
            return command;
        }

        protected void TryClose(bool? resultValue) => TryingToClose?.Invoke(this, resultValue);

        internal void InvokeOnShowing() => OnShowing();
        internal void InvokeOnShown() => OnShown();

        protected virtual void OnShowing() { }
        protected virtual void OnShown() { }
    }
}
