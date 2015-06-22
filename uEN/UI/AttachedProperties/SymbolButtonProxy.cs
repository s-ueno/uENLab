using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace uEN.UI.AttachedProperties
{
    public class SymbolButtonProxy
    {
        #region Symbol

        public static Symbols GetSymbol(DependencyObject obj)
        {
            return (Symbols)obj.GetValue(SymbolProperty);
        }

        public static void SetSymbol(DependencyObject obj, Symbols value)
        {
            obj.SetValue(SymbolProperty, value);
        }
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.RegisterAttached("Symbol", typeof(Symbols?), typeof(SymbolButtonProxy)
            , new PropertyMetadata(null, OnSymbolChanged));
        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (button == null) return;

            SetContent(button);
        }

        private static void SetContent(Button button)
        {
            var symbol = GetSymbol(button);
            var text = GetText(button);

            var content = new Grid();
            content.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            content.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            var symbolText = new TextBlock()
            {
                Text = Convert.ToChar(symbol).ToString(),
            };
            symbolText.SetBinding(TextBlock.ForegroundProperty, new Binding("Foreground")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Button), 1)
            });
            symbolText.SetResourceReference(TextBlock.StyleProperty, "SegoeUISymbolTextBlockKey");
            content.Children.Add(symbolText);

            content.Children.Add(new TextBlock()
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
            });

            Grid.SetColumn(content.Children[1], 1);
            button.Content = content;
        }

        #endregion

        #region Text

        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(SymbolButtonProxy)
            , new PropertyMetadata(null, OnTextChanged));
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (button == null) return;

            SetContent(button);
        }

        #endregion

        #region IsEllipseStyle

        public static bool GetIsEllipseStyle(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEllipseStyleProperty);
        }

        public static void SetIsEllipseStyle(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEllipseStyleProperty, value);
        }
        public static readonly DependencyProperty IsEllipseStyleProperty =
            DependencyProperty.RegisterAttached("IsEllipseStyle", typeof(bool), typeof(SymbolButtonProxy)
            , new PropertyMetadata(false, OnIsEllipseStyleChanged));
        private static void OnIsEllipseStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (button == null) return;

            var app = System.Windows.Application.Current;
            if (app == null) return;

            button.Style = app.TryFindResource("EllipseButtonStyle") as Style;
        }

        #endregion

    }
}
