using System.Linq;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace Spacebattle
{
    public class Vector<T> : IEnumerable<T>
    {
        private T[] _values;

        // Размерность линейного пространства.
        private int _rank;

        public Vector(IEnumerable<T> initialValues)
        {
            if (initialValues == null || initialValues.Count() == 0)
            {
                throw new ArgumentException($"Параметр {nameof(initialValues)} не содержит значений координат.");
            }

            _rank = initialValues.Count();
            _values = initialValues.ToArray();
        }

        public int Size
        {
            get
            {
                return _rank;
            }
        }

        public static implicit operator Vector<T>(T[] a) => new Vector<T>(a);
        

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)_values).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append("(");

            for (var i = 0; i < _values.Length; i++)
            { 
                if (i != 0)
                {
                    result.Append(", ");
                }

                var value = _values[i];

                result.Append(Equals(value, default(T)) ? "undefined" : value.ToString());
            }

            result.Append(")");

            return result.ToString();
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            var regex = @"^\({1}([\w.-]*)((, ){1}[\w.-]+)+\){1}$";

            if (!Regex.IsMatch(value, regex))
            {
                throw new FormatException("Не удалось преобразовать значение в массив координат, так как оно не соответствует формату.");
            }

            var valueStringBuidler = new StringBuilder(value);

            valueStringBuidler
                .Replace("(", "")
                .Replace(")", "");

            var coordinates = valueStringBuidler.ToString().Split(", ");

            var result = new T[coordinates.Length];

            for (var i = 0; i < coordinates.Length; i++)
            {
                try
                {
                    result[i] = parse(coordinates[i]);
                }
                catch(Exception e)
                {
                    throw new FormatException("Не удалось преобразовать значение в массив координат.", e);
                }
            }

            return result; 
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            if (a == null || b == null
                || a.Size != b.Size)
            {
                throw new ArgumentException("Размерности векторов не сопадают.");
            }

            var sumVector = new T[a.Size];

            for (var i = 0; i< a.Size; i++)
            {
                dynamic aValue = a.ElementAt(i);
                dynamic bValue = b.ElementAt(i);
                dynamic sum = aValue + bValue;

                sumVector[i] = sum;
            }

            return sumVector;
        }

    }
}