using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        private TrieNode<T> root = new TrieNode<T>();

        public event Action Detected;

        public void Add(IEnumerable<T> sample)
        {
            TrieNode<T> currentNode = root;

            foreach (var value in sample)
            {
                if (!currentNode.Children.ContainsKey(value))
                {
                    currentNode.AddChild(value, new TrieNode<T>());
                }

                currentNode = currentNode.GetChild(value);
            }

            currentNode.Detected += () => OnDetected();
        }

        public void Detect(IEnumerable<T> pattern)
        {
            TrieNode<T> currentNode = root;

            foreach (var value in pattern)
            {
                currentNode = currentNode.GetChild(value);

                if (currentNode == null)
                {
                    return; // No match found
                }

                currentNode.TriggerDetected();
            }
        }

        private void OnDetected()
        {
            Detected?.Invoke();
        }
    }
    
    public class TrieNode<T>
    {
        public Dictionary<T, TrieNode<T>> Children { get; private set; }
        public event Action Detected;

        public TrieNode()
        {
            Children = new Dictionary<T, TrieNode<T>>();
        }

        public void AddChild(T key, TrieNode<T> child)
        {
            Children[key] = child;
        }

        public TrieNode<T> GetChild(T key)
        {
            return Children.ContainsKey(key) ? Children[key] : null;
        }

        public void TriggerDetected()
        {
            Detected?.Invoke();
        }
    }
}
