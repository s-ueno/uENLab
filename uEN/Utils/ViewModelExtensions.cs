using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using uEN.UI;
using uEN.UI.Binding;

namespace uEN
{
    public static class ViewModelExtensions
    {
        public static bool TryFocus<T, P>(this T viewModel, Expression<Func<T, P>> expression) where T : BizViewModel
        {
            var view = viewModel.View as BizView;
            if (view == null)
                return false;

            var path = expression.ToSymbol();
            foreach (var each in view.BindingBehaviors.OfType<DependencyPropertyBehavior>())
            {
                var expr = each.BindingExpression as BindingExpression;
                if (expr.ParentBinding.Path.Path == path)
                {
                    var control = each.Element as Control;
                    if (control != null)
                    {
                        return control.Focus();
                    }
                }
            }
            return false;
        }
    }
}
