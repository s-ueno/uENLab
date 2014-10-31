using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace uEN.UI
{
    public class ViewDataTemplateSelector : DataTemplateSelector
    {
        public ViewDataTemplateSelector()
        {
            UseViewCache = false;
            TemplatedParentWidth = true;
            TemplatedParentHeight = true;
        }
        public bool UseViewCache { get; set; }
        public bool TemplatedParentWidth { get; set; }
        public bool TemplatedParentHeight { get; set; }
        private Dictionary<Type, DataTemplate> cache = new Dictionary<Type, DataTemplate>();
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;
            var t = item.GetType();
            if (UseViewCache && cache.ContainsKey(t))
            {
                return cache[t];
            }

            var vm = item as BizViewModel;
            if (vm == null || vm.VisualElements == null)
                return base.SelectTemplate(item, container);

            var template = new DataTemplate() { VisualTree = new FrameworkElementFactory(vm.VisualElements.VisualType) };
            if (TemplatedParentWidth)
            {
                template.VisualTree.SetBinding(FrameworkElement.WidthProperty,
                    new System.Windows.Data.Binding("ActualWidth") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            }
            if (TemplatedParentHeight)
            {
                template.VisualTree.SetBinding(FrameworkElement.HeightProperty,
                    new System.Windows.Data.Binding("ActualHeight") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            }
            template.Seal();
            return cache[t] = template;
        }
    }
}
