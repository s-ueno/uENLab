using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace uEN.UI.Binding
{
    public class RoutedEventBehavior : IBindingBehavior
    {
        public RoutedEvent RoutedEvent { get; set; }

        public object ViewModel { get; set; }
        public DependencyObject Element { get; set; }
        public LambdaExpression LambdaExpression { get; set; }
        
        protected Action Method { get; set; }
        protected Action<RoutedEventArgs> ArgsMethod { get; set; }

        public virtual void Ensure()
        {
            var uiElement = Element as UIElement;
            if (uiElement == null)
                return;

            if (Method == null && ArgsMethod == null)
            {
                var compile = LambdaExpression.Compile().DynamicInvoke(ViewModel);
                Method = compile as Action;
                ArgsMethod = compile as Action<RoutedEventArgs>;
            }
            uiElement.AddHandler(RoutedEvent, new RoutedEventHandler(OnEventInternal));
        }
        protected virtual void OnEventInternal(object sender, RoutedEventArgs e)
        {
            var currentCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                if (Method != null)
                    Method.Invoke();

                if (ArgsMethod != null)
                    ArgsMethod.Invoke(e);
            }
            catch (Exception)
            {
                //エラー時のアプリケーション ポリシー

                throw;
            }
            finally
            {
                Mouse.OverrideCursor = currentCursor;
            }
        }
    }
}
