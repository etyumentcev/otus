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
            if (initialValues == null || initialValues.Count() <= 0)
                throw new ArgumentException("Пустой массив");
            
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
            return new Vector<T>(a);
        }

        public IEnumerator<T> GetEnumerator()
        {
            //Задача 4
            foreach (var value in _values)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //Задача 4
            return GetEnumerator();
        }

        public override string ToString()
        {
            //Задача 5
            return $"({string.Join(", ", _values)})";
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            //Задача 6
            var trimValues = value.Trim('(', ')').Split(',');

            var data = new T[trimValues.Length];
            for (int i = 0; i < trimValues.Length; i++)
            {
                data[i] = parse(trimValues[i].Trim());
            }

            return data;
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            //Задача 7
            if(a.Size != b.Size)
                throw new ArgumentException("Размерности векторов не совпадают");
            
            ParameterExpression ap = Expression.Parameter(typeof(T), "a");
            ParameterExpression bp = Expression.Parameter(typeof(T), "b");
            Expression operationResult = Expression.Add(ap, bp);
            
            var lambda = Expression.Lambda<Func<T, T, T>>(operationResult, ap, bp);

            Func<T, T, T> addFunc = lambda.Compile();
            
            var data = new T[a.Size];
            for (int i = 0; i < a.Size; i++)
            {
                data[i] = addFunc(a._values[i], b._values[i]);
            }

            return data;
        }

    }
}