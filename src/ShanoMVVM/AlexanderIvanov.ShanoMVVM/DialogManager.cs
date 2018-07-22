using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AlexanderIvanov.ShanoMVVM
{
    public sealed class DialogManager : IDialogManager, IEnumerable
    {
        Dictionary<long, List<Window>> mActiveDialogs = new Dictionary<long, List<Window>>();
        Dictionary<Type, Func<object, FrameworkElement>> mAssociations = new Dictionary<Type, Func<object, FrameworkElement>>();
        public Func<FrameworkElement, Window> WindowCreator { get; set; } = DefaultCreateWindow;

        public DialogManager Add<TViewModel, TView>() where TView : FrameworkElement, new()
        {
            mAssociations.Add(typeof(TViewModel), x => new TView());
            return this;
        }

        public DialogManager Add<TViewModel>(Func<TViewModel, FrameworkElement> viewCreator)
        {
            mAssociations.Add(typeof(TViewModel), x => viewCreator((TViewModel)x));
            return this;
        }

        public bool? Show(
            object viewModel,
            WindowShowBehavior behavior = WindowShowBehavior.Dialog,
            ViewModel ownerVM = null,
            Action onClosed = null
        )
        {
            if (!TryGetView(viewModel, out FrameworkElement view, out Exception getViewException)) throw getViewException;

            if (!(view is Window window))
            {
                window = WindowCreator.Invoke(view);
                if (view is Page page)
                {
                    window.Width = page.WindowWidth;
                    window.Height = page.WindowHeight;
                    window.Title = page.WindowTitle;
                }
            }
            window.DataContext = viewModel;

            bool? viewModelCloseResult = null;
            bool closedByViewModel = false;

            if (ownerVM != null && TryGetActiveWindow(ownerVM, out Window ownerWindow)) window.Owner = ownerWindow;

            if (viewModel is ViewModel vm)
            {
                vm.TryingToClose += (sender, r) =>
                {
                    closedByViewModel = true;
                    viewModelCloseResult = r;
                    window.Close();
                };

                window.Closing += (sender, e) => UnregisterActiveWindow(vm, sender as Window);
                window.Closed += (sender, e) => (sender as Window).Owner = null;

                RegisterActiveWindow(vm, window);
            }

            if (onClosed != null) window.Closed += (sender, e) => onClosed.Invoke();

            if (behavior == WindowShowBehavior.Dialog)
            {
                bool? result = window.ShowDialog();
                return !closedByViewModel ? result : viewModelCloseResult;
            }
            else
            {
                window.Show();
                return null;
            }
        }

        bool TryGetView(object viewModel, out FrameworkElement view, out Exception exception)
        {
            Type vmType = viewModel.GetType();

            if (!mAssociations.TryGetValue(vmType, out Func<object, FrameworkElement> viewCreator))
            {
                view = null;
                exception = new InvalidOperationException($"ViewModel of type {vmType} has no associated views.");
                return false;
            }

            view = viewCreator.Invoke(viewModel);
            if (view == null)
            {
                exception = new InvalidOperationException($"ViewModel of type {vmType} has no associated views.");
                return false;
            }

            exception = null;
            return true;
        }

        bool TryGetActiveWindow(ViewModel owner, out Window window)
        {
            mActiveDialogs.TryGetValue(owner.ViewModelId, out List<Window> windows);
            window = windows?.FirstOrDefault();
            return window != null;
        }

        void RegisterActiveWindow(ViewModel owner, Window window)
        {
            if (!mActiveDialogs.TryGetValue(owner.ViewModelId, out List<Window> windows))
            {
                mActiveDialogs.Add(owner.ViewModelId, windows = new List<Window>());
            }
            windows.Add(window);
        }

        void UnregisterActiveWindow(ViewModel owner, Window window)
        {
            if (!mActiveDialogs.TryGetValue(owner.ViewModelId, out List<Window> windows)) return;
            windows.RemoveAll(x => ReferenceEquals(x, window));
        }

        private static Window DefaultCreateWindow(FrameworkElement element)
        {
            return new Window() {
                Content = element
            };
        }

        public IEnumerator GetEnumerator() => throw new NotImplementedException();
    }
}
