using ShanoLibraries.MVVM.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace ShanoLibraries.MVVM.Dialogs
{
    /// <summary>
    /// An implementation for <see cref="IViewManager"/>
    /// </summary>
    public sealed class ViewManager : IViewManager
    {
        readonly ObjectIDGenerator viewModelIDGenerator = new ObjectIDGenerator();
        readonly Dictionary<long, List<Window>> mActiveDialogs = new Dictionary<long, List<Window>>();
        readonly Dictionary<Type, Func<object, FrameworkElement>> mAssociations = new Dictionary<Type, Func<object, FrameworkElement>>();

        /// <summary>
        /// A factory function for <see cref="Window"/> or an inherited type.
        /// Called when trying to show a non-<see cref="Window"/> <see cref="FrameworkElement"/> as a window or dialog.
        /// Must set the the <see cref="FrameworkElement"/> as the <see cref="Window"/>'s content
        /// </summary>
        public Func<FrameworkElement, Window> WindowCreator { get; set; } = DefaultCreateWindow;

        /// <summary>
        /// Associates a type of <see cref="object"/> with the appropriate type of <see cref="FrameworkElement"/> view
        /// to be used by <see cref="Show(object, WindowShowBehavior, object, Action)"/>.
        /// </summary>
        /// <returns>The dialog manager returns itself :)</returns>
        public ViewManager AddAssociation<TViewModel, TView>()
            where TView : FrameworkElement, new()
            where TViewModel : class
            =>
            AddAssociation(typeof(TViewModel), x => new TView());

        /// <summary>
        /// Associates a type of <see cref="object"/> with a factory function for
        /// appropriate <see cref="FrameworkElement"/> views.
        /// </summary>
        /// <returns>The dialog manager returns itself :)</returns>
        public ViewManager AddAssociation<TViewModel>(Func<TViewModel, FrameworkElement> viewCreator)
            where TViewModel : class
            =>
            AddAssociation(typeof(TViewModel), x => viewCreator((TViewModel)x));

        private ViewManager AddAssociation(Type vmType, Func<object, FrameworkElement> viewCreator)
        {
            mAssociations.Add(vmType, viewCreator);
            return this;
        }


        public bool? Show(
            object viewModel,
            WindowShowBehavior behavior = WindowShowBehavior.Dialog,
            object ownerViewModel = null,
            Action onClosed = null
        )
        {
            if (!TryGetView(viewModel, out FrameworkElement view, out Exception getViewException)) throw getViewException;

            StandardViewModel libraryViewModel = viewModel as StandardViewModel;
            bool isLibraryViewModel = libraryViewModel != null;

            if (isLibraryViewModel) libraryViewModel.InvokeOnShowing();

            Window window = GetWindow(viewModel, view);

            bool? viewModelCloseResult = null;
            bool closedByViewModel = false;

            if (TryGetOwnerWindow(ownerViewModel, out Window ownerWindow)) window.Owner = ownerWindow;

            if (isLibraryViewModel)
            {
                libraryViewModel.TryingToClose += (sender, r) =>
                {
                    closedByViewModel = true;
                    viewModelCloseResult = r;
                    window.Close();
                };
            }

            window.Closing += (sender, e) => UnregisterActiveWindow(viewModel, sender as Window);
            window.Closed += (sender, e) => (sender as Window).Owner = null;

            RegisterActiveWindow(viewModel, window);

            if (onClosed != null) window.Closed += (sender, e) => onClosed.Invoke();

            switch (behavior)
            {
                case WindowShowBehavior.Dialog:
                    bool? result = window.ShowDialog();
                    return !closedByViewModel ? result : viewModelCloseResult;
                case WindowShowBehavior.Window:
                    window.Show();
                    return null;
                default:
                    throw new NotSupportedException($"Value {behavior} of enum {typeof(WindowShowBehavior)} is not handled");
            }
        }

        private bool TryGetOwnerWindow(object ownerViewModel, out Window ownerWindow)
        {
            ownerWindow = null;
            bool success = ownerViewModel != null && TryGetActiveWindow(ownerViewModel, out ownerWindow);
            return success;
        }

        private Window GetWindow(object viewModel, FrameworkElement view)
        {
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
            return window;
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

        bool TryGetActiveWindow(object ownerViewModel, out Window window)
        {
            long viewModelID = viewModelIDGenerator.GetId(ownerViewModel, out _);
            mActiveDialogs.TryGetValue(viewModelID, out List<Window> windows);
            window = windows?.FirstOrDefault();
            return window != null;
        }

        void RegisterActiveWindow(object viewModel, Window window)
        {
            long viewModelID = viewModelIDGenerator.GetId(viewModel, out _);
            if (!mActiveDialogs.TryGetValue(viewModelID, out List<Window> windows))
            {
                mActiveDialogs.Add(viewModelID, windows = new List<Window>());
            }
            windows.Add(window);
        }

        void UnregisterActiveWindow(object viewModel, Window window)
        {
            long viewModelID = viewModelIDGenerator.GetId(viewModel, out _);
            if (!mActiveDialogs.TryGetValue(viewModelID, out List<Window> windows)) return;
            windows.RemoveAll(x => ReferenceEquals(x, window));
        }

        private static Window DefaultCreateWindow(FrameworkElement element) => new Window() { Content = element };
    }
}
