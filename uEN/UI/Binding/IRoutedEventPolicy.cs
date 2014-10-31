using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN.UI.Binding
{
    public interface IRoutedEventPolicy
    {
        int Priolity { get; }
        IRoutedEventPolicy NextPolicy { get; set; }
        void Action(RoutedEventBehavior behavior);
    }


}
