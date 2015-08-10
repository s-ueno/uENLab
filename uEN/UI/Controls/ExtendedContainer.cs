using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using uEN.UI.AttachedProperties;

namespace uEN.UI.Controls
{
    public class ExtendedContainer
    {
        public ExtendedContainer(Window win)
        {
            Container = win.Template.FindName("PART_ExtendedContainer", win) as Grid;
            TopContent = win.Template.FindName("TopExtendedContent", win) as GenericPresenter;
            BottomContent = win.Template.FindName("BottomExtendedContent", win) as GenericPresenter;
        }
        protected Grid Container { get; set; }
        protected GenericPresenter TopContent { get; set; }
        protected GenericPresenter BottomContent { get; set; }
        public void Show(BizViewModel top, BizViewModel bottom)
        {
            Container.MouseDown += OnMouseDown;
            Container.Visibility = Visibility.Visible;

            TopContent.ViewModelPresenter.Content = top;
            BottomContent.ViewModelPresenter.Content = bottom;

            OpenAnimation(TopContent, true);
            OpenAnimation(BottomContent, false);
        }


        public static bool GetIsManualDispose(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsManualDisposeProperty);
        }
        public static void SetIsManualDispose(DependencyObject obj, bool value)
        {
            obj.SetValue(IsManualDisposeProperty, value);
        }
        public static readonly DependencyProperty IsManualDisposeProperty =
            DependencyProperty.RegisterAttached("IsManualDispose", typeof(bool), typeof(ExtendedContainer),
            new PropertyMetadata(false));


        public void Close()
        {
            Container.MouseDown -= OnMouseDown;

            CloseAnimation(TopContent, true, () => Container.Visibility = Visibility.Collapsed);
            CloseAnimation(BottomContent, false);

            DisposableContent(TopContent.ViewModelPresenter);
            DisposableContent(BottomContent.ViewModelPresenter);
        }
        private static void DisposableContent(ContentPresenter presenter)
        {
            var view = presenter.FindVisualChildren<BizView>().FirstOrDefault();
            if (view != null)
            {
                var value = GetIsManualDispose(view);
                if (value)
                    return;
            }
            var disposable = presenter.Content as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TopContent.IsMouseOver) return;
            if (BottomContent.IsMouseOver) return;
            Close();
        }
        private static readonly double SlideMargin = BizUtils.AppSettings("ExtendedContainer.Margin", 100d);
        private static void OpenAnimation(FrameworkElement element, bool isTop = true)
        {
            var fromThickness = isTop ? new Thickness(0, -SlideMargin, 0, SlideMargin) : new Thickness(0, SlideMargin, 0, -SlideMargin);
            var toThickness = new Thickness(0);
            var fromOpacity = 0;
            var toOpacity = 1;
            var storyboard = ViewTransition.CreateSlideStoryboard(fromThickness, toThickness, fromOpacity, toOpacity);
            storyboard.Begin(element);
        }
        private static void CloseAnimation(FrameworkElement element, bool isTop = true, Action action = null)
        {
            var fromThickness = new Thickness(0);
            var toThickness = isTop ? new Thickness(0, -SlideMargin, 0, SlideMargin) : new Thickness(0, SlideMargin, 0, -SlideMargin);
            var fromOpacity = 1;
            var toOpacity = 0;
            var storyboard = ViewTransition.CreateSlideStoryboard(fromThickness, toThickness, fromOpacity, toOpacity /*, slideTimeSpan: 0.4d, opacityTimeSpan: 0.3d */);
            if (action != null)
            {
                storyboard.Completed += (sender, e) => action();
            }
            storyboard.Begin(element);
        }
    }
}
