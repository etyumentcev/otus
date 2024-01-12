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
        public int Size => _values.Length;


        public Vector(IEnumerable<T> initialValues)
        {
            if (initialValues == null || !initialValues.Any())
            {
                throw new ArgumentException("NULL / EMPTY");
            }
            _values = initialValues.ToArray();
        }


        public static implicit operator Vector<T>(T[] a)
        {
            if (a == null || a.Length == 0)
            {
                throw new ArgumentException("NULL / EMPTY");
            }
            return new Vector<T>(a);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)_values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public override string ToString()
        {

            return $"({string.Join(", ", _values)})";
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            if (value[0] != '(' || value[value.Length - 1] != ')')
            {
                throw new FormatException("WRONG FORMAT");
            }
            var items = value.Substring(1, value.Length - 2).Trim().Split(',').Select(s => parse(s.Trim()));
            return items.ToArray();
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            if (a.Size != b.Size)
            {
                throw new ArgumentException("VECTOR SIZE ERROR");
            }
            var vectors = new List<T>();
            for (var i = 0; i < a.Size; i++)
            {
                dynamic elementOfA = a.ElementAt(i);
                dynamic elementOfB = b.ElementAt(i);
                vectors.Add(elementOfA + elementOfB);
            }

            return new Vector<T>(vectors);
        }

    }
}