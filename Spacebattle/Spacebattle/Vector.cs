using System.Collections;
using System.Linq.Expressions;


namespace Spacebattle
{
    public class Vector<T> : IEnumerable<T>
    {
        private static readonly Func<IndexExpression, IndexExpression, BinaryExpression> addExpression = (left, right) => Expression.Add(left, right);
        private static readonly Func<T[], T[], T[]> AddT = FuncGenerator<T>.ArrayExpressionToFunc(addExpression);
        private readonly T[] _values;

        public Vector(IEnumerable<T> initialValues)
        {
            // Задача 1.
            if (initialValues == null || !initialValues.Any())
            {
                throw new ArgumentException("Initial values cannot be null or empty");
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
                throw new ArgumentException("Array cannot be null or empty.");

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
            return _values.GetEnumerator();
        }

        public override string ToString()
        {
            //Задача 5
            return $"({string.Join(", ", _values)})";
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            //Задача 6
            string[] elements = value.Trim('(', ')').Split(',');
            T[] result = new T[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                // Применяем функцию parse к каждому элементу
                result[i] = parse(elements[i].Trim());
            }
            return result;
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            //Задача 7
            if (a.Size != b.Size)
                throw new ArgumentException("Vector dimensions do not match");

            return AddT(a.ToArray(), b.ToArray());
        }
    }
}