using System.Windows;

namespace ShanoLibraries.MVVM.DemoApplication.Views
{
    public partial class ShellView : Window, IBlockableWindow
    {
        public ShellView()
        {
            InitializeComponent();
        }

        public bool IsBlocked => xBlockingGrid.Visibility != Visibility.Visible;

        public void Block(FrameworkElement popup)
        {
            xBlockingGrid.Visibility = Visibility.Visible;
            xBlockingGrid.Children.Add(popup);
        }
        public void Unblock()
        {
            xBlockingGrid.Children.Clear();
            xBlockingGrid.Visibility = Visibility.Collapsed;
        }
    }
}
