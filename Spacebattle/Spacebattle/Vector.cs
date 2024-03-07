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
            //Задача 1
            if (!initialValues.Any())
                throw new ArgumentException("Пустая последовательность!!!");
            
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
            if (a.Length == 0)
                throw new ArgumentException("Пустой массив!!!");
                
            return new Vector<T>(a);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            //Задача 4
            return ((IEnumerable<T>)_values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //Задача 4
            return _values.GetEnumerator();
        }

        public override string ToString()
        {
            //Задача 5
            return "(" + string.Join(", ", _values) + ")";
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            //Задача 6
            return value.Trim('(', ')').Split(',').Select(parse).ToArray();
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            //Задача 7
            if (a.Size != b.Size)
                throw new ArgumentException("Размерности не совпадают");
            
            var parA = Expression.Parameter(typeof(T));
            var parB = Expression.Parameter(typeof(T));
            var add = Expression.Add(parA, parB);
            
            var lambda = Expression.Lambda<Func<T, T, T>>(add, parA, parB);
            var resultValues = new T[a.Size];
            for (int i = 0; i < a.Size; i++)
            {
                resultValues[i] = lambda.Compile()(a._values[i], b._values[i]);
            }
            
            return new Vector<T>(resultValues);
        }

    }
}