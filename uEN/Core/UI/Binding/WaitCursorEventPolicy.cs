using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace uEN.UI.Binding
{
    public class WaitCursorEventPolicyAttribute : Attribute, IRoutedEventPolicy
    {
        public void Action(RoutedEventBehavior behavior)
        {
            var currentCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                NextPolicy.Action(behavior);
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
