using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using uEN.Utils;

namespace uEN.UI.Binding
{
    public class DependencyPropertyBehavior : IBindingBehavior
    {
        public DependencyProperty DependencyProperty { get; set; }
        
        public object ViewModel { get; set; }
        public DependencyObject Element { get; set; }
        public LambdaExpression LambdaExpression { get; set; }

        public void Ensure()
        {
            BindingOperations.SetBinding(Element, DependencyProperty,
                new System.Windows.Data.Binding(LambdaExpression.ToSymbol()));
        }
    }
}
