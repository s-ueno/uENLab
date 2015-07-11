using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using uEN.UI.AttachedProperties;
using uEN.UI.Controls;
using uEN.UI.DataBinding;

namespace uEN.UI
{
    internal class WindowNavigation : NavigationAggregator
    {
        public override void UnRegister(DependencyObject obj)
        {
            var wnd = obj as Window;
            if (wnd == null) return;

            wnd.RemoveHandler(Breadcrumb.MovingEvent, new CancelRoutedEventHandler(OnBreadcrumbMoving));
            wnd.Closing -= WindowClosing;
        }
        public override void Register(DependencyObject obj)
        {
            var wnd = obj as Window;
            if (wnd == null) return;

            wnd.AddHandler(Breadcrumb.MovingEvent, new CancelRoutedEventHandler(OnBreadcrumbMoving));
            wnd.Closing += WindowClosing;
        }
        void OnBreadcrumbMoving(object sender, CancelRoutedEventArgs e)
        {
            var breadcrumb = e.OriginalSource as Breadcrumb;
            if (breadcrumb == null) return;

            var aggregateEventArgs = new NavigatingEventArgs(NavigatingEvent, sender)
            {
                NavigationSource = NavigationSource.Breadcrumb,
                SourceEventArg = e,
            };

            var mainContent = Breadcrumb.GetMainContent(breadcrumb) as BizViewModel;
            if (mainContent != null && mainContent.View != null)
            {
                mainContent.View.RaiseEvent(aggregateEventArgs);
                if (aggregateEventArgs.Cancel)
                {
                    e.Cancel = true;
                    aggregateEventArgs.Handled = true;
                }
            }
            else
            {
                var window = sender as Window;
                var viewModel = window.Content as BizViewModel;
                if (viewModel == null) return;

                var view = viewModel.View;
                if (view == null) return;
                view.RaiseEvent(aggregateEventArgs);

                if (aggregateEventArgs.Cancel)
                {
                    e.Cancel = true;
                    aggregateEventArgs.Handled = true;
                }
            }
        }

        void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var window = sender as Window;
            var aggregateEventArgs = new NavigatingEventArgs(NavigatingEvent, sender)
            {
                NavigationSource = NavigationSource.Window,
                SourceEventArg = e,
            };
            var app = System.Windows.Application.Current;
            if (app != null)
            {
                //終了中
                if (app.ShutdownMode == ShutdownMode.OnMainWindowClose &&
                    app.MainWindow == null)
                {
                    return;
                }

                if (app.MainWindow == window &&
                    app.ShutdownMode == ShutdownMode.OnMainWindowClose)
                {
                    var priority = app.Windows.OfType<Window>().Where(x => x != window).ToList();
                    priority.Add(window);
                    foreach (var each in priority)
                    {
                        OnCancel(each, aggregateEventArgs);
                        if (aggregateEventArgs.Cancel)
                        {
                            e.Cancel = true;
                            aggregateEventArgs.Handled = true;
                            return;
                        }
                    }
                }
                else
                {
                    OnCancel(window, aggregateEventArgs);
                    if (aggregateEventArgs.Cancel)
                    {
                        e.Cancel = true;
                        aggregateEventArgs.Handled = true;
                        return;
                    }
                }
            }
            else
            {
                OnCancel(window, aggregateEventArgs);
                if (aggregateEventArgs.Cancel)
                {
                    e.Cancel = true;
                    aggregateEventArgs.Handled = true;
                    return;
                }
            }
        }

        void OnCancel(Window win, NavigatingEventArgs e)
        {
            var vmr = Repository.GetPriorityExport<ViewModelWeakReference>();
            var activeViewModels = vmr.List().Select(x => x.Target).OfType<BizViewModel>();
            foreach (var each in activeViewModels)
            {
                var view = each.View;
                if (view == null) continue;
                if (!view.IsLoaded) continue;

                var window = each.GetWindow();
                if (win != window) continue;

                view.RaiseEvent(e);
                if (e.Cancel)
                {
                    e.Cancel = true;
                    e.Handled = true;
                    return;
                }
            }
        }

        bool IsCancel(Window win, NavigatingEventArgs e)
        {
            var vmr = Repository.GetPriorityExport<ViewModelWeakReference>();
            var activeViewModels = vmr.List().Select(x => x.Target).OfType<BizViewModel>();
            foreach (var each in activeViewModels)
            {


                var view = each.View;
                if (view == null) continue;
                if (!view.IsLoaded) continue;

                var window = each.GetWindow();
                if (win != window) continue;

                view.RaiseEvent(e);
                if (e.Cancel)
                {
                    e.Cancel = true;
                    e.Handled = true;
                    return true;
                }
            }
            return false;
        }


    }
}
