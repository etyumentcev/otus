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
        private int _size;

        public Vector(IEnumerable<T> initialValues)
        {
            // Задача 1.
            if (initialValues.ToArray().Length == 0)
            {
                throw new ArgumentException();
            }
            _values = initialValues.ToArray();
            _size = _values.Length;
        }

        private void calcSize()
        {
            _size = _values.Length;
        }

        public int Size
        {
            get
            {
                return _size;
            }
        }

        public static implicit operator Vector<T>(T[] a)
        {
            Vector<T> v = new Vector<T>(a);
            v.calcSize();
            return v;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            //Задача 4
            return new VectorEnumerator<T>(_values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //Задача 4
            return new VectorEnumerator<T>(_values);
        }

        public override string ToString()
        {
            //Задача 5
            string valuesString = string.Join(", ", _values);
            return $"({valuesString})";
        }

        public static T[] Parse(string value, Func<string, T> parse)
        {
            //Задача 6
            if (value.Contains(") ") || value.Contains(",)") || value.Contains("()"))
            {
                throw new FormatException();
            }

            string cleanValue = value.Replace("(", "").Replace(")", "").Replace(" ", "");
            string[] elements = cleanValue.Split(',');
            T[] result = new T[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                result[i] = parse(elements[i]);
            }
            ;
            return result;
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            //Задача 7
        
            if (a._size != b._size)
                throw new ArgumentException("Vectors must have the same size for addition.");

            ParameterExpression paramV1 = Expression.Parameter(typeof(T[]));
            ParameterExpression paramV2 = Expression.Parameter(typeof(T[]));

            List<Expression> elementExpressions = new List<Expression>();
            for (int i = 0; i < a._size; i++)
            {
                BinaryExpression addExpression = Expression.Add(
                    Expression.ArrayAccess(paramV1, Expression.Constant(i)),
                    Expression.ArrayAccess(paramV2, Expression.Constant(i))
                );
                elementExpressions.Add(addExpression);
            }

            NewArrayExpression newArrayExpression = Expression.NewArrayInit(typeof(T), elementExpressions);
            LambdaExpression lambdaExpression = Expression.Lambda<Func<T[], T[], T[]>>(newArrayExpression, paramV1, paramV2);
            Func<T[], T[], T[]> sumFunc = (Func<T[], T[], T[]>)lambdaExpression.Compile();
       
            T[] result= sumFunc(a._values, b._values);

            return new Vector<T>(result);
        }
    }

    public class VectorEnumerator<T> : IEnumerator<T>
    {
        private T[] data;
        private int position = -1;

        public VectorEnumerator(T[] array)
        {
            data = array;
        }

        public bool MoveNext()
        {
            position++;
            return (position < data.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        public T Current
        {
            get
            {
                try
                {
                    return data[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            // Освобождение ресурсов
        }

    }

}