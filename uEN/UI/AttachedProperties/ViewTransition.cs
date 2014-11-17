﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace uEN.UI.AttachedProperties
{
    public enum TransitionStyle
    {
        None,

        Slide,
        VerticalSlide,

        SlideOut,
        VerticalSlideOut,
    }

    public class ViewTransition
    {
        public static TransitionStyle GetTransitionStyle(DependencyObject obj)
        {
            return (TransitionStyle)obj.GetValue(TransitionStyleProperty);
        }

        public static void SetTransitionStyle(DependencyObject obj, TransitionStyle value)
        {
            obj.SetValue(TransitionStyleProperty, value);
        }

        // Using a DependencyProperty as the backing store for TransitionStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransitionStyleProperty =
            DependencyProperty.RegisterAttached("TransitionStyle", typeof(TransitionStyle), typeof(ViewTransition), new UIPropertyMetadata(TransitionStyle.None, OnTransitionStyleChanged));

        private static void OnTransitionStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fw = d as FrameworkElement;
            var style = e.NewValue as TransitionStyle?;
            if (!style.HasValue || style == TransitionStyle.None)
                return;

            fw.Loaded -= fw_Loaded;
            fw.Loaded += fw_Loaded;
        }

        static void fw_Loaded(object sender, RoutedEventArgs e)
        {
            var fw = (FrameworkElement)sender;
            var style = GetTransitionStyle(fw);

            Play(fw, style);
        }

        public static Storyboard Play(FrameworkElement target, TransitionStyle style, Action completedAction = null)
        {
            Storyboard storyboard = null;
            switch (style)
            {
                case TransitionStyle.None:
                    break;
                case TransitionStyle.Slide:
                    storyboard = CreateSlideStoryboard();
                    break;
                case TransitionStyle.VerticalSlide:
                    storyboard = CreateVerticalSlideStoryboard();
                    break;
                case TransitionStyle.SlideOut:
                    storyboard = CreateSlideStoryboard(false);
                    break;
                case TransitionStyle.VerticalSlideOut:
                    storyboard = CreateVerticalSlideStoryboard(false);
                    break;
                default:
                    break;
            }
            if (completedAction != null)
                storyboard.Completed += (x, y) => completedAction();
            storyboard.Begin(target);
            return storyboard;
        }

        private static Storyboard CreateSlideStoryboard(bool isFadeIn = true)
        {
            var storyboard = new Storyboard();

            var fromThickness = isFadeIn ? new Thickness(30, 0, -30, 0) : new Thickness(0);
            var toThickness = isFadeIn ? new Thickness(0) : new Thickness(30, 0, -30, 0);

            var slideAnimation = new ThicknessAnimation();
            slideAnimation.From = fromThickness;
            slideAnimation.To = toThickness;
            slideAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath(FrameworkElement.MarginProperty));
            storyboard.Children.Add(slideAnimation);

            var fromOpacity = isFadeIn ? 0 : 1;
            var toOpacity = isFadeIn ? 1 : 0;

            var opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = fromOpacity;
            opacityAnimation.To = toOpacity;
            opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));
            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }

        private static Storyboard CreateVerticalSlideStoryboard(bool isFadeIn = true)
        {
            var storyboard = new Storyboard();

            var fromThickness = isFadeIn ? new Thickness(0, 30, 0, -30) : new Thickness(0);
            var toThickness = isFadeIn ? new Thickness(0) : new Thickness(0, 30, 0, -30);


            var slideAnimation = new ThicknessAnimation();
            slideAnimation.From = fromThickness;
            slideAnimation.To = toThickness;
            slideAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath(FrameworkElement.MarginProperty));
            storyboard.Children.Add(slideAnimation);

            var fromOpacity = isFadeIn ? 0 : 1;
            var toOpacity = isFadeIn ? 1 : 0;

            var opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = fromOpacity;
            opacityAnimation.To = toOpacity;
            opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(FrameworkElement.OpacityProperty));
            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }

    }
}