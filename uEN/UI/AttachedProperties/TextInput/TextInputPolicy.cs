using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using uEN.UI.DataBinding;

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
    }

    /// <summary>テキスト入力の支援をします</summary>
    public class TextInputPolicyAttribute : Attribute, IBindingAttribute
    {
        public TextInputPolicyAttribute(TextInputState state)
        {
            State = state;
        }
        public TextInputState State { get; private set; }

        public void Binding(IBindingBehavior behavior)
        {
            var b = behavior as DependencyPropertyBehavior;
            if (b == null) return;

            var element = b.Element as FrameworkElement;
            if (element != null)
            {
                element.GotFocus -= OnSetInputMethod;
                element.GotFocus += OnSetInputMethod;
            }
        }
        void OnSetInputMethod(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                SetInputMethod(textBox);
                return;
            }

            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var tb = comboBox.FindVisualChildren<TextBox>().FirstOrDefault();
                if (tb != null)
                    SetInputMethod(tb);
                return;
            }
        }
        private void SetInputMethod(DependencyObject element)
        {
            switch (State)
            {
                case TextInputState.None:
                    InputMethod.SetPreferredImeState(element, InputMethodState.DoNotCare);
                    break;
                case TextInputState.Disable:
                    InputMethod.SetIsInputMethodSuspended(element, true);
                    break;
                case TextInputState.Off:
                    InputMethod.SetPreferredImeState(element, InputMethodState.Off);
                    break;
                case TextInputState.On:
                    InputMethod.SetPreferredImeState(element, InputMethodState.On);
                    break;
                case TextInputState.Hiragana:
                    InputMethod.SetPreferredImeState(element, InputMethodState.On);
                    InputMethod.SetPreferredImeConversionMode(element,
                        ImeConversionModeValues.FullShape | ImeConversionModeValues.Native);
                    break;
                case TextInputState.Katakana:
                    InputMethod.SetPreferredImeState(element, InputMethodState.On);
                    InputMethod.SetPreferredImeConversionMode(element, 
                        ImeConversionModeValues.FullShape | ImeConversionModeValues.Katakana | ImeConversionModeValues.Native);
                    break;
                case TextInputState.KatakanaHalf:
                    InputMethod.SetPreferredImeState(element, InputMethodState.On);
                    InputMethod.SetPreferredImeConversionMode(element, ImeConversionModeValues.Native);
                    break;
                case TextInputState.Alphanumeric:
                    InputMethod.SetIsInputMethodSuspended(element, true);
                    break;
                case TextInputState.AlphaFull:
                    InputMethod.SetPreferredImeState(element, InputMethodState.On);
                    InputMethod.SetPreferredImeConversionMode(element,
                        ImeConversionModeValues.FullShape | ImeConversionModeValues.Alphanumeric);
                    break;
                default:
                    break;
            }
        }

 

    }
}
