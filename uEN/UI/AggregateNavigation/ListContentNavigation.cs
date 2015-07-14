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
    internal class ListContentNavigation : NavigationAggregator
    {
        public override void UnRegister(DependencyObject obj)
        {
            var listContent = obj as ListContent;
            if (listContent == null) return;

            var titleContent = listContent.TitleContent;
            if (titleContent != null)
            {
                titleContent.RemoveHandler(SelectionChangingBehavior.SelectionChangingEvent,
                    new SelectionChangingEventHandler(OnSelectionChanging));
                SelectionChangingBehavior.SetInstance(titleContent, null);
            }
            Content = null;
        }
        public override void Register(DependencyObject obj)
        {
            var listContent = obj as ListContent;
            if (listContent == null) return;

            if (listContent.IsLoaded)
            {
                RegisterRaw(listContent);
            }
            else
            {
                listContent.Loaded += (sender, e) => RegisterRaw(sender as ListContent);
            }

            Content = listContent;
        }
        protected ListContent Content { get; set; }
        private void RegisterRaw(ListContent content)
        {
            var titleContent = content.TitleContent;
            if (titleContent != null)
            {
                SelectionChangingBehavior.SetInstance(titleContent, new SelectionChangingBehavior());
                titleContent.AddHandler(SelectionChangingBehavior.SelectionChangingEvent,
                    new SelectionChangingEventHandler(OnSelectionChanging));
            }
        }
        private void OnSelectionChanging(object sender, SelectionChangingEventargs e)
        {
            var aggregateEventArgs = new NavigatingEventArgs(NavigatingEvent, Content)
            {
                NavigationSource = NavigationSource.ListContent,
                SourceEventArg = e,
            };

            var vm = e.OldItem as BizViewModel;
            if (vm == null || vm.View == null) return;
            vm.View.RaiseEvent(aggregateEventArgs);
            if (aggregateEventArgs.Cancel)
            {
                e.Cancel = true;
                aggregateEventArgs.Handled = true;
            }
        }
    }
}
