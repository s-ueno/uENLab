using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using uEN.Utils;

namespace uEN.UI
{
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    [Export(typeof(BreadcrumbNavigator))]
    public class BreadcrumbNavigator
    {
        public static BreadcrumbNavigator GetBreadcrumbNavigator(DependencyObject obj)
        {
            return (BreadcrumbNavigator)obj.GetValue(BreadcrumbNavigatorProperty);
        }
        public static void SetBreadcrumbNavigator(DependencyObject obj, BreadcrumbNavigator value)
        {
            obj.SetValue(BreadcrumbNavigatorProperty, value);
        }
        public static readonly DependencyProperty BreadcrumbNavigatorProperty =
            DependencyProperty.RegisterAttached("BreadcrumbNavigator", typeof(BreadcrumbNavigator), typeof(BreadcrumbNavigator), new PropertyMetadata(null));

        

        public Stack<WeakReference<BizViewModel>> ViewModelStacks { get; set; }


    }
}
