using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace uEN.UI.AttachedProperties
{

    public class TreeViewBehavior : FrameworkElement
    {
        #region Behavior

        public static TreeViewBehavior GetBehavior(DependencyObject obj)
        {
            return (TreeViewBehavior)obj.GetValue(BehaviorProperty);
        }
        public static void SetBehavior(DependencyObject obj, TreeViewBehavior value)
        {
            obj.SetValue(BehaviorProperty, value);
        }
        public static readonly DependencyProperty BehaviorProperty =
            DependencyProperty.RegisterAttached("Behavior", typeof(TreeViewBehavior), typeof(TreeViewBehavior)
                , new FrameworkPropertyMetadata(null, OnChangedBehavior));
        static void OnChangedBehavior(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as TreeViewBehavior;
            if (oldValue != null) oldValue.UnRegister();


            var tree = d as TreeView;
            if (tree == null) return;


            var newValue = e.NewValue as TreeViewBehavior;
            if (newValue == null) return;


            if (newValue != null) newValue.Register(tree);
        }

        #endregion

        #region CurrentItem

        public static object GetCurrentItem(DependencyObject obj)
        {
            return (object)obj.GetValue(CurrentItemProperty);
        }
        public static void SetCurrentItem(DependencyObject obj, object value)
        {
            obj.SetValue(CurrentItemProperty, value);
        }
        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.RegisterAttached("CurrentItem", typeof(object), typeof(TreeViewBehavior),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                    OnCurrentItemChanged));
        static void OnCurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = GetBehavior(d);
            if (me == null) return;
            me.OnCurrentItemChanged(e.OldValue, e.NewValue);
        }
        void OnCurrentItemChanged(object oldValue, object newValue)
        {
            if (engaged) return;
            if (Target == null || Target.ItemContainerGenerator == null) return;

            var treeItem = Find(Target, newValue);
            if (treeItem != null) treeItem.IsSelected = true;
        }
        public static TreeViewItem Find(TreeView tree, object value)
        {
            foreach (var each in tree.Items)
            {
                var item = tree.ItemContainerGenerator.ContainerFromItem(each) as TreeViewItem;
                //item.ExpandSubtree();
                var ret = findChildren(item, value);
                if (ret != null)
                {
                    return ret; ;
                }
            }
            return null;
        }
        static TreeViewItem findChildren(TreeViewItem item, object target)
        {
            if (item == null) return null;
            if (item.DataContext == target) return item;
            foreach (var each in item.Items)
            {
                var child = item.ItemContainerGenerator.ContainerFromItem(each) as TreeViewItem;
                var ret = findChildren(child, target);
                if (ret != null)
                {
                    return ret;
                }
            }
            return null;
        }

        #endregion

        protected TreeView Target { get; set; }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            UnRegister();
        }
        public void UnRegister()
        {
            if (Target == null) return;

            Target.SelectedItemChanged -= OnSelectedItemChanged;
            Target.PreviewMouseRightButtonUp -= OnPreviewMouseRightButtonUpForApplyDisplayingContextMenu;

            Target.Unloaded -= OnUnloaded;

            Target = null;
        }
        public void Register(TreeView obj)
        {
            Target = obj;

            Target.SelectedItemChanged -= OnSelectedItemChanged;
            Target.SelectedItemChanged += OnSelectedItemChanged;

            Target.PreviewMouseRightButtonUp -= OnPreviewMouseRightButtonUpForApplyDisplayingContextMenu;
            Target.PreviewMouseRightButtonUp += OnPreviewMouseRightButtonUpForApplyDisplayingContextMenu;

            Target.Unloaded -= OnUnloaded;
            Target.Unloaded += OnUnloaded;
        }


        public static readonly RoutedEvent SelectionChangingEvent =
            SelectionChangingBehavior.SelectionChangingEvent.AddOwner(typeof(TreeViewBehavior));
        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (engaged) return;

            var treeView = sender as TreeView;
            if (treeView == null) return;


            var oldTreeItem = Find(treeView, e.OldValue);
            var newTreeItem = Find(treeView, e.NewValue);


            var se = new SelectionChangingEventargs(e.OldValue, e.NewValue) { RoutedEvent = SelectionChangingEvent };
            treeView.RaiseEvent(se);
            if (se.Cancel)
            {
                engaged = true;
                treeView.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        if (oldTreeItem != null)
                        {
                            oldTreeItem.IsSelected = true;
                        }
                    }
                    finally
                    {
                        engaged = false;
                    }
                }), DispatcherPriority.Background, null);
                return;
            }

            SetCurrentItem(treeView, e.NewValue);
            //engaged = true;
            //try
            //{
            //    SetCurrentItem(treeView, e.NewValue);
            //}
            //finally
            //{
            //    engaged = false;
            //}
        }
        bool engaged = false;


        #region ContextMenuHelper

        public static bool GetApplyDisplayingContextMenuWhenSelectedNode(DependencyObject obj)
        {
            return (bool)obj.GetValue(ApplyDisplayingContextMenuWhenSelectedNodeProperty);
        }
        public static void SetApplyDisplayingContextMenuWhenSelectedNode(DependencyObject obj, bool value)
        {
            obj.SetValue(ApplyDisplayingContextMenuWhenSelectedNodeProperty, value);
        }
        public static readonly DependencyProperty ApplyDisplayingContextMenuWhenSelectedNodeProperty =
            DependencyProperty.RegisterAttached("ApplyDisplayingContextMenuWhenSelectedNode", typeof(bool), typeof(TreeViewBehavior),
                new FrameworkPropertyMetadata(false));

        private void OnPreviewMouseRightButtonUpForApplyDisplayingContextMenu(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tree = sender as TreeView;
            if (tree == null) return;
            if (tree.ContextMenu == null) return;

            var behavior = GetBehavior(tree);
            if (behavior == null) return;

            var allowApply = GetApplyDisplayingContextMenuWhenSelectedNode(this);
            if (!allowApply) return;

            var source = e.OriginalSource as DependencyObject;
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            var item = source as TreeViewItem;

            if (item == null)
            {
                e.Handled = true;
            }
            else
            {
                item.Focus();
                item.IsSelected = true;
            }

        }

        #endregion

    }
}
