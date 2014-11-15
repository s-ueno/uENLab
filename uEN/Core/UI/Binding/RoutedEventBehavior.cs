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
        public IEnumerable<Attribute> Attributes { get; protected set; }

        protected Action Method { get; set; }
        protected Action<RoutedEventArgs> ArgsMethod { get; set; }

        public virtual void Ensure()
        {
            var uiElement = Element as UIElement;
            if (uiElement == null)
                return;

            var methodInfo = LambdaExpression.GetMethodInfo();
            var p = methodInfo.GetParameters().FirstOrDefault();
            if (p != null)
            {
                var invokerType = typeof(Action<>).MakeGenericType(p.ParameterType);
                var invoker = Delegate.CreateDelegate(invokerType, ViewModel, methodInfo);
                ArgsMethod = invoker as Action<RoutedEventArgs>;
            }
            else
            {
                var invoker = Delegate.CreateDelegate(typeof(Action), ViewModel, methodInfo);
                Method = invoker as Action;
            }
            uiElement.AddHandler(RoutedEvent, new RoutedEventHandler(OnEventInternal));
            Attributes = ListAttribute(methodInfo);
        }

        public IEnumerable<Attribute> ListAttribute(MethodInfo mi)
        {
            var list = new List<Attribute>();
            var atts = mi.GetCustomAttributes<Attribute>();
            if (atts != null && atts.Any())
            {
                list.AddRange(atts);
            }

            var t = ViewModel.GetType();
            var classAtts = t.GetCustomAttributes(true).OfType<Attribute>();
            var asmAtts = t.Assembly.GetCustomAttributes(true).OfType<Attribute>();

            list.AddRange(classAtts.Union(asmAtts));
            list.Add(new WaitCursorEventPolicyAttribute());
            list.Add(new ActionEventPolicyAttribute());

            return list;
        }

        protected virtual void OnEventInternal(object sender, RoutedEventArgs e)
        {
            var policies = Attributes.OfType<IRoutedEventPolicy>()
                                     .OrderByDescending(x => x.Priolity);
            var enumerator = policies.GetEnumerator();
            var hasPolicy = enumerator.MoveNext();
            IRoutedEventPolicy policy;
            while (hasPolicy)
            {
                policy = enumerator.Current;
                hasPolicy = enumerator.MoveNext();
                if (hasPolicy)
                {
                    policy.NextPolicy = enumerator.Current;
                }
            }
            var actionPolicy = policies.OfType<ActionEventPolicyAttribute>().Single();
            actionPolicy.EventArgs = e;
            actionPolicy.Do = x =>
            {
                if (Method != null)
                    Method.Invoke();

                if (ArgsMethod != null)
                    ArgsMethod.Invoke(e);
            };
            policies.First().Action(this);
        }
    }
}
