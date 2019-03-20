using Autofac;
using MahApps.Metro.Controls;
using ShanoLibraries.MVVM.DemoApplication.ViewModels;
using ShanoLibraries.MVVM.DemoApplication.Views;
using ShanoLibraries.MVVM.DependencyInjection;
using ShanoLibraries.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ShanoLibraries.MVVM.DemoApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        protected override void OnDeactivated(EventArgs e) => base.OnDeactivated(e);

        protected override void OnStartup(StartupEventArgs e)
        {
            IViewManager viewManager = new ViewManager()
                .AddAssociation<ShellViewModel, ShellView>();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register<IReadOnlyList<int>>(x => new[] { 1, 2, 3 });
            containerBuilder.Register<IViewManager>(x => viewManager);
            containerBuilder.RegisterInstance<string>("TEXT :)").Keyed<string>("key123");

            var dependencies = new DependencyProvider<IContainer>(
                containerBuilder.Build(),
                unkeyedInjection: (services, type) => services.Resolve(type),
                keyedInjection: (services, type, key) => services.ResolveKeyed(key, type)
            );

            var shellDialog = new ShellViewModel(dependencies);

            viewManager.Show(shellDialog, WindowShowBehavior.Window, onClosed: () => Shutdown(0));
            base.OnStartup(e);
        }
    }
}
