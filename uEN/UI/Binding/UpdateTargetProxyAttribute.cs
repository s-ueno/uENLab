using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using uEN.Core;

namespace uEN.UI.DataBinding
{
    public abstract class UpdateTargetProxyAttribute : Attribute, IBindingAttribute
    {
        protected PropertyAccessor Accessor { get; private set; }
        public void Binding(IBindingBehavior behavior)
        {
            var dp = behavior as DependencyPropertyBehavior;
            if (dp == null) return;

            var body = behavior.LambdaExpression.Body;
            var propertyInfo = (PropertyInfo)((MemberExpression)body).Member;
            Accessor = PropertyAccessor.Create(propertyInfo);

            dp.UpdatingTarget -= dp_UpdatingTarget;
            dp.UpdatingTarget += dp_UpdatingTarget;
        }
        void dp_UpdatingTarget(object sender, EventArgs e)
        {
            var behavior = sender as DependencyPropertyBehavior;
            if (behavior == null) return;

            var source = Accessor.GetValue(behavior.ViewModel);
            if (source == null) return;

            Resolve(source, behavior);
        }
        protected abstract void Resolve(object source, DependencyPropertyBehavior behavior);
    }
}
