using System.Linq;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;


namespace Spacebattle
{
    public class Vector<T> : IEnumerable<T>
    {
        private T[] _values;

        public Vector(IEnumerable<T> initialValues)
        {
            // Задача 1.
            if (initialValues == null || !initialValues.Any())
            {
                throw new ArgumentException($"Последовательность должна содержать один или более элементов");
            }

            _values = initialValues.ToArray();
        }

        public int Size
        {
            get
            {
                //Задача 2
                return _values.Length;
            }
        }

        public static implicit operator Vector<T>(T[] a)
        {
            //Задача 3
            if (a == null || a.Length == 0)
            {
                throw new ArgumentException($"Последовательность должна содержать один или более элементов");
            }
            return new Vector<T>(a);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            //Задача 4
            foreach (var item in _values)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //Задача 4
            foreach (var item in _values)
            {
                yield return item;
            }
        }

        public override string ToString()
        {
            //Задача 5
            return $"({string.Join(", ", _values)})";
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            //Задача 6
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"Строка не должна быть пустой.");
            }

            var source = value.Trim('(', ')').Split(new[] { ',' }, StringSplitOptions.TrimEntries);
            var result = new T[source.Length];
            for (var index = 0; index < source.Length; index++)
            {
                result[index] = parse(source[index]);
            }
            if (result.Length == 0)
            {
                throw new ArgumentException($"Не удалось привести строку к вектору.");
            }
            return result;
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            //Задача 7
            if (a.Size != b.Size)
            {
                throw new ArgumentException("Размерности векторов не совпадают");
            }

            T[] values = new T[a.Size];
            for (int i = 0; i < a.Size; i++)
            {
                values[i] = CreateAdd().Invoke(a._values[i], b._values[i]);
            }
            return new Vector<T>(values);
        }
        static Func<T, T, T> CreateAdd()
        {
            ParameterExpression ap = Expression.Parameter(typeof(T), "a");
            ParameterExpression bp = Expression.Parameter(typeof(T), "b");

            Expression operationResult = Expression.Add(ap, bp);

            Expression<Func<T, T, T>> lambda = Expression.Lambda<Func<T, T, T>>(operationResult, ap, bp);

            return lambda.Compile();
        }

    }
}