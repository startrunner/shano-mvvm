using AlexanderIvanov.ShanoMVVM.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using InjectExpression = System.Linq.Expressions.Expression<AlexanderIvanov.ShanoMVVM.Injection.Setter>;

namespace AlexanderIvanov.ShanoMVVM
{
    public sealed class DialogManager : IDialogManager, IEnumerable
    {
        readonly Dictionary<long, List<Window>> mActiveDialogs = new Dictionary<long, List<Window>>();
        readonly Dictionary<Type, Func<object, FrameworkElement>> mAssociations = new Dictionary<Type, Func<object, FrameworkElement>>();
        readonly Dictionary<Type, List<Injection>> mInjections = new Dictionary<Type, List<Injection>>();
        readonly IServiceProvider mServiceProvider;
        public Func<FrameworkElement, Window> WindowCreator { get; set; } = DefaultCreateWindow;

        public DialogManager(IServiceProvider serviceProvider = null)
        {
            mServiceProvider = serviceProvider;
        }

        public DialogManager Add<TViewModel, TView>() where TView : FrameworkElement, new() =>
            AddAssociation(typeof(TViewModel), x => new TView());

        public DialogManager Add<TViewModel>(Func<TViewModel, FrameworkElement> viewCreator) =>
            AddAssociation(typeof(TViewModel), x => viewCreator((TViewModel)x));

        private DialogManager AddAssociation(Type vmType, Func<object, FrameworkElement> viewCreator)
        {
            mAssociations.Add(vmType, viewCreator);

            if (mServiceProvider == null) return this;
            PropertyInfo[] injectingProperties = vmType.GetRuntimeProperties().Where(x => x.GetCustomAttribute<InjectOnShowAttribute>() != null).ToArray();
            FieldInfo[] injectingFields = vmType.GetRuntimeFields().Where(x => x.GetCustomAttribute<InjectOnShowAttribute>() != null).ToArray();
            foreach (PropertyInfo property in injectingProperties) AddInjection(vmType, property.PropertyType, property.Name);
            foreach (FieldInfo field in injectingFields) AddInjection(vmType, field.FieldType, field.Name);
            return this;
        }


        private void AddInjection(Type vmType, Type propertyType, string propertyName)
        {
            if (!mInjections.ContainsKey(vmType)) mInjections.AddNew(vmType);

            ParameterExpression paramViewModel = InjectExpression.Parameter(typeof(object), "viewModelObject");
            ParameterExpression paramInjected = InjectExpression.Parameter(typeof(object), "injectedObject");
            UnaryExpression viewModelCast = InjectExpression.ConvertChecked(paramViewModel, vmType);
            UnaryExpression injectedCast = InjectExpression.ConvertChecked(paramInjected, propertyType);

            InjectExpression e = InjectExpression.Lambda<Injection.Setter>(
                parameters: new[] { paramViewModel, paramInjected },
                body: InjectExpression.Assign(
                    left: InjectExpression.PropertyOrField(viewModelCast, propertyName),
                    right: injectedCast
                )
            );
            ;
            mInjections[vmType].Add(new Injection(propertyType, e.Compile()));
        }



        public bool? Show(
            object viewModel,
            WindowShowBehavior behavior = WindowShowBehavior.Dialog,
            IViewModel ownerViewModel = null,
            Action onClosed = null
        )
        {
            if (!TryGetView(viewModel, out FrameworkElement view, out Exception getViewException)) throw getViewException;
            InjectOnShowing(viewModel);

            ViewModel libraryViewModel = viewModel as ViewModel;
            bool isLibraryViewModel = libraryViewModel != null;

            IViewModel viewModelInterface = viewModel as IViewModel;
            bool viewModelImplementsInterface = viewModelInterface != null;

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

            if (viewModelImplementsInterface)
            {
                window.Closing += (sender, e) => UnregisterActiveWindow(viewModelInterface, sender as Window);
                window.Closed += (sender, e) => (sender as Window).Owner = null;

                RegisterActiveWindow(viewModelInterface, window);
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

        private bool TryGetOwnerWindow(IViewModel ownerViewModel, out Window ownerWindow)
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

        private void InjectOnShowing(object viewModel)
        {
            Type viewModelType = viewModel.GetType();

            if (mServiceProvider != null && mInjections.TryGetValue(viewModelType, out List<Injection> injections))
            {
                foreach (Injection injection in injections)
                {
                    injection.SetterAction.Invoke(viewModel, mServiceProvider.GetService(injection.ServiceType));
                }
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

        bool TryGetActiveWindow(IViewModel owner, out Window window)
        {
            mActiveDialogs.TryGetValue(owner.UniqueId, out List<Window> windows);
            window = windows?.FirstOrDefault();
            return window != null;
        }

        void RegisterActiveWindow(IViewModel owner, Window window)
        {
            if (!mActiveDialogs.TryGetValue(owner.UniqueId, out List<Window> windows))
            {
                mActiveDialogs.Add(owner.UniqueId, windows = new List<Window>());
            }
            windows.Add(window);
        }

        void UnregisterActiveWindow(IViewModel owner, Window window)
        {
            if (!mActiveDialogs.TryGetValue(owner.UniqueId, out List<Window> windows)) return;
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
