using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ShanoMVVM.DemoApplication.ViewModels;
using ShanoMVVM.DemoApplication.Views;

namespace ShanoMVVM.DemoApplication
{
    public partial class App : Application
    {
        public App()
        {
            new ShellView() { DataContext = new ShellViewModel() }.ShowDialog();
        }
    }
}
