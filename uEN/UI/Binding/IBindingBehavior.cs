using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace uEN.UI.DataBinding
{
    public interface IBindingBehavior : IDisposable
    {
        object ViewModel { get; set; }
        DependencyObject Element { get; set; }
        LambdaExpression LambdaExpression { get; set; }
        IEnumerable<Attribute> Attributes { get; }
        void Ensure();
    }
}
