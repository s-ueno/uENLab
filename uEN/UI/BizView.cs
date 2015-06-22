using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using uEN.UI.DataBinding;

namespace uEN.UI
{
    public abstract class BizView : UserControl
    {
        protected BizView()
        {
            DataContextChanged += OnBizViewDataContextChanged;
            BindingOperations.SetBinding(this, UIElement.IsEnabledProperty, new Binding("IsEnabled"));
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

            BindingBehaviors.EnsureBinding();

            viewModel.ApplyView();

            viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            viewModel.PropertyChanged += OnViewModelPropertyChanged;

            viewModel.MessageNotify -= OnViewModelMessageNotify;
            viewModel.MessageNotify += OnViewModelMessageNotify;

            this.Loaded -= BizView_Loaded;
            this.Loaded += BizView_Loaded;
        }

        protected virtual void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        protected virtual void OnViewModelMessageNotify(object sender, MessageNotificationEventArgs e)
        {
            
        }

        void BizView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as BizViewModel;
            if (viewModel != null)
            {
                viewModel.LoadedView();
                viewModel.Initialized = true;
                viewModel.UpdateTarget();
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
                var errorBinding = firstError.BindingInError as BindingExpression;

                /* .Net 4.0
                var pi = errorBinding.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                              .FirstOrDefault(x => x.Name == "Target");
                var uiElements = pi.GetValue(errorBinding, null) as UIElement;
                */

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
