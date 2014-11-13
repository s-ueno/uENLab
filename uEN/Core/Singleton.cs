using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN.Core
{
    public static class Singleton<T> where T : class, new()
    {
        public static T Value
        {
            get { return value; }
        }
        static readonly T value = new T();
    }
}
