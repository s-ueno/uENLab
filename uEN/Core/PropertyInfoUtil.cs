using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace uEN.Core
{
    public static class PropertyInfoUtil
    {
        public static IEnumerable<PropertyAccessor> ToAccessors(Type t)
        {
            List<PropertyAccessor> result;
            if (!dic.TryGetValue(t, out result))
            {
                var list = t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                            .Where(x => x.CanRead)
                            .Select(x =>
                            {
                                var accessor = PropertyAccessor.Create(x);
                                return accessor;
                            })
                            .ToList();
                result = dic[t] = list;
            }
            return result;
        }
        static ConcurrentDictionary<Type, List<PropertyAccessor>> dic = new ConcurrentDictionary<Type, List<PropertyAccessor>>();
    }


    [Serializable]
    public abstract class PropertyAccessor
    {
        public bool CanWrite { get; set; }
        public string Name { get; private set; }
        public abstract object GetValue(object obj);
        public abstract void SetValue(object obj, object value);
        public static PropertyAccessor Create(PropertyInfo pi)
        {
            Delegate getter = null;
            Delegate setter = null;

            if (pi.CanRead)
            {
                var getterDelegateType = typeof(Func<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
                getter = Delegate.CreateDelegate(getterDelegateType, pi.GetGetMethod(true));
            }

            if (pi.CanWrite)
            {
                Type setterDelegateType = typeof(Action<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
                setter = Delegate.CreateDelegate(setterDelegateType, pi.GetSetMethod(true));
            }

            var accessorType = typeof(TypedPropertyProvider<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
            var provider = (PropertyAccessor)Activator.CreateInstance(accessorType, getter, setter);
            provider.Name = pi.Name;
            provider.CanWrite = pi.CanWrite;
            return provider;
        }
    }

    [Serializable]
    internal class TypedPropertyProvider<TTarget, TProperty> : PropertyAccessor
    {
        readonly Func<TTarget, TProperty> getter;
        readonly Action<TTarget, TProperty> setter;
        public TypedPropertyProvider(Func<TTarget, TProperty> getter, Action<TTarget, TProperty> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }
        public override object GetValue(object obj)
        {
            return this.getter((TTarget)obj);
        }
        public override void SetValue(object obj, object value)
        {
            this.setter((TTarget)obj, (TProperty)value);
        }
    }


}
