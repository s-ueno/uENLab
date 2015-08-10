using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using uEN.UI;

namespace uEN
{
    public static class UIElementExtensions
    {
        public static bool IsParentAndChild(DependencyObject parent, DependencyObject child)
        {
            
            if (parent == null || child == null)
                return false;
            var p = VisualTreeHelper.GetParent(child);
            if (p == null)
                return false;
            if (p == parent)
                return true;
            return IsParentAndChild(parent, p);
        }

        public static T FindVisualParent<T>(this DependencyObject target) where T : DependencyObject
        {
            if (!(target is Visual))
                return null;
            var parent = VisualTreeHelper.GetParent(target);
            while (!(parent is T) && parent != null)
                parent = VisualTreeHelper.GetParent(parent);
            return parent as T;
        }
        public static IEnumerable<T> ListVisualParents<T>(this DependencyObject target, Func<T, bool> predicate) where T : DependencyObject
        {
            if (!(target is Visual))
                yield break;
            var parent = FindVisualParent<T>(target);
            if (parent == null) yield break;

            if (predicate == null || predicate(parent)) yield return parent;
            foreach (var each in ListVisualParents<T>(parent, predicate))
            {
                yield return each;
            }
        }

        public static T FindVisualParentFromPoint<T>(this UIElement target, Point point) where T : DependencyObject
        {
            var element = target.InputHitTest(point) as DependencyObject;
            if (element == null) return null;
            else if (element is T) return (T)element;
            else return element.FindVisualParent<T>();
        }

        public static IEnumerable<T> FindVisualChildrenFromPoint<T>(this UIElement target, Point point) where T : DependencyObject
        {
            var element = target.InputHitTest(point) as DependencyObject;
            if (element == null) yield break;
            else if (element is T) yield return (T)element;
            else
            {
                foreach (var each in element.FindVisualChildren<T>())
                {
                    if (each is T) yield return (T)each;
                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject target) where T : DependencyObject
        {
            if (!(target is Visual))
            {
                yield break;
            }

            var cnt = VisualTreeHelper.GetChildrenCount(target);
            for (int i = 0; i < cnt; i++)
            {
                var result = VisualTreeHelper.GetChild(target, i);
                if (result is T)
                {
                    yield return (T)result;
                }
                foreach (var each in FindVisualChildren<T>(result))
                {
                    yield return each;
                }
            }
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

        public static Size MeasureString(this string s)
        {
            var app = System.Windows.Application.Current;
            if (app == null) return new Size(0, 0);

            var textBlock = new TextBlock();
            textBlock.FontFamily = app.FindResource("AppFont") as FontFamily;
            textBlock.FontSize = (app.FindResource("AppFontSize") as double?) ?? 12d;

            var formattedText = new FormattedText(
                s,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                textBlock.FontSize,
                Brushes.Black);
            return new Size(formattedText.Width, formattedText.Height);
        }

    }
}
