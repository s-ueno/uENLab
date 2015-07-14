using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace uEN.UI
{
    public delegate void GeneralRoutedEventHandler(object sender, GeneralRoutedEventArgs e);
    public class GeneralRoutedEventArgs : RoutedEventArgs
    {
        public GeneralRoutedEventArgs(object value)
        {
            this.Value = value;
        }

        public object Value { get; set; }
    }
}
