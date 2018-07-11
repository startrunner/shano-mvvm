using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ShanoMVVM.DemoApplication.ViewModels;
using ShanoMVVM.DemoApplication.Views;

namespace ShanoMVVM.DemoApplication
{
    public partial class App : Application
    {
        public App()
        {
            new ShellView(new ShellViewModel()).ShowDialog();
        }
    }
}
