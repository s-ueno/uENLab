using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace uEN.Extensions
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


        public static IEnumerable<Attribute> ListAttributes(this  Expression expr)
        {
            var memExp = (expr as LambdaExpression).Body as MemberExpression;
            var list = new List<Attribute>();
            while (memExp is MemberExpression)
            {
                if (memExp.Member != null)
                {
                    var atts = memExp.Member.GetCustomAttributes(typeof(Attribute), true).OfType<Attribute>();
                    if (atts.Any())
                    {
                        list.AddRange(atts);
                    }
                }
                memExp = memExp.Expression as MemberExpression;
            }
            return list;
        }

        //http://stackoverflow.com/questions/12166785/lambda-expressions-t-functin-tout-and-methodinfo
        public static MethodInfo GetMethodInfo(this Expression expression)
        {
            var currentExpression = DissectUnaryExpression(expression);
            if (currentExpression is LambdaExpression)
                currentExpression = ((LambdaExpression)currentExpression).Body;

            if (currentExpression.NodeType == ExpressionType.Convert ||
                currentExpression.NodeType == ExpressionType.ConvertChecked)
            {
                var unaryExpression = currentExpression as UnaryExpression;
                currentExpression = unaryExpression.Operand;
            }

            MethodCallExpression methodCallExpression = currentExpression as MethodCallExpression;
            if (methodCallExpression == null) 
                throw new ArgumentException("ErrorInvalidMethodCallExpression", "expression");

            ConstantExpression constantExpression =
                methodCallExpression.Object as ConstantExpression;

            MethodInfo methodInfo;
            if (constantExpression != null)
            {
                methodInfo = constantExpression.Value as MethodInfo;
            }
            else
            {
                constantExpression = methodCallExpression.Arguments
                                        .Single(a => a.Type == typeof(MethodInfo)
                                            && a.NodeType == ExpressionType.Constant) as
                                        ConstantExpression;
                methodInfo = constantExpression.Value as MethodInfo;
            }

            return methodInfo;
        }

        private static Expression DissectUnaryExpression(Expression expression)
        {
            var currentExpression = expression;
            while (currentExpression is UnaryExpression)
                currentExpression = (currentExpression as UnaryExpression).Operand;
            return currentExpression;
        }
    }
}
