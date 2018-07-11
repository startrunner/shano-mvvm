using System.Windows;

namespace ShanoMVVM.DemoApplication.Views
{
    public partial class ShellView : Window
    {
        public ShellView(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}
