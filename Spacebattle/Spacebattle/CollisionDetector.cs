using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        public delegate void DetectedHandler<T>(T val);

        public DetectedHandler<T> Detected;

        private Dictionary<T, CollisionDetector<T>> node;

        public CollisionDetector()
        {
            node = new Dictionary<T, CollisionDetector<T>>();
        }
        
        public void Add(IEnumerable<T> sample)
        {
            var currentNode = this;
            foreach (var val in sample)
            {
                if (!currentNode.node.ContainsKey(val))
                {
                    currentNode.node[val] = new CollisionDetector<T>();
                }
                currentNode = currentNode.node[val];
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            var currentNode = this;
            foreach (var val in pattern)
            {
                if (currentNode.node.ContainsKey(val))
                {
                    currentNode = currentNode.node[val];

                    if (!currentNode.node.Any())
                    {
                        // Коллизия - вызываем событие Detected
                        Detected?.Invoke(val);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }
}