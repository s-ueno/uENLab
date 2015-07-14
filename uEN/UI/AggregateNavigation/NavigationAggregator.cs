using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using uEN.UI.AttachedProperties;
using uEN.UI.Controls;
using uEN.UI.DataBinding;

namespace uEN.UI
{
    public class NavigationAggregatorAttribute : Attribute, IBindingAttribute
    {
        public void Binding(IBindingBehavior behavior)
        {
            var listContent = behavior.Element as ListContent;
            if (listContent != null)
            {
                var navi = NavigationAggregator.GetInstance(listContent);
                if (navi == null)
                    NavigationAggregator.SetInstance(listContent, new ListContentNavigation());
            }

            var viewModel = behavior.ViewModel as BizViewModel;
            if (viewModel != null)
            {
                var view = viewModel.View;
                if (view != null)
                {
                    if (viewModel.IsWindowContent)
                    {
                        var win = viewModel.GetWindow();
                        var navi = NavigationAggregator.GetInstance(win);
                        if (navi == null)
                            NavigationAggregator.SetInstance(win, new WindowNavigation());
                    }
                }
            }
        }
    }
    public abstract class NavigationAggregator
    {
        //Navigatorの仕組み上イベントはコンテンツを子に表示するので、ソース要素へと下方にルーティング
        //を検討したが、コンポジットUIの仕組み上、複数のSubView構成で取り扱いが難しかった。
        public static readonly RoutedEvent NavigatingEvent =
            EventManager.RegisterRoutedEvent("Navigating", /*RoutingStrategy.Tunnel*/ RoutingStrategy.Direct,
            typeof(NavigatingEventHandler), typeof(NavigationAggregator));

        public static NavigationAggregator GetInstance(DependencyObject obj)
        {
            return (NavigationAggregator)obj.GetValue(InstanceProperty);
        }

        public static void SetInstance(DependencyObject obj, NavigationAggregator value)
        {
            obj.SetValue(InstanceProperty, value);
        }
        public static readonly DependencyProperty InstanceProperty =
            DependencyProperty.RegisterAttached("Instance", typeof(NavigationAggregator), typeof(NavigationAggregator)
            , new FrameworkPropertyMetadata(null, OnInstanceChanged));
        private static void OnInstanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as NavigationAggregator;
            if (oldValue != null)
            {
                oldValue.UnRegister(d);
            }
            var newValue = e.NewValue as NavigationAggregator;
            if (newValue != null)
            {
                newValue.Register(d);
            }
        }
        public abstract void UnRegister(DependencyObject obj);
        public abstract void Register(DependencyObject obj);
    }
}
