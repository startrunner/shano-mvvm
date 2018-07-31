using System.Windows.Input;

namespace ShanoLibraries.MVVM.DemoApplication.ViewModels
{
    class BlockingViewModel: ViewModel
    {
        public ICommand Close => CloseTrueCommand();
    }
}
