using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using uEN.Utils;

namespace uEN.UI.AttachedProperties
{

    public enum TextInputState
    {
        None,
        Disable,
        Off,
        On,
        Hiragana,
        Katakana,
        KatakanaHalf,
        Alphanumeric,
        AlphaFull,
        Numeric,
    }

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    [Export(typeof(TextInputHelper))]
    public class TextInputHelper
    {
        public static TextInputState GetValue(DependencyObject obj)
        {
            return (TextInputState)obj.GetValue(ValueProperty);
        }
        public static void SetValue(DependencyObject obj, TextInputState value)
        {
            obj.SetValue(ValueProperty, value);
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(TextInputState), typeof(TextInputHelper),
            new UIPropertyMetadata(TextInputState.None, OnValueChanged));
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null) return;

            if (!fe.IsLoaded)
            {
                fe.Loaded += fe_Loaded;
            }
            else
            {
                fe.SetValue(HelperProperty, GetHelper(fe) ?? Repository.GetPriorityExport<TextInputHelper>());
            }
        }
        static void fe_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox != null)
            {
                textbox.SetValue(HelperProperty, Repository.GetPriorityExport<TextInputHelper>());
            }

            var list = (sender as FrameworkElement).FindVisualChildren<TextBox>();
            foreach (var each in list)
            {
                each.SetValue(HelperProperty, Repository.GetPriorityExport<TextInputHelper>());
            }

        }

        private static TextInputHelper GetHelper(DependencyObject obj)
        {
            return (TextInputHelper)obj.GetValue(HelperProperty);
        }
        private static void SetHelper(DependencyObject obj, TextInputHelper value)
        {
            obj.SetValue(HelperProperty, value);
        }
        private static readonly DependencyProperty HelperProperty =
            DependencyProperty.RegisterAttached("Helper", typeof(TextInputHelper), typeof(TextInputHelper),
            new UIPropertyMetadata(null, OnHelperChanged));
        private static void OnHelperChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var helper = e.NewValue as TextInputHelper;
            if (helper != null)
                helper.Register(d as TextBox);
        }

        protected virtual void Register(TextBox textbox)
        {
            _imeFlag = false;
            TextCompositionManager.AddPreviewTextInputHandler(textbox, OnPreviewTextInput);
            TextCompositionManager.AddPreviewTextInputStartHandler(textbox, OnPreviewTextInputStart);
            //TextCompositionManager.AddPreviewTextInputUpdateHandler(textbox, OnPreviewTextInputUpdate);

            textbox.TextChanged -= textBox1_TextChanged;
            textbox.TextChanged += textBox1_TextChanged;
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_imeFlag) return;

            Console.WriteLine((sender as TextBox).Text);
            var text = e.ToString();
            //(sender as TextBox).Text = string.Empty;
            //IMEで確定した場合のみ、ここに入る
            //Console.WriteLine(textBox1.Text);


        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            _imeFlag = false;

            Console.WriteLine((sender as TextBox).Text);
            (sender as TextBox).Text = string.Empty;
        }
        private void OnPreviewTextInputStart(object sender, TextCompositionEventArgs e)
        {
            _imeFlag = true;
        }
        private void OnPreviewTextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            if (e.TextComposition.CompositionText.Length == 0)
                _imeFlag = false;
        }
        private bool _imeFlag = false;

    }



}
