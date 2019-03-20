using ShanoLibraries.MVVM.DependencyObjectHelpers;
using System.Windows.Controls;

namespace ShanoLibraries.MVVM.DemoApplication.Views
{
    /// <summary>
    /// Interaction logic for DPView.xaml
    /// </summary>
    public partial class DPView : UserControl
    {
        public static readonly DependencyProperty<string> TextProperty = RegisterDependency.Property<DPView, string>();
        public string Text { get => TextProperty.Get(this); set => TextProperty.Set(this, value); }

        public DPView()
        {
            InitializeComponent();
        }
    }
}
