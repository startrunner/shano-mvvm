using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShanoLibraries.MVVM.DependencyObjectHelpers
{
    public class DependencyPropertyWrapper
    {
        public static implicit operator DependencyProperty(DependencyPropertyWrapper wrapper) => wrapper.wrapee;

        internal DependencyPropertyWrapper(DependencyProperty wrapee) => this.wrapee = wrapee;
        private protected DependencyProperty wrapee;
    }
}
