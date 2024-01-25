using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        private readonly Dictionary<T, CollisionDetector<T>> node;
        public event Action Detected; 

        public CollisionDetector()
        {
            node = new Dictionary<T, CollisionDetector<T>>();
        }
        public void Add(IEnumerable<T> sample)
        {
            var current = node;
            foreach (T item in sample)
            {
                if (!current.ContainsKey(item))
                {
                    current[item] = new CollisionDetector<T>();
                }
                current = current[item].node;
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            var current = node;
            foreach (T item in pattern)
            {
                if (!current.ContainsKey(item))
                {
                    return;
                }
                current = current[item].node;
            }

            Detected?.Invoke();

        }
    }
}
