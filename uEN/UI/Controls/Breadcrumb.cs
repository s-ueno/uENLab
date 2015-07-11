using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
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
using uEN.UI.AttachedProperties;
using uEN.UI.Controls;

namespace uEN.UI.Controls
{
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
    ///     MyNamespace:Breadcrumb
    ///
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    [Export(typeof(Breadcrumb))]
    public class Breadcrumb : ItemsControl
    {

        static Breadcrumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Breadcrumb), new FrameworkPropertyMetadata(typeof(Breadcrumb)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            HomeButton = Template.FindName("HomeButton", this) as Button;
            NewWindowButton = Template.FindName("NewWindowButton", this) as Button;
        }

        #region Use

        public static bool GetUse(DependencyObject obj)
        {
            return (bool)obj.GetValue(UseProperty);
        }
        public static void SetUse(DependencyObject obj, bool value)
        {
            obj.SetValue(UseProperty, value);
        }
        public static readonly DependencyProperty UseProperty =
            DependencyProperty.RegisterAttached("Use", typeof(bool), typeof(Breadcrumb),
             new UIPropertyMetadata(false, OnUseChanged));
        private static void OnUseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue as bool? == true)
            {
                var navi = Repository.GetPriorityExport<Breadcrumb>();
                Breadcrumb.SetBreadcrumb(Window.GetWindow(d), navi);
                navi.Container = d as ContentPresenter;
            }
        }
        public ContentPresenter Container { get; private set; }

        #endregion

        #region HomeButton

        public Button HomeButton
        {
            get { return _homeButton; }
            set
            {
                _homeButton = value;
                if (value != null)
                {
                    value.Click -= OnHomeButtonClick;
                    value.Click += OnHomeButtonClick;
                }
            }
        }

        private Button _homeButton;
        private void OnHomeButtonClick(object sender, RoutedEventArgs e)
        {

            GoHome();
        }

        private bool IsCreatingNewWIndow;

        #endregion

        #region NewWindowButton

        public Button NewWindowButton
        {
            get { return _newWindowButton; }
            set
            {
                _newWindowButton = value;
                if (value != null)
                {
                    value.Click -= OnNewWindowButtonClick;
                    value.Click += OnNewWindowButtonClick;
                }
            }
        }
        private Button _newWindowButton;

        public static readonly RoutedEvent NewWindowCreationEvent = EventManager.RegisterRoutedEvent("NewWindowCreation"
            , RoutingStrategy.Bubble, typeof(GeneralRoutedEventHandler), typeof(Breadcrumb));

        void OnNewWindowButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                IsCreatingNewWIndow = true;
                var content = GetMainContent(this);
                GoBack();

                var window = new Window();

                this.RaiseEvent(new GeneralRoutedEventArgs(window) { RoutedEvent = NewWindowCreationEvent });

                var vm = content as BizViewModel;
                if (vm != null)
                {
                    vm.IsWindowContent = true;
                    vm.UpdateSource();
                }

                Keyboard.ClearFocus();

                window.SetResourceReference(Window.StyleProperty, "ChildWndowStyle");
                window.ContentTemplateSelector = Repository.GetPriorityExport<ViewDataTemplateSelector>();
                window.Content = content;

                window.Show();
            }
            finally { IsCreatingNewWIndow = false; }
        }

        #endregion

        #region Breadcrumb

        public static Breadcrumb GetBreadcrumb(DependencyObject obj)
        {
            return (Breadcrumb)obj.GetValue(BreadcrumbProperty);
        }
        public static void SetBreadcrumb(DependencyObject obj, Breadcrumb value)
        {
            obj.SetValue(BreadcrumbProperty, value);
        }
        public static readonly DependencyProperty BreadcrumbProperty =
            DependencyProperty.RegisterAttached("Breadcrumb", typeof(Breadcrumb), typeof(Breadcrumb),
            new UIPropertyMetadata(null));

        #endregion

        #region MainContent

        public static object GetMainContent(DependencyObject obj)
        {
            return obj.GetValue(MainContentProperty);
        }

        public event EventHandler Navigated;
        public static void SetMainContent(DependencyObject obj, object value)
        {
            obj.SetValue(MainContentProperty, value);
            var b = obj as Breadcrumb;
            if (b.Navigated != null)
                b.Navigated(value, EventArgs.Empty);
        }
        public static readonly DependencyProperty MainContentProperty =
            DependencyProperty.RegisterAttached("MainContent", typeof(object), typeof(Breadcrumb), new PropertyMetadata(null));

        #endregion

        public virtual void GoForward(BizViewModel viewModel, string caption = null)
        {
            if (RaiseMoving().Cancel)
                return;
            if (Container.Content != this)
            {
                var button = new Button();
                button.SetValue(MainContentProperty, Container.Content);
                button.SetResourceReference(Button.StyleProperty, "ModernButtonStyle");
                button.Content = "Home";
                button.Click += (sender, ex) => GoHome();
                Items.Add(button);

                Container.Content = this;
                ViewTransition.Play(this, TransitionStyle.Slide);
            }

            var lastItem = Items.OfType<TextBlock>().LastOrDefault();
            if (lastItem != null)
            {
                Items.Remove(lastItem);
                AddItem(CreateButton(lastItem.Text), false);
            }

            var newDescription = caption ?? viewModel.Description;
            AddItem(CreateArrowPath(), false);
            AddItem(CreateTextBlock(newDescription));
            SetMainContent(this, viewModel);
        }

        private CancelRoutedEventArgs RaiseMoving()
        {
            var e = new CancelRoutedEventArgs(MovingEvent, this);
            if (!IsCreatingNewWIndow)
                this.RaiseEvent(e);
            return e;
        }
        public static readonly RoutedEvent MovingEvent =
            EventManager.RegisterRoutedEvent("Moving", RoutingStrategy.Bubble,
            typeof(CancelRoutedEventHandler), typeof(Breadcrumb));
        public event CancelRoutedEventHandler Moving
        {
            add { AddHandler(MovingEvent, value); }
            remove { RemoveHandler(MovingEvent, value); }
        }

        protected virtual FrameworkElement CreateArrowPath()
        {
            var arrow = new Viewbox();
            arrow.Width = arrow.Height = 10;
            arrow.Margin = new Thickness(10, 0, 10, 0);
            arrow.Stretch = Stretch.Fill;

            var path = new Path();
            path.Data = Geometry.Parse("F1 M 30.0833,22.1667L 50.6665,37.6043L 50.6665,38.7918L 30.0833,53.8333L 30.0833,22.1667 Z ");
            path.SetResourceReference(Path.StyleProperty, "PathButtonStyle");
            path.SetResourceReference(Path.FillProperty, "AppBrandOpacity5");

            arrow.Child = path;
            return arrow;
        }
        protected virtual FrameworkElement CreateTextBlock(string description)
        {
            var title = new TextBlock();
            title.Text = description;
            title.Margin = new Thickness(5, 0, 5, 0);
            title.SetResourceReference(TextBlock.ForegroundProperty, "AppForeground");
            return title;
        }
        protected virtual FrameworkElement CreateButton(string description)
        {
            var button = new Button();
            button.SetValue(MainContentProperty, GetMainContent(this));
            button.SetResourceReference(Button.StyleProperty, "ModernButtonStyle");
            button.Content = description;

            button.Click += (sender, ex) =>
            {
                var vm = GetMainContent(this) as BizViewModel;
                var isHome = Items.Count == 0;

                if (RaiseMoving().Cancel)
                    return;

                var text = button.Content as string;
                var clicked = (Button)sender;
                var index = Items.IndexOf(clicked);
                foreach (var each in Items.OfType<DependencyObject>().Skip(index).ToArray())
                {
                    Items.Remove(each);
                }
                SetMainContent(this, clicked.GetValue(MainContentProperty));
                AddItem(CreateTextBlock(text));
            };
            return button;
        }

        public virtual void GoBack()
        {
            if (!Items.OfType<Button>().Any())
                return;
            var vm = GetMainContent(this) as BizViewModel;

            if (RaiseMoving().Cancel)
                return;

            var button = Items.OfType<Button>().LastOrDefault();
            if (button != null)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        public virtual void GoHome()
        {
            var homeButton = Items.OfType<Button>().FirstOrDefault();
            if (homeButton == null)
                return;
            if (RaiseMoving().Cancel)
                return;

            var vm = GetMainContent(this) as BizViewModel;
            SetMainContent(this, Container.Content = homeButton.GetValue(MainContentProperty));
            Items.Clear();
        }

        protected virtual void AddItem(FrameworkElement element, bool isSlideAnimation = true, bool isFadeAnimation = true)
        {
            if (isSlideAnimation || isFadeAnimation)
                Play(element, isSlideAnimation, isFadeAnimation);

            Items.Add(element);
        }

        private static void Play(FrameworkElement element, bool isSlideAnimation, bool isFadeAnimation)
        {
            var storyboard = new Storyboard();

            if (isSlideAnimation)
            {
                var slideAnimation = new ThicknessAnimation();
                slideAnimation.From = new Thickness(300, 0, -300, 0);
                slideAnimation.To = element.Margin;
                slideAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.7));
                slideAnimation.EasingFunction = new BackEase() { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 };

                Storyboard.SetTargetProperty(slideAnimation, new PropertyPath(FrameworkElement.MarginProperty));
                storyboard.Children.Add(slideAnimation);
            }

            if (isFadeAnimation)
            {
                var opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 0;
                opacityAnimation.To = 1;
                opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));
                storyboard.Children.Add(opacityAnimation);
            }

            storyboard.Begin(element);
        }



    }
}

