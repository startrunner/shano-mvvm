using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using ShanoLibraries.MVVM.DemoApplication.ViewModels;
using ShanoLibraries.MVVM.DemoApplication.Views;

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

        protected override void OnStartup(StartupEventArgs e)
        {
            IDialogManager dialogManager =
                new DialogManager()
                .AddAssociation<ShellViewModel, ShellView>()
                .AddAssociation<BlockingViewModel, BlockingView>();

            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.Register<IReadOnlyList<int>>(x => new[] { 1, 2, 3 });
            containerBuilder.Register<IDialogManager>(x => dialogManager);

            var dependencies = new DependencyManager<IContainer>(
                containerBuilder.Build(),
                injectionOnConstruct: (services, type) => services.Resolve(type),
                injectOnConstructKeyed: (services, type, key) => services.ResolveKeyed(key, type)
            );

            var shellDialog = new ShellViewModel(dependencies);
            dialogManager.Show(shellDialog, WindowShowBehavior.Window, onClosed: () => Shutdown(0));
        }
    }
}
