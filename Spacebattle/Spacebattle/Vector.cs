using System.Linq.Expressions;
using System.Collections;
using System.Text;


namespace Spacebattle
{
    public class Vector<T> : IEnumerable<T>
    {
        private static readonly Func<T, T, T> _addFunc = CreateAdd();
        private readonly T[] _values;

        public Vector(IEnumerable<T> initialValues)
        {
            if (initialValues is null || !initialValues.Any())
            {
                throw new ArgumentException("Incorrect initial values");
            }

            _values = initialValues.ToArray();
        }

        public int Size => _values.Length;

        public static implicit operator Vector<T>(T[] a)
        {
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
            var sb = new StringBuilder();
            sb.Append('(');
            sb.AppendJoin(", ", _values);
            sb.Append(')');

            return sb.ToString();
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new FormatException("Input string cannot be null or empty");
            }

            if (!value.StartsWith('(') || !value.EndsWith(')') || value.Length < 3)
            {
                throw new FormatException("Input string is in an incorrect format");
            }

            var coordinates = value.Trim('(', ')').Split(", ");
            
            return coordinates
                .Select(parse)
                .ToArray();
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            if (a.Size != b.Size || a is null || b is null)
            {
                throw new ArgumentException("The vectors must have the same dimension");
            }
            
            var resultValues = new T[a.Size];
            for (var i = 0; i < a.Size; i++)
            {
                resultValues[i] = _addFunc(a._values[i], b._values[i]);
            }

            return new Vector<T>(resultValues);
        }

        private static Func<T, T, T> CreateAdd()
        {
            var ap = Expression.Parameter(typeof(T), "a");
            var bp = Expression.Parameter(typeof(T), "b");

            var operationResult = Expression.Add(ap, bp);

            var lambda = Expression.Lambda<Func<T, T, T>>(operationResult, ap, bp);

            return lambda.Compile();
        }
    }
}