using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using uEN.Core;
using uEN.UI.DataBinding;

namespace uEN.UI.AttachedProperties
{
    /// <summary>
    /// テキスト入力系に対して、アノテーションを利用した入力支援をします
    /// </summary>
    public class TextInputAnnotationAttribute : Attribute, IBindingAttribute
    {

        public void Binding(IBindingBehavior behavior)
        {
            var b = behavior as DependencyPropertyBehavior;
            if (b == null) return;

            var element = b.Element as TextBox;
            if (element == null) return;

            foreach (var each in b.Attributes)
            {
                var numericAnnotation = each as NumericAnnotationAttribute;
                if (numericAnnotation != null)
                {
                    element.SetCurrentValue(
                        TextBox.HorizontalContentAlignmentProperty,
                        System.Windows.HorizontalAlignment.Right);
                    var binding = b.Binding;
                    if (binding.Converter == null)
                    {
                        var memExp = b.LambdaExpression.Body as MemberExpression;
                        if (memExp != null)
                        {
                            var pi = memExp.Member as PropertyInfo;
                            if (pi != null)
                            {
                                binding.Converter = new InputToSourceTypeConverter(pi.PropertyType);
                            }
                        }
                    }
                    element.GotFocus -= element_GotFocus;
                    element.GotFocus += element_GotFocus;
                }
            }
        }

        void element_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var obj = (TextBox)sender;
            if (obj.IsReadOnly) return;

            var binding = BindingOperations.GetBinding(obj, TextBox.TextProperty);
            if (binding == null) return;

            if (binding.Mode == BindingMode.OneWay ||
                binding.Mode == BindingMode.OneTime) return;

            if (binding.Converter == null) return;

            var value = binding.Converter.ConvertBack(obj.Text, null, binding.ConverterParameter, binding.ConverterCulture);
            obj.SetCurrentValue(TextBox.TextProperty, Convert.ToString(value));
        }
    }

    /// <summary>Inputelement(ユーザー入力＝文字）をバインドソースの型へ変換する</summary>
    public class InputToSourceTypeConverter : IValueConverter
    {
        static InputToSourceTypeConverter()
        {
            TypeDescriptor.AddAttributes(typeof(decimal), new TypeConverterAttribute(typeof(CustomDecimalConverter)));
            TypeDescriptor.AddAttributes(typeof(Double), new TypeConverterAttribute(typeof(CustomDoubleConverter)));
            TypeDescriptor.AddAttributes(typeof(Single), new TypeConverterAttribute(typeof(CustomSingleConverter)));
            TypeDescriptor.AddAttributes(typeof(Int16), new TypeConverterAttribute(typeof(CustomInt16Converter)));
            TypeDescriptor.AddAttributes(typeof(Int32), new TypeConverterAttribute(typeof(CustomInt32Converter)));
            TypeDescriptor.AddAttributes(typeof(Int64), new TypeConverterAttribute(typeof(CustomInt64Converter)));
            TypeDescriptor.AddAttributes(typeof(UInt16), new TypeConverterAttribute(typeof(CustomUInt16Converter)));
            TypeDescriptor.AddAttributes(typeof(UInt32), new TypeConverterAttribute(typeof(CustomUInt32Converter)));
            TypeDescriptor.AddAttributes(typeof(UInt64), new TypeConverterAttribute(typeof(CustomUInt64Converter)));
        }

        public InputToSourceTypeConverter(Type type)
        {
            Nullable = IsNullbale(type);
            Converter = TypeDescriptor.GetConverter(GetType(type));

        }
        public TypeConverter Converter { get; private set; }
        public bool Nullable { get; private set; }

        //source ⇒ target は規定(ToString)が良い
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
        //target ⇒ source が問題で、バインディング元タイプにnullable考慮して対応
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                if (Nullable) return null;
            }
            if (value is string)
            {
                if (string.IsNullOrWhiteSpace(value as string))
                {
                    if (Nullable) return null;
                }
            }
            return Converter.ConvertFrom(value);
        }

        protected virtual Type GetType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GetGenericArguments().FirstOrDefault();
            }
            return type;
        }
        protected virtual bool IsNullbale(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }

}
