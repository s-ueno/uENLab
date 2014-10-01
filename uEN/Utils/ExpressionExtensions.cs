using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace uEN.Utils
{
    public static class ExpressionExtensions
    {
        public static string ToPropertyName<T>(this T obj, Expression<Func<T, object>> expr)
        {
            return expr.ToSymbol();
        }
        public static string ToSymbol(this Expression expr)
        {
            if (expr == null)
                return null;

            var memExp = (expr as LambdaExpression).Body as MemberExpression;
            var list = new List<string>();
            while (memExp is MemberExpression)
            {
                list.Add(memExp.Member.Name);
                memExp = memExp.Expression as MemberExpression;
            }
            return string.Join(".", list.Reverse<string>());
        }
    }
}
