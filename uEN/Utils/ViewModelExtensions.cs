using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using uEN.UI.DataBinding;
using uEN.Extensions;
namespace uEN.UI
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
                    var control = each.Element as UIElement;
                    if (control != null)
                    {
                        return control.Focus();
                    }
                }
            }
            return false;
        }


        public static void ShowOk<T>(this T vm, string title, string message, Action action) where T : BizViewModel
        {
            ShowMessage(vm, title, message, new MessageDialogHelper.Command("OK", action));
        }

        public static void ShowYesNo<T>(this T vm, string title, string message, Action yesAction, Action noAction) where T : BizViewModel
        {
            ShowMessage(vm, title, message,
                new MessageDialogHelper.Command("はい", yesAction),
                new MessageDialogHelper.Command("いいえ", noAction, true));
        }
        public static void ShowYesNoCancel<T>(this T vm, string title, string message, Action yesAction, Action noAction, Action cancelAction) where T : BizViewModel
        {
            ShowMessage(vm, title, message,
                new MessageDialogHelper.Command("はい", yesAction),
                new MessageDialogHelper.Command("いいえ", noAction),
                new MessageDialogHelper.Command("キャンセル", cancelAction, true)
                );
        }
        public static void ShowMessage<T>(this T vm, string title, string message, params MessageDialogHelper.Command[] commands) where T : BizViewModel
        {
            var helper = MessageDialogHelper.Create(Window.GetWindow(vm.View), vm.View);
            helper.Show(title, message, commands);
        }
    }
}
