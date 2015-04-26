using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace uEN.UI.DataBinding
{
    public class WaitCursorEventPolicyAttribute : Attribute, IRoutedEventPolicy
    {
        public void Action(RoutedEventBehavior behavior)
        {
            Action(() => NextPolicy.Action(behavior));
        }

        public static void Action(Action action)
        {
            var currentCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                action();
            }
            finally
            {
                Mouse.OverrideCursor = currentCursor;
            }
        }

        public IRoutedEventPolicy NextPolicy { get; set; }
        public int Priolity
        {
            get { return int.MaxValue; }
        }
    }
}
