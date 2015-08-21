using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace uEN.UI.AttachedProperties
{
    public class ScrollIntoViewProxy
    {
        public static ScrollIntoViewProxy GetInstance(DependencyObject obj)
        {
            return (ScrollIntoViewProxy)obj.GetValue(InstanceProperty);
        }
        public static void SetInstance(DependencyObject obj, ScrollIntoViewProxy value)
        {
            obj.SetValue(InstanceProperty, value);
        }
        public static readonly DependencyProperty InstanceProperty =
            DependencyProperty.RegisterAttached("Instance", typeof(ScrollIntoViewProxy), typeof(ScrollIntoViewProxy),
            new FrameworkPropertyMetadata(null, OnInstanceChanged));
        private static void OnInstanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = e.NewValue as ScrollIntoViewProxy;
            if (me != null)
                me.Register(d);
        }
        private void Register(DependencyObject d)
        {
            var selector = d as Selector;
            if (selector != null)
            {
                selector.SelectionChanged -= OnSelectionChanged;
                selector.SelectionChanged += OnSelectionChanged;
            }
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (changing) return;
            if (sender != e.OriginalSource) return;
            try
            {
                changing = true;
                OnSelectionChangedRaw(sender);
            }
            finally
            {
                changing = false;
            }
        }
        [ThreadStatic]
        volatile static bool changing = false;

        private static void OnSelectionChangedRaw(object sender)
        {
            var selector = sender as Selector;
            if (selector == null) return;

            var selectedItem = selector.SelectedItem;
            if (selectedItem == null) return;

            var listBox = sender as ListBox;
            if (listBox != null)
            {
                try
                {
                    listBox.ScrollIntoView(listBox.Items[listBox.Items.Count - 1]);
                    listBox.ScrollIntoView(selectedItem);
                }
                catch
                {
                }
                return;
            }

            var dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                try
                {
                    if (dataGrid.SelectedIndex < 0) return;

                    //dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
                    dataGrid.ScrollIntoView(selectedItem);
                    dataGrid.UpdateLayout();

                    var row = dataGrid.ItemContainerGenerator.ContainerFromItem(selectedItem) as DataGridRow;
                    if (row != null)
                    {
                        if (dataGrid.SelectionUnit == DataGridSelectionUnit.FullRow)
                        {
                            row.IsSelected = true;
                            //dataGrid.DoEvents(DispatcherPriority.Loaded);
                        }
                        else
                        {
                            var presenter = row.FindVisualChildren<DataGridCellsPresenter>().FirstOrDefault();
                            if (presenter != null)
                            {
                                var cell = presenter.ItemContainerGenerator.ContainerFromIndex(0) as DataGridCell;
                                if (cell != null)
                                {
                                    var dataGridCellInfo = new DataGridCellInfo(cell);

                                    dataGrid.SelectedCells.Clear();
                                    dataGrid.SelectedCells.Add(dataGridCellInfo);
                                    dataGrid.CurrentCell = dataGridCellInfo;
                                    //dataGrid.DoEvents(DispatcherPriority.Loaded);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                return;
            }
        }
    }
}
