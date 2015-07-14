using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using uEN.UI.AttachedProperties;

namespace uEN.UI
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    [Export(typeof(ViewDataTemplateSelector))]
    public class ViewDataTemplateSelector : DataTemplateSelector
    {
        public ViewDataTemplateSelector()
        {
            UseViewCache = true;
            TemplatedParentWidth = true;
            TemplatedParentHeight = true;
        }
        public bool UseViewCache { get; set; }
        public bool TemplatedParentWidth { get; set; }
        public bool TemplatedParentHeight { get; set; }
        private Dictionary<WeakReference, DataTemplate> cache = new Dictionary<WeakReference, DataTemplate>();
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            if (UseViewCache)
            {
                DataTemplate dt = null;
                foreach (var keyValue in cache.ToArray())
                {
                    if (keyValue.Key.Target == item)
                    {
                        dt = keyValue.Value;
                    }
                    else if (!keyValue.Key.IsAlive)
                    {
                        cache.Remove(keyValue.Key);
                    }
                }
                if (dt != null)
                    return dt;
            }

            var vm = item as BizViewModel;
            if (vm == null || vm.VisualElements == null)
                return base.SelectTemplate(item, container);

            var template = new DataTemplate() { VisualTree = new FrameworkElementFactory(vm.VisualElements.VisualType) };

            if (Application.Current != null)
                template.VisualTree.SetValue(FrameworkElement.StyleProperty, Application.Current.TryFindResource(typeof(BizView)));

            if (TemplatedParentWidth)
                template.VisualTree.SetBinding(FrameworkElement.WidthProperty,
                    new System.Windows.Data.Binding("ActualWidth") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            if (TemplatedParentHeight)
                template.VisualTree.SetBinding(FrameworkElement.HeightProperty,
                    new System.Windows.Data.Binding("ActualHeight") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });

            template.Seal();

            return UseViewCache ? cache[new WeakReference(item)] = template : template;
        }
    }

    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    [Export(typeof(ViewModelWeakReference))]
    public class ViewModelWeakReference
    {
        protected virtual IList<WeakReference> Collection { get { return _list; } }
        private List<WeakReference> _list = new List<WeakReference>();

        public void Push(BizViewModel viewModel)
        {
            Flush();

            Collection.Add(new WeakReference(viewModel));
        }

        public void Pop(BizViewModel viewModel)
        {
            Flush();

            WeakReference weakReference = null;
            foreach (var each in Collection)
            {
                if (each.Target == viewModel)
                {
                    weakReference = each;
                    break;
                }
            }
            if (weakReference != null)
                Collection.Remove(weakReference);
        }

        public IEnumerable<WeakReference> List()
        {
            Flush();
            return Collection.ToArray();
        }
             
        public void Flush()
        {
            var items = Collection.ToArray();
            foreach (var each in items)
            {
                if (!each.IsAlive)
                {
                    Collection.Remove(each);
                }
            }
        }

    }

}
