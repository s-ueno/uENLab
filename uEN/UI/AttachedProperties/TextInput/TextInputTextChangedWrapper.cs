using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace uEN.UI.AttachedProperties
{
    /// <summary>IME変換中を除いてテキストの変更を通知する機能を提供します</summary>
    public class TextInputTextChangedWrapper
    {
        public static TextInputTextChangedWrapper GetValue(DependencyObject obj)
        {
            return (TextInputTextChangedWrapper)obj.GetValue(ValueProperty);
        }
        public static void SetValue(DependencyObject obj, TextInputTextChangedWrapper value)
        {
            obj.SetValue(ValueProperty, value);
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(TextInputTextChangedWrapper), typeof(TextInputTextChangedWrapper),
            new UIPropertyMetadata(null, OnImeDetectorChanged));
        private static void OnImeDetectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = e.NewValue as TextInputTextChangedWrapper;
            if (me != null)
            {
                me.Register(d as TextBox);
            }
        }
        private bool engaged = false;
        private string oldText = null;
        private int oldCaretIndex = 0;
        private void Register(TextBox textbox)
        {
            engaged = false;

            TextCompositionManager.RemovePreviewTextInputHandler(textbox, OnPreviewTextInput);
            TextCompositionManager.RemovePreviewTextInputStartHandler(textbox, OnPreviewTextInputStart);
            TextCompositionManager.RemovePreviewTextInputUpdateHandler(textbox, OnPreviewTextInputUpdate);

            TextCompositionManager.AddPreviewTextInputHandler(textbox, OnPreviewTextInput);
            TextCompositionManager.AddPreviewTextInputStartHandler(textbox, OnPreviewTextInputStart);
            TextCompositionManager.AddPreviewTextInputUpdateHandler(textbox, OnPreviewTextInputUpdate);

            textbox.TextChanged -= OnTextBoxTextChanged;
            textbox.TextChanged += OnTextBoxTextChanged;
        }
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            engaged = false;
        }
        private void OnPreviewTextInputStart(object sender, TextCompositionEventArgs e)
        {
            engaged = true;
        }
        private void OnPreviewTextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            if (e.TextComposition.CompositionText.Length == 0)
                engaged = false;
        }
        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (engaged) return;
            var textBox = (TextBox)sender;

            textBox.RaiseEvent(
                new RoutedPropertyChangedEventArgs<TextInputTextChangedEventArgs>(null,
                    new TextInputTextChangedEventArgs(TextChangedEvent, oldText, oldCaretIndex), TextChangedEvent));
            oldText = textBox.Text;
            oldCaretIndex = textBox.CaretIndex;
        }
        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Direct,
            typeof(RoutedPropertyChangedEventHandler<TextInputTextChangedEventArgs>), typeof(TextInputTextChangedWrapper));
    }

    public class TextInputTextChangedEventArgs : RoutedEventArgs
    {
        public TextInputTextChangedEventArgs(RoutedEvent routedEvent, string oldText, int oldCaretIndex)
            : base(routedEvent)
        {
            OldText = oldText;
            OldCaretIndex = oldCaretIndex;
        }
        public string OldText { get; private set; }
        public int OldCaretIndex { get; private set; }
    }

}
