using System.Windows;

namespace ShanoLibraries.MVVM.Dialogs.ViewModels
{
    class MessageBoxViewModel
    {
        public MessageBoxImage Image { get; set; }
        public MessageBoxButton Button { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public MessageBoxResult Result { get; set; }

        public bool YesNoEnabled => Button == MessageBoxButton.YesNo || Button == MessageBoxButton.YesNoCancel;
        public bool CancelEnabled => Button == MessageBoxButton.OKCancel || Button == MessageBoxButton.YesNoCancel;
        public bool OKEnabled => Button == MessageBoxButton.OK || Button == MessageBoxButton.OKCancel;
    }
}
