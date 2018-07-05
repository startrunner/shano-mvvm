using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShanoMVVM
{
    public class ShanoMVVMConfiguration
    {
        public Dictionary<Type, Func<object, Type>> windowAssociations;

        public Window FindWindow(ViewModelBase vm)
        {
            throw new NotImplementedException();
        }
    }
}
