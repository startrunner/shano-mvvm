using System;

namespace ShanoMVVM.DemoApplication.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public string Alpha { get; } = "A";

        public string Beta { get; } = "B";

        public string Gamma => throw new NotImplementedException();
    }
}
