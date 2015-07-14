using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using uEN.Core;
using uEN.UI.AttachedProperties;
using uEN.UI.DataBinding;

namespace uEN.UI
{
    public class MessageDialogHelper
    {
        public class Command
        {
            public Command(string caption, Action action, bool isDefaultFocus = false)
            {
                Caption = caption;
                Action = action;
                IsDefaultFocus = isDefaultFocus;
            }
            public string Caption { get; private set; }
            public Action Action { get; set; }
            public bool IsDefaultFocus { get; private set; }
        }

        public static Command GetCommandValue(DependencyObject obj)
        {
            return (Command)obj.GetValue(CommandValueProperty);
        }
        public static void SetCommandValue(DependencyObject obj, Command value)
        {
            obj.SetValue(CommandValueProperty, value);
        }
        public static readonly DependencyProperty CommandValueProperty =
            DependencyProperty.RegisterAttached("CommandValue", typeof(Command), typeof(MessageDialogHelper), new PropertyMetadata(null));

        public static MessageDialogHelper Create(Window win, DependencyObject caller)
        {
            MessageDialogHelper helper = new MessageDialogHelper();
            helper.Caller = caller;

            helper.Container = win.Template.FindName("PART_MessageContentContainer", win) as Grid;
            helper.WindowContent = win.Template.FindName("PART_WindowContent", win) as Grid;
            helper.Title = win.Template.FindName("PART_MessageTitle", win) as TextBlock;
            helper.Message = win.Template.FindName("PART_MessageDescription", win) as TextBlock;
            helper.MessageButtons = win.Template.FindName("PART_MessageButtons", win) as StackPanel;
            return helper;
        }
        private DependencyObject Caller { get; set; }
        private Grid Container { get; set; }
        private Grid WindowContent { get; set; }
        private TextBlock Title { get; set; }
        private TextBlock Message { get; set; }
        private StackPanel MessageButtons { get; set; }
        public void Show(string title, string message, params Command[] commands)
        {
            var temp = Mouse.OverrideCursor;
            try
            {
                Mouse.OverrideCursor = null;

                Title.Text = title;
                Message.Text = message;
                MessageButtons.Children.Clear();
                foreach (var each in commands)
                {
                    var button = new Button();
                    button.SetResourceReference(Button.StyleProperty, "FlatButtonStyle");
                    button.Content = each.Caption;
                    button.MinWidth = 80;
                    button.Padding = new Thickness(1);
                    button.Margin = new Thickness(10);
                    button.Click -= OnButtonClick;
                    button.Click += OnButtonClick;
                    button.SetValue(CommandValueProperty, each);
                    MessageButtons.Children.Add(button);
                    if (each.IsDefaultFocus)
                    {
                        button.Loaded += (sender, e) => (sender as Button).Focus();
                    }
                }
                if (!commands.Any(x => x.IsDefaultFocus))
                {
                    MessageButtons.Children.OfType<Button>().Last().Loaded += (sender, e) => (sender as Button).Focus();
                }

                Container.Visibility = Visibility.Visible;
                ViewTransition.Play(Container, TransitionStyle.Slide);

                wait();
            }
            finally
            {
                Mouse.OverrideCursor = temp;
            }
        }

        private void wait()
        {
            while (true)
            {
                Thread.Yield();
                UIElementExtensions.DoEvents(null);
                if (flg == true) break;
            }
        }
        bool flg = false;

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var each in MessageButtons.Children.OfType<Button>())
            {
                each.Click -= OnButtonClick;
            }
            var button = (Button)sender;
            var command = GetCommandValue(button);
            ViewTransition.Play(Container, TransitionStyle.SlideOut, () =>
            {
                Container.Visibility = Visibility.Collapsed;
                flg = true;
            });

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                if (command.Action != null)
                {
                    WaitCursorEventPolicyAttribute.Action(command.Action);
                }

            }), DispatcherPriority.Background, null);

        }
    }
}
