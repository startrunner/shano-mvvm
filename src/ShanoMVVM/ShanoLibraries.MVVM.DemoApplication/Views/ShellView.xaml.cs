using System.Windows;
using System.Windows.Controls;

namespace ShanoLibraries.MVVM.DemoApplication.Views
{
    public partial class ShellView : Page
    {
        public ShellView()
        {
            InitializeComponent();
        }

        public bool IsBlockedByPopup => xBlockingGrid.Visibility == Visibility.Visible;

        public void BlockWithPopup(FrameworkElement popup)
        {
            xBlockingGrid.Visibility = Visibility.Visible;
            xBlockingGrid.Children.Add(popup);
        }

        public void HidePopup()
        {
            xBlockingGrid.Children.Clear();
            xBlockingGrid.Visibility = Visibility.Collapsed;
        }
    }
}
