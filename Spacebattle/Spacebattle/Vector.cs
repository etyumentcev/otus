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
            if (initialValues == null || initialValues?.Count() == 0)
                throw new ArgumentException("Вектор нельзя инициализировать пустым массивом!");
            _values = initialValues?.ToArray();
            Size = _values.Length;
        }

        public int Size { get; private set; }

        public static implicit operator Vector<T>(T[] a)
        {
            return new Vector<T>(a);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            int position = 0;
            foreach (var item in _values)
            {
                position++;
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('(');
            foreach (var item in _values) 
            {
                if (stringBuilder.Length > 1) 
                {
                    stringBuilder.Append(", ");
                }
                stringBuilder.Append(item);
            }
            stringBuilder.Append(')');
            return stringBuilder.ToString();
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            if (value[0] != '(' | value[value.Length - 1] != ')')
                throw new FormatException(string.Format("{0} не верный формат для типа {1}!", value, typeof(T[]).ToString()));

            string tmpStr = value[1..^1].Trim();

            string[] items = tmpStr.Split(new char[] { ',' });
            var values = items.Select(s => parse(s.Trim()));
            return values.ToArray();
        }

        static TT Add<TT>(TT x, TT y)
        {
            dynamic dx = x, dy = y;
            return dx + dy;
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            if (a.Size != b.Size)
                throw new ArgumentException("Размерности векторов не совпадают!");
            T[] values = new T[a.Size];
            for (int i = 0; i < a.Size; i++)
            {

                values[i] = Add(a._values[i], b._values[i]);
            }
            return new Vector<T>(values);
        }
    }
}