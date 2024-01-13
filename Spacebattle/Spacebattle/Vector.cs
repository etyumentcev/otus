using System.Collections;
using System.Numerics;
using System.Xml.Linq;


namespace Spacebattle
{
    public class Vector<T> : IEnumerable<T>
    {
        private readonly T[] _values;

        public Vector(IEnumerable<T> initialValues)
        {
            if (initialValues == null || !initialValues.Any())
                throw new ArgumentException("Переданный список равен NULL или пуст", nameof(initialValues));

            _values = initialValues.ToArray();
        }

        public int Size => _values.Length;

        public static implicit operator Vector<T>(T[] a)
        {
            return new Vector<T>(a);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _values.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

        public override string ToString() => $"({string.Join(", ", _values)})";

        public static T[] Parse(string value, Func<string, T> parse)
        {
            var result = new List<T>();
            if (!value.StartsWith('(') || !value.EndsWith(')'))
                throw new FormatException("Строка должна начинаться и заканчиваться круглыми скобками");

            value = value[1..^1];
            var contentArray = value.Split(", ");
            foreach (var content in contentArray)
            {
                try
                {
                    var elemmentOfVector = parse(content);
                    result.Add(elemmentOfVector);
                }
                catch
                {
                    throw new FormatException("Некорректный формат одного из элементов");
                }
            }

            return result.ToArray();
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            if (a.Size != b.Size)
                throw new ArgumentException("Вектора имеют разные размеры");

            var elements = new List<T>();
            for (var i = 0; i < a.Size; i++)
            {
                dynamic elementOfA = a.ElementAt(i);
                dynamic elementOfB = b.ElementAt(i);
                dynamic summ = elementOfA + elementOfB;
                elements.Add(summ);
            }

            return new Vector<T>(elements);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not IEnumerable<T> enums)
                return false;

            var vector = enums is Vector<T>
                ? enums as Vector<T>
                : new Vector<T>(enums);                           

            if (Size != vector.Size)
                return false;

            for (var i = 0; i < Size; i++)
            {
                dynamic elementOfA = this.ElementAt(i);
                dynamic elementOfB = vector.ElementAt(i);
                if (elementOfA != elementOfB)
                    return false;
            }

            return true;
        }
    }
}