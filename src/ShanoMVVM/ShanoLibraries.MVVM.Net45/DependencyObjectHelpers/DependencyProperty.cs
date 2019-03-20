using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShanoLibraries.MVVM.DependencyObjectHelpers
{
    public class DependencyProperty<TProperty> : DependencyPropertyWrapper
    {
        public DependencyProperty(DependencyProperty property) : base(property) { }
        public TProperty Get(DependencyObject owner) => (TProperty)owner.GetValue(wrapee);
        public void Set(DependencyObject owner, TProperty value) => owner.SetValue(wrapee, value);
    }
}
