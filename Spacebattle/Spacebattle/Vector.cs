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
            if(initialValues == null)
                throw new ArgumentNullException(nameof(initialValues));

            if (initialValues.Count() == 0)
                throw new ArgumentException("Размерность вектора должна быть большей нуля");

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

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            //Задача 4
            foreach(T t in _values)
                yield return t;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //Задача 4
            return _values.GetEnumerator();
        }

        public override string ToString()
        {
            //Задача 5
            return "(" + string.Join(", ", _values.Select(x => x.ToString())) + ")";
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            //Задача 6
            if(value == null) throw new ArgumentNullException("value");
            
            value = value.Trim(new char[] {'(',')'});
            var elems = value.Split(", ");
            if (elems.Length == 0) throw 
                    new ArgumentException("Не верный формат или вектор не содержит ни одного элемента");

            return elems.Select(x => parse(x)).ToArray();
        }

        private static Func<T, T, T> CreateAdd()
        {
            ParameterExpression ap = Expression.Parameter(typeof(T), "a");
            ParameterExpression bp = Expression.Parameter(typeof(T), "b");

            Expression operationResult = Expression.Add(ap, bp);

            Expression<Func<T, T, T>> lambda = Expression.Lambda<Func<T, T, T>>(operationResult, ap, bp);

            Func<T, T, T> add = lambda.Compile();
            return add;
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            //Задача 7

            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");

            if (a.Size != b.Size) 
                throw new ArgumentException("Операция невозможна. Размерности векторов не совпадают.");

           
            return new Vector<T>(a._values.Zip(b._values, CreateAdd()));
        }

    }
}