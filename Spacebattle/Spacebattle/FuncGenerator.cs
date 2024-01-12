using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spacebattle
{
    public static class FuncGenerator<T>
    {
        public static Func<T, T, T> CreateOperatorFuncAdd(Func<ParameterExpression, ParameterExpression, BinaryExpression> f)
        {
            ParameterExpression ap = Expression.Parameter(typeof(T), "a");
            ParameterExpression bp = Expression.Parameter(typeof(T), "b");
            Expression operationResult = f(ap, bp);

            Expression<Func<T, T, T>> lambda = Expression.Lambda<Func<T, T, T>>(operationResult, ap, bp);
            return lambda.Compile();
        }

        public static Func<T[], T[], T[]> CreateArrayOperatorFuncAdd(Func<ParameterExpression, ParameterExpression, BinaryExpression> f)
        {
            Func<T, T, T> op = CreateOperatorFuncAdd(f);
            return (a, b) =>
            {
                int len = a.Length;
                T[] result = new T[len];
                for (int i = 0; i < len; i++)
                    result[i] = op(a[i], b[i]);
                return result;
            };
        }
    }
}
