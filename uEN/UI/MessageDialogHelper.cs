using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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



        public MessageDialogHelper(DependencyObject caller)
        {
            Caller = caller;
            var win = Window.GetWindow(caller);

            Container = win.Template.FindName("PART_MessageContentContainer", win) as Grid;
            WindowContent = win.Template.FindName("PART_WindowContent", win) as Grid;
            Title = win.Template.FindName("PART_MessageTitle", win) as TextBlock;
            Message = win.Template.FindName("PART_MessageDescription", win) as TextBlock;
            MessageButtons = win.Template.FindName("PART_MessageButtons", win) as StackPanel;
        }
        private DependencyObject Caller { get; set; }
        private Grid Container { get; set; }
        private Grid WindowContent { get; set; }
        private TextBlock Title { get; set; }
        private TextBlock Message { get; set; }
        private StackPanel MessageButtons { get; set; }


        public bool IsValid { get { return Container != null; } }



        public void Show(string title, string message, params Command[] commands)
        {
            Title.Text = title;
            Message.Text = message;
            MessageButtons.Children.Clear();
            foreach (var each in commands)
            {
                var button = new Button();
                button.Style = Application.Current.Resources["FlatButtonStyle"] as Style;
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
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var command = GetCommandValue(button);

            ViewTransition.Play(Container, TransitionStyle.SlideOut, () =>
            {
                Container.Visibility = Visibility.Collapsed;
            });
            if (command.Action != null)
            {
                WaitCursorEventPolicyAttribute.Action(command.Action);
            }
        }
    }
}
