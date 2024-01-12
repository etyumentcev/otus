using System.Linq;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;


namespace Spacebattle
{
    public class Vector<T> : IEnumerable<T>
    {
        private int _size = 0;
        private T[] _values;

        public Vector(IEnumerable<T> initialValues)
        {
            if (initialValues == null || !initialValues.Any())
                throw new ArgumentException("Cписок NULL или пуст", nameof(initialValues));
            _size = initialValues.Count();
            _values = initialValues.ToArray();
        }

        public T[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public static implicit operator Vector<T>(T[] a)
        {
            return new Vector<T>(a);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _values.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

        public override string ToString()
        {
            return $"({string.Join(", ", _values)})";
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            if (value.FirstOrDefault() != '(' | value.LastOrDefault() != ')')
                throw new FormatException(string.Format("{0}  is not valid format for type {1}", value, typeof(T[]).ToString()));

            string tmpStr = value.Substring(1, value.Length - 2).Trim();
            string[] items = tmpStr.Split(',');
            var values = items.Select(s => parse(s.Trim()));
            return values.ToArray();
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            if (a.Size != b.Size)
                throw new ArgumentException("Невозможно сложить массивы разной величны!");
            return AddT(a.Values.ToArray(), b.Values.ToArray());
        }

        private static Func<T[], T[], T[]> AddT => FuncGenerator<T>.CreateArrayOperatorFuncAdd((a, b) => Expression.Add(a, b));
    }
}