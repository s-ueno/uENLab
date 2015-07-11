using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace uEN.UI
{
    public enum NavigationSource
    {
        ListContent, Breadcrumb, Window, View
    }

    public delegate void NavigatingEventHandler(object sender, NavigatingEventArgs e);
    public class NavigatingEventArgs : RoutedEventArgs
    {
        public NavigatingEventArgs() : base() { }
        public NavigatingEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        public NavigatingEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        public NavigationSource NavigationSource { get; internal set; }
        public EventArgs SourceEventArg { get; internal set; }
        public bool Cancel { get; set; }
    }

}
