using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace uEN.UI
{
    public class CancelRoutedEventArgs : RoutedEventArgs
    {
        public CancelRoutedEventArgs() : base() { }
        public CancelRoutedEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        public CancelRoutedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        public bool Cancel { get; set; }
    }

    public delegate void CancelRoutedEventHandler(object sender, CancelRoutedEventArgs e);

}
