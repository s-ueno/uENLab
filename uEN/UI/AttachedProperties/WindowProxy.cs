using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace uEN.UI.AttachedProperties
{
    public enum Commands
    {
        None,
        Minimize,
        RestoreOrMaximize,
        Close,
        Setting
    }

    public class WindowProxy
    {
        #region CommandProperty

        public static Commands GetCommand(DependencyObject obj)
        {
            return (Commands)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, Commands value)
        {
            obj.SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(Commands), typeof(WindowProxy), new UIPropertyMetadata(Commands.None, OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (button == null)
                return;

            var command = button.GetValue(CommandProperty) as Commands?;
            if (command == Commands.RestoreOrMaximize)
            {
                var win = Window.GetWindow(button);
                button.Content = win.WindowState == WindowState.Maximized ? "2" : "1";
                win.StateChanged += (x, y) =>
                {
                    var w = (Window)x;
                    button.Content = w.WindowState == WindowState.Maximized ? "2" : "1";
                };
            }
            button.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnCommandActio));
        }

        private static void OnCommandActio(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var command = button.GetValue(CommandProperty) as Commands?;
            DoCommand(button, command);
        }

        private static void DoCommand(FrameworkElement sender, Commands? command)
        {
            if (!command.HasValue)
                return;

            var win = Window.GetWindow(sender);
            switch (command.Value)
            {
                case Commands.None:
                    break;
                case Commands.RestoreOrMaximize:
                    var c = sender.GetValue(ContentPresenter.ContentProperty) as string;
                    if (c == "1")
                    {
                        SystemCommands.MaximizeWindow(win);
                    }
                    else
                    {
                        SystemCommands.RestoreWindow(win);
                    }
                    break;
                case Commands.Minimize:
                    SystemCommands.MinimizeWindow(win);
                    break;

                case Commands.Close:
                    SystemCommands.CloseWindow(win);
                    break;
                case Commands.Setting:
                    break;
                default:
                    break;
            }
        }



        #endregion






    }
}
