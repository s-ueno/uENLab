using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using uEN.UI.Binding;

namespace uEN.UI
{
    public abstract class BizView : UserControl
    {
        protected BizView()
        {
            DataContextChanged += OnBizViewDataContextChanged;
        }
        private void OnBizViewDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            BuildBinding();
            if (BindingBehaviors != null)
            {
                foreach (var each in BindingBehaviors)
                {
                    each.Ensure();
                }
            }
        }
        protected virtual void BuildBinding()
        {

        }


        public IList<IBindingBehavior> BindingBehaviors { get; set; }

        protected virtual BindingBehaviorBuilder<T> CreateBindingBuilder<T>() where T : BizViewModel
        {
            if (BindingBehaviors == null)
                BindingBehaviors = new List<IBindingBehavior>();

            return new BindingBehaviorBuilder<T>(this);
        }


    }
}
