using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using uEN.UI.DataBinding;

namespace uEN.UI.AttachedProperties
{
    /// <summary>
    /// テキスト入力系に対して、入力監視を行います。
    /// </summary>
    public class TextInputObserverAttribute : Attribute, IBindingAttribute
    {

        public void Binding(IBindingBehavior behavior)
        {
            var b = behavior as DependencyPropertyBehavior;
            if (b == null) return;




            var element = b.Element as TextBox;
            if (element == null) return;





            var textInputObserver = TextInputObserver.GetTextInputObserver(element);
            if (textInputObserver == null)
            {
                textInputObserver = Repository.GetPriorityExport<TextInputObserver>();
                TextInputObserver.SetTextInputObserver(element, textInputObserver);
            }
        }
    }


    /// <summary>
    /// テキスト入力を監視し、検証エラー値を入力不可とします
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    [Export(typeof(TextInputObserver))]
    public class TextInputObserver
    {
        public static TextInputObserver GetTextInputObserver(DependencyObject obj)
        {
            return (TextInputObserver)obj.GetValue(TextInputObserverProperty);
        }
        public static void SetTextInputObserver(DependencyObject obj, TextInputObserver value)
        {
            obj.SetValue(TextInputObserverProperty, value);
        }
        public static readonly DependencyProperty TextInputObserverProperty =
            DependencyProperty.RegisterAttached("TextInputObserver", typeof(TextInputObserver), typeof(TextInputObserver),
            new UIPropertyMetadata(null, OnTextInputObserverChanged));

        private static void OnTextInputObserverChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = e.NewValue as TextInputObserver;
            if (me != null)
            {
                me.Register(d);
            }
        }
        protected virtual void Register(DependencyObject d)
        {
            TextInputTextChangedWrapper.SetValue(d, new TextInputTextChangedWrapper());
            var uiElement = (UIElement)d;
            uiElement.AddHandler(TextInputTextChangedWrapper.TextChangedEvent,
                new RoutedPropertyChangedEventHandler<TextInputTextChangedEventArgs>(OnTextChanged));
        }
        protected virtual void OnTextChanged(object sender, RoutedPropertyChangedEventArgs<TextInputTextChangedEventArgs> e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            if (string.IsNullOrEmpty(textBox.Text)) return;

            var binding = textBox.GetBindingExpression(TextBox.TextProperty) as BindingExpression;
            if (binding == null) return;

            var textChangedEventArg = e.NewValue;
            if (!binding.ValidateWithoutUpdate())
            {
                textBox.SetCurrentValue(TextBox.TextProperty, textChangedEventArg.OldText);
                textBox.CaretIndex = textChangedEventArg.OldCaretIndex;
                return;
            }
        }
    }
}
