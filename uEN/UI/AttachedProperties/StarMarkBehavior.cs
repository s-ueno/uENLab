using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace uEN.UI.AttachedProperties
{
    public enum StarMarkStyle
    {
        /// <summary>*</summary>
        Asterisk = 0x2a,
        /// <summary>⋆</summary>
        Star,
        /// <summary>★</summary>
        DarkStar,
        /// <summary>✰</summary>
        ShadowedStar,
        /// <summary>🌟</summary>
        GlowingStar,
    }

    internal class UIElementsLocation
    {
        public UIElementsLocation(UIElement uiElement)
        {
            Element = uiElement;
            Container = uiElement.FindVisualParent<Panel>();

            GridRow = Grid.GetRow(uiElement);
            GridRowSpan = Grid.GetRowSpan(uiElement);
            GridColumn = Grid.GetColumn(uiElement);
            GridColumnSpan = Grid.GetColumnSpan(uiElement);

            CanvasBottom = Canvas.GetBottom(uiElement);
            CanvasTop = Canvas.GetTop(uiElement);
            CanvasRight = Canvas.GetRight(uiElement);
            CanvasLeft = Canvas.GetLeft(uiElement);

            FlowDirection = Panel.GetFlowDirection(uiElement);
            ZIndex = Panel.GetZIndex(uiElement);
        }
        public UIElement Element { get; private set; }
        public Panel Container { get; private set; }

        public int GridRow { get; private set; }
        public int GridRowSpan { get; private set; }
        public int GridColumn { get; private set; }
        public int GridColumnSpan { get; private set; }

        public double CanvasBottom { get; private set; }
        public double CanvasTop { get; private set; }
        public double CanvasRight { get; private set; }
        public double CanvasLeft { get; private set; }

        public FlowDirection FlowDirection { get; private set; }
        public int ZIndex { get; private set; }

        public void SetLocation(UIElement element)
        {
            SetLocation(element, this);
        }
        public static void SetLocation(UIElement element, UIElementsLocation location)
        {
            var uiElement = location.Element;

            Grid.SetRow(uiElement, location.GridRow);
            Grid.SetRowSpan(uiElement, location.GridRowSpan);
            Grid.SetColumn(uiElement, location.GridColumn);
            Grid.SetColumnSpan(uiElement, location.GridColumnSpan);

            Canvas.SetBottom(uiElement, location.CanvasBottom);
            Canvas.SetTop(uiElement, location.CanvasTop);
            Canvas.SetRight(uiElement, location.CanvasRight);
            Canvas.SetLeft(uiElement, location.CanvasLeft);

            Panel.SetFlowDirection(uiElement, location.FlowDirection);
            Panel.SetZIndex(uiElement, location.ZIndex);
        }
    }

    public class StarMarkBehavior
    {
        public static StarMarkStyle GetStarMarkStyle(DependencyObject obj)
        {
            return (StarMarkStyle)obj.GetValue(StarMarkStyleProperty);
        }

        public static void SetStarMarkStyle(DependencyObject obj, StarMarkStyle value)
        {
            obj.SetValue(StarMarkStyleProperty, value);
        }
        public static readonly DependencyProperty StarMarkStyleProperty =
            DependencyProperty.RegisterAttached("StarMarkStyle", typeof(StarMarkStyle?), typeof(StarMarkBehavior),
            new FrameworkPropertyMetadata(null, OnStarMarkStyleChanged));

        public static Brush GetForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForegroundProperty, value);
        }
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(StarMarkBehavior)
            , new FrameworkPropertyMetadata(Brushes.Red, OnForegroundChanged));
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = e.NewValue as Brush;
            if (brush == null) return;

            var mark = GetMark(d);
            if (mark != null)
                mark.Foreground = brush;
        }

        public static object GetToolTip(DependencyObject obj)
        {
            return obj.GetValue(ToolTipProperty);
        }
        public static void SetToolTip(DependencyObject obj, object value)
        {
            obj.SetValue(ToolTipProperty, value);
        }

        public static readonly DependencyProperty ToolTipProperty =
            DependencyProperty.RegisterAttached("ToolTip", typeof(object), typeof(StarMarkBehavior),
            new FrameworkPropertyMetadata("必須入力の項目です", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnToolTipStringChanged));
        private static void OnToolTipStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mark = GetMark(d);
            if (mark != null)
                mark.ToolTip = e.NewValue;
        }

        private static void OnStarMarkStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var style = e.NewValue as StarMarkStyle?;
            if (!style.HasValue) return;

            var uiElement = d as FrameworkElement;
            if (uiElement == null) return;

            var panel = uiElement.FindVisualParent<Panel>();
            if (panel == null) return;

            var location = new UIElementsLocation(uiElement);
            panel.Children.Remove(uiElement);

            var grid = new Grid();
            var gridBinding = new Binding()
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
                {
                    AncestorType = panel.GetType(),
                    AncestorLevel = 1
                },
                Path = new PropertyPath(FrameworkElement.ActualWidthProperty),
            };
            grid.SetBinding(FrameworkElement.WidthProperty, gridBinding);
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(uiElement);


            var uiElementBinding = new Binding()
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
                {
                    AncestorType = grid.GetType(),
                    AncestorLevel = 1
                },
                Path = new PropertyPath(FrameworkElement.ActualWidthProperty),

            };
            uiElement.SetBinding(FrameworkElement.WidthProperty, uiElementBinding);
            Grid.SetColumn(uiElement, 1);


            var textBlock = new TextBlock();
            textBlock.Foreground = GetForeground(uiElement);
            textBlock.ToolTip = GetToolTip(uiElement);
            textBlock.FontSize = 12.5;
            textBlock.FontFamily = new FontFamily("MS ゴシック");
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.Margin = new Thickness(-15, 0, 15, 0);
            switch (style.Value)
            {
                case StarMarkStyle.Star:
                    textBlock.Text = "⋆";
                    textBlock.Margin = new Thickness(-10, 0, 10, 0);
                    break;
                case StarMarkStyle.Asterisk:
                    textBlock.Text = "*";
                    textBlock.Margin = new Thickness(-10, 0, 10, 0);
                    break;
                case StarMarkStyle.DarkStar:
                    textBlock.Text = "★";
                    break;
                case StarMarkStyle.ShadowedStar:
                    textBlock.Text = "✰";
                    break;
                case StarMarkStyle.GlowingStar:
                    textBlock.Text = "🌟";
                    textBlock.FontSize = 9;
                    break;
            }
            grid.Children.Add(textBlock);
            Grid.SetColumn(textBlock, 0);

            location.SetLocation(grid);
            panel.Children.Add(grid);

            uiElement.SetValue(MarkProperty, textBlock);
        }

        private static TextBlock GetMark(DependencyObject obj)
        {
            return (TextBlock)obj.GetValue(MarkProperty);
        }
        private static void SetMark(DependencyObject obj, TextBlock value)
        {
            obj.SetValue(MarkProperty, value);
        }
        private static readonly DependencyProperty MarkProperty =
            DependencyProperty.RegisterAttached("Mark", typeof(TextBlock), typeof(StarMarkBehavior), new PropertyMetadata(null));
    }
}
