using System.Linq.Expressions;

namespace Spacebattle
{
    public static class FuncGenerator<T>
    {
        public static Func<T[], T[], T[]> ArrayExpressionToFunc(Func<IndexExpression, IndexExpression, BinaryExpression> f)
        {
            // Создание параметров для входных массивов и результирующего массива
            ParameterExpression apA = Expression.Parameter(typeof(T[]), "a");
            ParameterExpression bpA = Expression.Parameter(typeof(T[]), "b");
            ParameterExpression operationResult = Expression.Parameter(typeof(T[]), "c");
            ParameterExpression iA = Expression.Parameter(typeof(int), "i");

            LabelTarget labelReturn = Expression.Label(typeof(T[]));

            // Блок кода внутри цикла
            Expression innerBlock = Expression.Block(
                Expression.SubtractAssign(iA, Expression.Constant(1)),
                Expression.IfThen(Expression.Equal(iA, Expression.Constant(-1)),
                Expression.Return(labelReturn, operationResult)),
                Expression.Assign(Expression.ArrayAccess(operationResult, iA), f(Expression.ArrayAccess(apA, iA), Expression.ArrayAccess(bpA, iA)))
                );

            // Выражение для создания нового массива
            Expression<Func<int, T[]>> newTA = (i) => new T[i];

            // Основное тело функции. Инициализация переменных и выполнение цикла
            Expression addeA = Expression.Block(
                new[] { iA, operationResult },
                Expression.Assign(iA, Expression.ArrayLength(apA)),
                Expression.Assign(operationResult, Expression.Invoke(newTA, iA)),
                Expression.Loop(innerBlock, labelReturn)
                );

            // Компиляция, чтобы получить результат
            Expression<Func<T[], T[], T[]>> lambdaA = Expression.Lambda<Func<T[], T[], T[]>>(addeA, apA, bpA);

            return lambdaA.Compile();
        }
    }
}
