using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace uEN.UI.DataBinding
{
    public class DisplayMemberPathAttribute : Attribute, IBindingAttribute
    {
        public DisplayMemberPathAttribute(string displayMemberPath)
        {
            DisplayMemberPath = displayMemberPath;
        }
        public string DisplayMemberPath { get; private set; }

        public void Binding(IBindingBehavior behavior)
        {
            var dp = behavior as DependencyPropertyBehavior;
            if (dp == null)
                return;

            var combo = dp.Element as ComboBox;
            if (combo == null)
                return;

            combo.DisplayMemberPath = DisplayMemberPath;
        }
    }
}
