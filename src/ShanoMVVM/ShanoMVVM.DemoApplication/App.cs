using AlexanderIvanov.ShanoMVVM;
using Autofac;
using ShanoMVVM.DemoApplication.ViewModels;
using ShanoMVVM.DemoApplication.Views;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ShanoMVVM.DemoApplication
{
    public partial class App : Application
    {
        class ContainerWrapper : IServiceProvider
        {
            readonly IContainer mContainer;
            public ContainerWrapper(IContainer container) => mContainer = container;
            public object GetService(Type serviceType) => mContainer.Resolve(serviceType);
        }
        public App()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IReadOnlyList<int>>(new List<int> { 1, 2, 3 });
            IContainer container = builder.Build();

            IDialogManager manager =
                new DialogManager(new ContainerWrapper(container))
                .Add<ShellViewModel, ShellView>();

            ShellViewModel vm = new ShellViewModel();
            manager.Show(vm);
        }
    }
}
