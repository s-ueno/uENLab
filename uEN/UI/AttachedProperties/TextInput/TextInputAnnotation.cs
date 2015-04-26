using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
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
                }
            }
        }
    }



}
