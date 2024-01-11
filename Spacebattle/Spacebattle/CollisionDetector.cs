using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        public event Action Detected;

        private Dictionary<T, CollisionDetector<T>> node;

        public CollisionDetector()
        {
            node = new Dictionary<T, CollisionDetector<T>>();
        }

        public void Add(IEnumerable<T> sample)
        {
            var currentNode = this;
            foreach (var item in sample)
            {
                if (!currentNode.node.ContainsKey(item))
                {
                    currentNode.node[item] = new CollisionDetector<T>();
                }

                currentNode = currentNode.node[item];
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            var currentNode = this;
            foreach (var item in pattern)
            {
                if (!currentNode.node.ContainsKey(item))
                {
                    return;
                }

                currentNode = currentNode.node[item];
            }

            Detected?.Invoke();
        }
    }
}
