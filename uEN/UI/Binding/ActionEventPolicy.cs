using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace uEN.UI.DataBinding
{
    public class ActionEventPolicyAttribute : Attribute, IRoutedEventPolicy
    {
        public ActionEventPolicyAttribute()
        {
            ExceptionPolicy = Repository.GetPriorityExport<IExceptionPolicy>();
        }

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
                    ExceptionPolicy.Do(behavior, ex);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
