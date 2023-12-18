using System.Collections;


namespace Spacebattle
{
    public class Vector<T> : IEnumerable<T>
    {
        private T[] _values;

        public Vector(IEnumerable<T> initialValues)
        {
            if (initialValues == null || !initialValues.Any())
                throw new ArgumentException("Initial values cannot be null or empty.");

            _values = initialValues.ToArray();
        }

        public int Size => _values.Length;

        public static implicit operator Vector<T>(T[] a)
        {
            if (a == null || a.Length == 0)
                throw new ArgumentException("Array cannot be null or empty.");

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
            return value.Trim('(', ')').Split(',')
                .Select(parse).ToArray();
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            if (a.Size != b.Size)
                throw new ArgumentException("Vector dimensions must be equal for addition.");

            T[] result = new T[a.Size];

            for (int i = 0; i < a.Size; i++)
            {
                result[i] = (T)Convert.ChangeType((dynamic)a._values[i]! + b._values[i], typeof(T));
            }

            return new Vector<T>(result);
        }

    }
}