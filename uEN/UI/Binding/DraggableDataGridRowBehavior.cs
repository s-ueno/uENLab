using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using uEN.Core;

namespace uEN.UI.DataBinding
{
    public class DraggableDataGridRowBehaviorAttribute : UpdateTargetProxyAttribute
    {
        protected override void Resolve(object source, DependencyPropertyBehavior behavior)
        {
            var simpleGrid = source as ISimpleGridVisualElements;
            if (simpleGrid == null) return;

            simpleGrid.Assigned -= OnSimpleGridAssigned;
            simpleGrid.Assigned += OnSimpleGridAssigned;
        }

        private void OnSimpleGridAssigned(object sender, EventArgs e)
        {
            var simpleGrid = sender as ISimpleGridVisualElements;
            if (simpleGrid == null) return;

            var grid = simpleGrid.Grid;
            if (grid == null) return;

            var draggableBehavior = DraggableDataGridRowBehavior.GetBehavior(grid);
            if (draggableBehavior != null) return;

            DraggableDataGridRowBehavior.SetBehavior(grid, new DraggableDataGridRowBehavior());
        }
    }

    internal class DraggableDataGridRowBehavior : NotifiableImp
    {
        public static DraggableDataGridRowBehavior GetBehavior(DependencyObject obj)
        {
            return (DraggableDataGridRowBehavior)obj.GetValue(BehaviorProperty);
        }
        public static void SetBehavior(DependencyObject obj, DraggableDataGridRowBehavior value)
        {
            obj.SetValue(BehaviorProperty, value);
        }
        public static readonly DependencyProperty BehaviorProperty =
            DependencyProperty.RegisterAttached("Behavior", typeof(DraggableDataGridRowBehavior), typeof(DraggableDataGridRowBehavior)
            , new FrameworkPropertyMetadata(null, OnBehaviorChanged));
        private static void OnBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = d as DataGrid;
            if (dataGrid == null) return;

            var oldBehavior = e.OldValue as DraggableDataGridRowBehavior;
            if (oldBehavior != null)
                oldBehavior.UnRegister(dataGrid);

            var newBehavior = e.NewValue as DraggableDataGridRowBehavior;
            if (newBehavior != null)
                newBehavior.Register(dataGrid);
        }
        protected Popup Popup { get; set; }
        protected DataGrid Grid { get; set; }
        protected Panel Panel { get; set; }
        public void Register(DataGrid grid)
        {
            Grid = grid;

            Grid.BeginningEdit += OnBeginEdit;
            Grid.CellEditEnding += OnEndEdit;
            Grid.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            Grid.MouseLeftButtonUp += OnMouseLeftButtonUp;

            if (Grid.IsLoaded)
            {
                Initialize();
            }
            else
            {
                Grid.Loaded += (sender, e) =>
                {
                    Initialize();
                };
            }
        }
        private void Initialize()
        {
            Panel = Grid.FindVisualParent<Panel>();
            Panel.MouseMove += OnMouseMove;

            Popup = new Popup();
            Popup.Placement = PlacementMode.RelativePoint;
            Popup.PlacementTarget = Panel;
            Popup.AllowsTransparency = true;
        }
        public void UnRegister(DataGrid grid)
        {
            Panel.MouseMove -= OnMouseMove;

            grid.BeginningEdit -= OnBeginEdit;
            grid.CellEditEnding -= OnEndEdit;
            grid.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            grid.MouseLeftButtonUp -= OnMouseLeftButtonUp;

            Grid = null;
            Panel = null;
            Popup = null;
        }
        protected bool IsEditing { get; set; }
        protected bool IsDragging { get; set; }
        public object DraggedItem
        {
            get { return _draggedItem; }
            set { SetProperty(ref _draggedItem, value, "DraggedItem"); }
        }
        object _draggedItem;

        private void OnBeginEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            IsEditing = true;
            if (IsDragging) ResetDragDrop();
        }
        private void OnEndEdit(object sender, DataGridCellEditEndingEventArgs e)
        {
            IsEditing = false;
        }
        private void ResetDragDrop()
        {
            IsDragging = false;
            Popup.IsOpen = false;
            Grid.IsReadOnly = false;
        }
        void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsEditing) return;

            var uiElement = (UIElement)sender;
            var dataGridRow = uiElement.FindVisualParentFromPoint<DataGridRow>(e.GetPosition(Grid));

            if (dataGridRow == null || dataGridRow.IsEditing) return;

            IsDragging = true;
            DraggedItem = dataGridRow.Item;

            var drawingVisual = new DrawingVisual();
            var sourceBrush = new VisualBrush(dataGridRow);
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(sourceBrush, null,
                    new Rect(new Point(0, 0), new Point(dataGridRow.ActualWidth, dataGridRow.ActualHeight)));
            }
            var renderTarget = new RenderTargetBitmap((int)dataGridRow.ActualWidth, (int)dataGridRow.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(drawingVisual);
            var image = new Image();
            image.Opacity = 0.7;
            image.Source = renderTarget;

            var border = new Border();
            border.BorderThickness = new Thickness(1);
            border.SetResourceReference(Border.BorderBrushProperty, "AppBrandOpacity7");
            border.SetResourceReference(Border.BackgroundProperty, "AppBrandOpacity3");
            border.Child = image;

            Popup.Child = border;
        }
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!IsDragging || e.LeftButton != MouseButtonState.Pressed) return;

            if (!Popup.IsOpen)
            {
                Grid.IsReadOnly = true;
                Popup.IsOpen = true;
            }


            var popupSize = new Size(Popup.ActualWidth, Popup.ActualHeight);
            var location = e.GetPosition(Panel);
            Popup.PlacementRectangle = new Rect(new Point(location.X + 40, location.Y + 10), popupSize);


            var row = Grid.FindVisualParentFromPoint<DataGridRow>(e.GetPosition(Grid));
            if (row == null) return;

            Grid.SelectedItem = row.Item;
        }
        void OnMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!IsDragging || IsEditing)
            {
                return;
            }

            try
            {
                var iList = Grid.ItemsSource as IList;
                if (iList == null)
                {
                    var collectionView = Grid.ItemsSource as CollectionView;
                    if (collectionView != null)
                        iList = collectionView.SourceCollection as IList;
                }
                if (iList == null)
                {
                    BizUtils.TraceWarning("SourceCollection need IList.");
                    return;
                }


                var item = Grid.SelectedItem;
                if (item == null || !ReferenceEquals(DraggedItem, item))
                {
                    iList.Remove(DraggedItem);

                    var index = iList.IndexOf(item);
                    iList.Insert(index, DraggedItem);

                    Grid.Items.Refresh();
                    Grid.SelectedItem = null;
                    Grid.SelectedItem = DraggedItem;
                }
            }
            finally
            {
                ResetDragDrop();
            }
        }

    }
}
