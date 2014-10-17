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
        protected virtual void OnBizViewDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            var viewModel = DataContext as BizViewModel;
            if (viewModel != null)
                viewModel.View = this;

            BindingBehaviors = new List<IBindingBehavior>();
            BuildBinding();
            foreach (var each in BindingBehaviors)
            {
                each.Ensure();
            }
        }
        protected virtual void BuildBinding()
        {

        }
        public IList<IBindingBehavior> BindingBehaviors { get; set; }

        protected virtual BindingBehaviorBuilder<T> CreateBindingBuilder<T>() where T : BizViewModel
        {
            return new BindingBehaviorBuilder<T>(this);
        }


    }
}
