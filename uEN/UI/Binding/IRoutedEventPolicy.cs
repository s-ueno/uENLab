using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN.UI.DataBinding
{
    public interface IRoutedEventPolicy
    {
        int Priolity { get; }
        IRoutedEventPolicy NextPolicy { get; set; }
        void Action(RoutedEventBehavior behavior);
    }

    public abstract class RoutedEventPolicyAttribute : Attribute, IRoutedEventPolicy
    {
        IRoutedEventPolicy IRoutedEventPolicy.NextPolicy { get; set; }
        void IRoutedEventPolicy.Action(RoutedEventBehavior behavior)
        {
            Behavior = behavior;
            Action(() => (this as IRoutedEventPolicy).NextPolicy.Action(behavior));
        }
        protected RoutedEventBehavior Behavior { get; set; }
        public virtual int Priolity
        {
            get { return int.MaxValue - 100; }
        }
        protected abstract void Action(Action action);
    }
}
