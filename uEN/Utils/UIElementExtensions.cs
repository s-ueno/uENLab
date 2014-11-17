using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace uEN
{
    public static class UIElementExtensions
    {
        public static T FindParentWithVisualTree<T>(this DependencyObject target) where T : DependencyObject
        {
            if (!(target is Visual))
                return null;
            var parent = VisualTreeHelper.GetParent(target);
            while (!(parent is T) && parent != null)
                parent = VisualTreeHelper.GetParent(parent);
            return parent as T;
        }


        //http://msdn.microsoft.com/ja-jp/library/system.windows.threading.dispatcher.pushframe.aspx
        [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents(this DependencyObject target, DispatcherPriority priority = DispatcherPriority.Background)
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(priority,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;
            return null;
        }
    }
}
