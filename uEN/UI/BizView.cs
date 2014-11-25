using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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
            if (viewModel == null)
            {
                UpdateSource();
                return;
            }

            viewModel.View = this;
            BindingBehaviors = new BindingBehaviorCollection();
            BuildBinding();
            foreach (var each in BindingBehaviors)
            {
                each.Ensure();
            }
            viewModel.ApplyView();
            this.Loaded -= BizView_Loaded;
            this.Loaded += BizView_Loaded;
        }

        void BizView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as BizViewModel;
            if (viewModel != null)
            {
                viewModel.LoadedView();
                viewModel.Initialized = true;
            }
        }
        protected virtual void BuildBinding()
        {

        }
        public BindingBehaviorCollection BindingBehaviors { get; set; }

        protected virtual BindingBehaviorBuilder<T> CreateBindingBuilder<T>() where T : BizViewModel
        {
            return new BindingBehaviorBuilder<T>(this);
        }

        public virtual IEnumerable<DependencyPropertyBehavior> UpdateSource(string groupRegion = null)
        {
            var list = BindingBehaviors.ListBehaviors<DependencyPropertyBehavior>(groupRegion);
            foreach (var each in list)
            {
                each.UpdateSource();
            }
            return list;
        }
        public virtual IEnumerable<DependencyPropertyBehavior> UpdateTarget(string groupRegion = null)
        {
            var list = BindingBehaviors.ListBehaviors<DependencyPropertyBehavior>(groupRegion);
            foreach (var each in list)
            {
                each.UpdateTarget();
            }
            return list;
        }

        public virtual void ThrowValidationError(string groupRegion = null)
        {
            var list = UpdateSource(groupRegion);
            var errors = BindingBehaviors.ListValidationErrors(list);
            var firstError = errors.FirstOrDefault();
            if (firstError != null)
            {
                var errorBinding = firstError.BindingInError as BindingExpressionBase;
                var uiElements = errorBinding.Target as UIElement;
                if (uiElements != null)
                {
                    uiElements.Focus();
                }
                throw new BizApplicationException(Convert.ToString(firstError.ErrorContent));
            }

        }


    }
}
