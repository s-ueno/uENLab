using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using uEN.Core;

namespace uEN
{
    public class Commitable<T> : NotifiableImp
    {
        private T target;

        public T Target
        {
            get { return target; }
            set
            {
                target = value;
                this.OnPropertyChanged("Target");
            }
        }

        public T Original { get; private set; }

        public Commitable(T row)
        {
            this.Original = row;
            Undo();
        }

        public void Undo()
        {
            this.Target = (T)CopyCloneUtil.DeepClone(this.Original);
        }

        public void Commit()
        {
            CopyCloneUtil.CopyProperty(this.Target, this.Original);
        }

        public bool HasChanges()
        {
            return CopyCloneUtil.HasChanges(this.Original, this.Target);
        }
    }


    public static class CopyCloneUtil
    {
        /// <summary>
        /// Nullable派生かどうかを判断します。
        /// </summary>
        /// <param name="type">対象型</param>
        /// <returns>Nullable派生有無</returns>
        public static bool IsNullable(Type type)
        {
            if (type == null)
                return false;
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        /// <summary>
        /// 指定した型は数値、文字列、Nullable値のいずれかかどうかを取得します。
        /// </summary>
        /// <param name="type">型</param>
        /// <returns>結果</returns>
        /// <remarks>基本的にはdb格納可能基本型ということを判断しているという意味になります。</remarks>
        public static bool IsBasicType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return type == typeof(string) || type.IsValueType || IsNullable(type);
        }

        public static bool CopyProperty<T>(T from, params string[] excludedPropertyNames)
        {
            return CopyProperty(typeof(T), from, null, excludedPropertyNames);
        }

        public static bool CopyProperty<T>(T from, T to, params string[] excludedPropertyNames)
        {
            return CopyProperty(typeof(T), from, to, excludedPropertyNames);
        }

        public static bool HasChanges<T>(T from, T to, params string[] excludedPropertyNames)
        {
            return HasChanges(typeof(T), from, to, excludedPropertyNames);
        }

        private static bool HasChanges(Type t, object from, object to, params string[] excludedPropertyNames)
        {
            if (from == null && to == null)
                return false;
            if (to == null || from == null)
                return true;
            if (!from.GetType().IsAssignableFrom(to.GetType()))
                return true;
            excludedPropertyNames = excludedPropertyNames ?? new string[0];
            if (from is IEnumerable)
            {
                var efrom = ((IEnumerable)from).GetEnumerator();
                var eto = ((IEnumerable)to).GetEnumerator();

                while (efrom.MoveNext() && eto.MoveNext())
                {
                    var cfrom = efrom.Current;
                    var cto = eto.Current;
                    if (cfrom == null && cto == null)
                        continue;
                    if (cfrom == null)
                        return true;

                    if (HasChanges(cfrom.GetType(), cfrom, cto, excludedPropertyNames))
                        return true;
                }
            }

            foreach (var prop in t.GetProperties())
            {
                if (excludedPropertyNames.Contains(prop.Name))
                    continue;


                var getm = prop.GetGetMethod();
                if (getm == null)
                    continue;


                //Todo List系バグ調査
                object vFrom;
                object vTo;

                vFrom = getm.Invoke(from, null);
                vTo = getm.Invoke(to, null);

                if (IsBasicType(getm.ReturnType))
                {
                    if (!object.Equals(vFrom, vTo))
                        return true;
                }
                else
                {
                    if (vFrom == null && vTo == null)
                        continue;
                    if (vFrom == null)
                        return true;

                    if (HasChanges(vFrom.GetType(), vFrom, vTo, excludedPropertyNames))
                        return true;
                }
            }
            return false;
        }

        private static bool CopyProperty(Type t, object from, object to, params string[] excludedPropertyNames)
        {
            if (from == null)
                return false;
            if (to == null)
                to = CreateInstance(from);
            if (to == null)
                return false;
            excludedPropertyNames = excludedPropertyNames ?? new string[0];
            if (from is IList)
            {
                var list = from as IList;
                if (!list.IsReadOnly)
                {
                    var listTo = to as IList;
                    if (!list.IsFixedSize)
                    {
                        listTo.Clear();
                        foreach (var each in list)
                        {
                            listTo.Add(each);
                        }
                    }
                }
            }
            else if (from is Array)
            {
                var arr = from as Array;
                var arrTo = to as Array;
                if (arr.Length == arrTo.Length)
                {
                    for (int i = 0; i < arrTo.Length; i++)
                    {
                        arrTo.SetValue(arr.GetValue(i), i);
                    }
                }
            }

            foreach (var prop in t.GetProperties())
            {
                if (excludedPropertyNames.Contains(prop.Name))
                    continue;
                var setm = prop.GetSetMethod();
                var getm = prop.GetGetMethod();
                if (setm == null || getm == null)
                    continue;
                if (IsBasicType(getm.ReturnType))
                    setm.Invoke(to, new object[] { getm.Invoke(from, null) });
                else
                {
                    var originalSub = getm.Invoke(from, null);
                    if (originalSub == null)
                    {
                        setm.Invoke(to, new object[] { null });
                        continue;
                    }
                    var toSub = getm.Invoke(to, null);

                    //数が違う配列、違う方のインスタンスが入っている時も新規作成対象。
                    if (toSub == null || !originalSub.GetType().IsAssignableFrom(toSub.GetType()) || toSub is IEnumerable)
                    {
                        toSub = CreateInstance(originalSub);
                        if (toSub == null)
                            continue;
                        setm.Invoke(to, new object[] { toSub });
                    }
                    CopyProperty(originalSub.GetType(), originalSub, toSub, excludedPropertyNames);
                }
            }
            return true;
        }

        private static object CreateInstance(object originalSub)
        {
            return DeepClone(originalSub);
        }

        public static object DeepClone(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return formatter.Deserialize(ms);
            }
        }
    }
}
