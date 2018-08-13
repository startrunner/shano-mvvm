using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ShanoLibraries.MVVM;

namespace ShanoLibraries.MVVM.DemoApplication.ViewModels
{
    public class ShellViewModel : InjectingViewModel
    {
        public string Alpha { get; set; } = "A";

        public string Beta { get; set; } = "B";

        public string Gamma => "C";

        [Inject]
        public IReadOnlyList<int> Integers { get; private set; } 

        [Inject]
        IReadOnlyList<int> mIntegers;

        [Inject]
        IDialogManager mDialogManager;

        [Inject("key123")] string str;

        public ShellViewModel(IDependencyManager dependencies) : base(dependencies)
        {
            Alpha = string.Join(", ", Integers) + "the quick brown fox";
            Beta = string.Join(", ", mIntegers);
        }

        bool fooCanExecute = true;
        public ICommand Foo => Command(ExecuteFoo, ()=>fooCanExecute);
        public ICommand Bar => Command(ExecuteBar);
        public ICommand Block => Command(ExecuteBlock);

        private void ExecuteBlock()
        {
            var block = new BlockingViewModel { };
            mDialogManager.Block(block, ownerViewModel: this);
        }

        private void ExecuteOkay()
        {

        }

        protected override void OnShowing()
        {

        }

        public void ExecuteFoo()
        {
            MessageBox.Show(string.Join(", ", Integers), "foo");
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
