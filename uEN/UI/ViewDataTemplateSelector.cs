using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace uEN.UI
{
    public class ViewDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var vm = item as BizViewModel;
            if (vm == null || vm.VisualElements == null)
                return base.SelectTemplate(item, container);

            var template = new DataTemplate() { VisualTree = new FrameworkElementFactory(vm.VisualElements.VisualType) };
            template.Seal();
            return template;
        }
    }
}
