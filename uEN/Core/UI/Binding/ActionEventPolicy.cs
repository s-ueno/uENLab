using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using uEN.Utils;
namespace uEN.UI.Binding
{
    public class ActionEventPolicyAttribute : Attribute, IRoutedEventPolicy
    {
        public ActionEventPolicyAttribute()
        {
            this.Compose();
        }
        [Import(typeof(IExceptionPolicy))]
        public IExceptionPolicy ExceptionPolicy { get; set; }
        public int Priolity { get { return int.MinValue; } }
        public IRoutedEventPolicy NextPolicy { get; set; }
        public RoutedEventArgs EventArgs { get; set; }
        public Action<RoutedEventArgs> Do { get; set; }

        public void Action(RoutedEventBehavior behavior)
        {
            try
            {
                Do(EventArgs);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy != null)
                {
                    ExceptionPolicy.Do(ex);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
