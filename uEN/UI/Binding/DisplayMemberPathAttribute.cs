using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using uEN.Core;

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

            var ctr = dp.Element as ItemsControl;
            if (ctr == null)
                return;

            ctr.DisplayMemberPath = DisplayMemberPath;
        }
    }


    public class SelectedValuePathAttribute : Attribute, IBindingAttribute
    {
        public SelectedValuePathAttribute(string valuePath)
        {
            this.ValuePath = valuePath;
        }
        public string ValuePath { get; private set; }

        public void Binding(IBindingBehavior behavior)
        {
            var dp = behavior as DependencyPropertyBehavior;
            if (dp == null)
                return;

            var ctr = dp.Element as Selector;
            if (ctr == null)
                return;

            ctr.SelectedValuePath = ValuePath;
        }
    }


}
