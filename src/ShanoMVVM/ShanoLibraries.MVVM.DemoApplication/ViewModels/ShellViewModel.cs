using ShanoLibraries.MVVM.DependencyInjection;
using ShanoLibraries.MVVM.Dialogs;
using ShanoLibraries.MVVM.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace ShanoLibraries.MVVM.DemoApplication.ViewModels
{
    public class ShellViewModel : StandardViewModel
    {
        public string Alpha { get; set; } = "A";

        public string Beta { get; set; } = "B";

        public string Gamma => "C";

        public IReadOnlyList<int> Intgers { get; }
        readonly IReadOnlyList<int> integers = null;
        readonly IViewManager viewManager = null;
        readonly string str;

        public ShellViewModel(IDependencyProvider dependencies)
        {
            dependencies
                .ResolveKeyed("key123", out str)
                .Resolve(out integers, out viewManager);
            Intgers = dependencies.Resolve<IReadOnlyList<int>>();
            ;
            Alpha = string.Join(", ", Intgers) + "the quick brown fox";
            Beta = string.Join(", ", integers);
        }

        bool fooCanExecute = true;
        public ICommand Foo => RelayCommand(ExecuteFoo, () => fooCanExecute);
        public ICommand Bar => RelayCommand(ExecuteBar);

        private void ExecuteOkay()
        {

        }

        protected override void OnShowing()
        {

        }

        public void ExecuteFoo()
        {
            MessageBox.Show(string.Join(", ", Intgers), "foo");
        }

        public void ExecuteBar()
        {
            Beta += '_';
            NotifyChanged(nameof(Beta));
            fooCanExecute = !fooCanExecute;
            NotifyCanExecuteChanged(nameof(Foo));
        }
    }
}
