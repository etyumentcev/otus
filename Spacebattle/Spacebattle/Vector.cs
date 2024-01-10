using System.Linq;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Spacebattle
{
    public class Vector<T> : IEnumerable<T>
    {
        private T[] _values;

        public Vector(IEnumerable<T> initialValues)
        {
            // Задача 1.
            if (initialValues == null || !initialValues.Any())
                throw new ArgumentException("Невозможно создать вектор из пустого перечисления");

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
                throw new ArgumentException("Невозможно создать вектор из массива нулевой длины");
            }
            return new Vector<T>(a);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            //Задача 4
            foreach (T item in _values)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //Задача 4
            return _values.GetEnumerator();
        }

        public override string ToString()
        {
            //Задача 5
            StringBuilder sb = new();
            sb.Append("(");
            for (int i = 0; i < Size; i++)
            {
                sb.Append(_values[i]);
                if (i < Size - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            //Задача 6
            string[] strArrayValues = value.Trim('(', ')').Split(',');
            if (strArrayValues.Length == 0)
                throw new ArgumentException($"Невозможно преобразовать строку '$value' в массив");

            T[] result = new T[strArrayValues.Length];
            for (int i = 0; i < strArrayValues.Length; i++)
            {
                result[i] = parse(strArrayValues[i]);
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

            ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
            var add = Expression.Add(paramA, paramB);
            var lambda = Expression.Lambda<Func<T, T, T>>(add, paramA, paramB).Compile();

            T[] result = new T[a.Size];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = lambda(a._values[i], b._values[i]);
            }

            return new Vector<T>(result);
        }

    }
}