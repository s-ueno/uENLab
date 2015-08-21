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
                        var ret = control.Focus();
                        if (ret == false && control is ContentPresenter)
                        {
                            var content = (control as ContentPresenter).Content as UIElement;
                            if (content != null)
                                ret = content.Focus();
                        }
                        return ret;
                    }
                }
            }
            return false;
        }
        public static bool TryFocus<T>(this T viewModel, Expression<Func<T, Action>> expression) where T : BizViewModel
        {
            var view = viewModel.View as BizView;
            if (view == null)
                return false;

            var mi = expression.GetMethodInfo();
            foreach (var each in view.BindingBehaviors.OfType<RoutedEventBehavior>())
            {
                var methodInfo = each.LambdaExpression.GetMethodInfo();

                if (mi.Name == methodInfo.Name)
                {
                    var control = each.Element as UIElement;
                    if (control != null)
                        return control.Focus();
                }
            }
            return false;
        }

        public static void ShowOk<T>(this T vm, string title, string message, Action action) where T : BizViewModel
        {
            ShowMessage(vm, title, message, new MessageDialogHelper.Command("OK", null));
            (action ?? new Action(() => { }))();
        }

        public static void ShowYesNo<T>(this T vm, string title, string message, Action yesAction, Action noAction = null) where T : BizViewModel
        {
            var ret = false;
            ShowMessage(vm, title, message,
                new MessageDialogHelper.Command("Yes", () => ret = true),
                new MessageDialogHelper.Command("No", () => ret = false, true));

            if (ret)
            {
                Action action = yesAction ?? new Action(() => { });
                action();
            }
            else
            {
                Action action = noAction ?? new Action(() => { });
                action();
            }
        }
        public static void ShowYesNoCancel<T>(this T vm, string title, string message, Action yesAction, Action noAction = null, Action cancelAction = null) where T : BizViewModel
        {
            bool? ret = null;

            ShowMessage(vm, title, message,
                new MessageDialogHelper.Command("Yes", () => ret = true),
                new MessageDialogHelper.Command("No", () => ret = false),
                new MessageDialogHelper.Command("Cancel", () => ret = null, true)
                );
            if (ret == true)
            {
                Action action = yesAction ?? new Action(() => { });
                action();
            }
            else if (ret == false)
            {
                Action action = noAction ?? new Action(() => { });
                action();
            }
            else
            {
                Action action = cancelAction ?? new Action(() => { });
                action();
            }

        }
        public static void ShowMessage<T>(this T vm, string title, string message,
            params MessageDialogHelper.Command[] commands) where T : BizViewModel
        {
            var win = vm.GetWindow();
            if (win == null) return;

            var helper = MessageDialogHelper.Create(vm.GetWindow(), vm.View);
            if (helper == null) return;

            if (win.WindowState == WindowState.Minimized)
            {
                win.WindowState = WindowState.Normal;
            }

            var backup = win.Topmost;
            try
            {
                win.Topmost = true;
                setChildWindowEnable(win, false);
                if (!win.IsActive)
                    win.Activate();
                helper.Show(title, message, commands);
            }
            finally
            {
                win.Topmost = backup;
                setChildWindowEnable(win, true);
            }
        }
        private static void setChildWindowEnable(Window win, bool enable)
        {
            if (System.Windows.Application.Current != null)
            {
                if (win != System.Windows.Application.Current.MainWindow)
                {
                    var vmr = Repository.GetPriorityExport<ViewModelWeakReference>();
                    var list = vmr.List();
                    foreach (var each in list)
                    {
                        var viewModel = each.Target as BizViewModel;
                        if (viewModel == null) continue;

                        var view = viewModel.View as BizView;
                        if (view == null) continue;

                        var wnd = viewModel.GetWindow();
                        if (wnd == null) continue;

                        if (win != wnd)
                        {
                            wnd.IsEnabled = enable;
                        }
                    }
                }
            }
        }
    }
}
