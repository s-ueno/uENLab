using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uEN.Core;

namespace uEN.UI.Controls
{

    public enum ListContentHeaderStyle
    {
        Horizontal, Vertical
    }

    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:uEN.UI.Controls"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:uEN.UI.Controls;assembly=uEN.UI.Controls"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     MyNamespace:ListContent
    ///
    /// </summary>
    public class ListContent : Selector
    {
        static ListContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListContent),
                new FrameworkPropertyMetadata(typeof(ListContent)));
        }

        #region HeaderStyle

        public static ListContentHeaderStyle GetHeaderStyle(DependencyObject obj)
        {
            return (ListContentHeaderStyle)obj.GetValue(HeaderStyleProperty);
        }
        public static void SetHeaderStyle(DependencyObject obj, ListContentHeaderStyle value)
        {
            obj.SetValue(HeaderStyleProperty, value);
        }
        public static readonly DependencyProperty HeaderStyleProperty =
            DependencyProperty.RegisterAttached("HeaderStyle", typeof(ListContentHeaderStyle), typeof(ListContent), new UIPropertyMetadata(ListContentHeaderStyle.Horizontal));

        #endregion

        #region BorderVisibility

        public static Visibility GetBorderVisibility(DependencyObject obj)
        {
            return (Visibility)obj.GetValue(BorderVisibilityProperty);
        }

        public static void SetBorderVisibility(DependencyObject obj, Visibility value)
        {
            obj.SetValue(BorderVisibilityProperty, value);
        }
        public static readonly DependencyProperty BorderVisibilityProperty =
            DependencyProperty.RegisterAttached("BorderVisibility", typeof(Visibility), typeof(ListContent), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TitleContent = Template.FindName("TitleContent", this) as ListBox;
        }

        public ListBox TitleContent
        {
            get { return titleContent; }
            set
            {
                titleContent = value;
                if (value != null)
                {
                    var notifiable = titleContent.Items as INotifyCollectionChanged;
                    if (notifiable != null)
                    {
                        notifiable.CollectionChanged -= notifiable_CollectionChanged;
                        notifiable.CollectionChanged += notifiable_CollectionChanged;
                    }
                }
            }
        }
        private ListBox titleContent;
        void notifiable_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var count = TitleContent.Items != null ? TitleContent.Items.Count : 0;
            if (count != 0)
            {
                var listboxItems = TitleContent.FindVisualChildren<ListBoxItem>().ToArray();
                for (int i = 0; i < listboxItems.Length; i++)
                {
                    var item = listboxItems[i];
                    item.Triggers.Clear();

                    var loadedTrigger = new EventTrigger(FrameworkElement.LoadedEvent);
                    loadedTrigger.Actions.Add(new BeginStoryboard() { Storyboard = LoadedStoryboard(i, item) });
                    item.Triggers.Add(loadedTrigger);
                }
            }
        }

        static Storyboard LoadedStoryboard(int d, FrameworkElement element)
        {
            var storyboard = new Storyboard();
            var slideAnimation = new ThicknessAnimation();
            var headerStyle = GetHeaderStyle(element.FindVisualParent<ListContent>());

            if (headerStyle == ListContentHeaderStyle.Horizontal)
            {
                element.Margin = new Thickness(0, 55, 20, -55);
                slideAnimation.To = new Thickness(0, 5, 20, 5);
                slideAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.7));
            }
            else
            {
                element.Margin = new Thickness(200, 0, -200, 10);
                slideAnimation.To = new Thickness(10, 0, 10, 5);
                slideAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            }
            slideAnimation.EasingFunction = new BackEase() { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 };
            slideAnimation.BeginTime = TimeSpan.FromMilliseconds(d * 100);
            
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath(FrameworkElement.MarginProperty));
            storyboard.Children.Add(slideAnimation);

            var opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = 0;
            opacityAnimation.To = 1;
            opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            slideAnimation.BeginTime = TimeSpan.FromMilliseconds(d * 100);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));
            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }
    }
}
