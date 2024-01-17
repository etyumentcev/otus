using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        Trie<int> trie;

        private List<IEnumerable<T>> samples;
        public event Action? CollisionDetected;

        public CollisionDetector()
        {
            trie = new Trie<int>();
            samples = new List<IEnumerable<T>>();
        }

        public void Add(IEnumerable<T> sample)
        {
            IEnumerable<int> i = (IEnumerable<int>)sample;
            trie.Add(i);
        }

        public void Detect(IEnumerable<T> pattern)
        {
            if (trie.Detect((IEnumerable<int>)pattern))
            {
                OnCollisionDetected();
            }
        }

        void OnCollisionDetected()
        {
            CollisionDetected?.Invoke();
        }

    }
    public class TrieNode<T>
    {
        public Dictionary<T, TrieNode<T>> Children { get; set; }
        public bool IsEndOfWord { get; set; }

        public TrieNode()
        {
            Children = new Dictionary<T, TrieNode<T>>();
            IsEndOfWord = false;
        }
    }

    public class Trie<T>
    {
        private TrieNode<T> root;

        public Trie()
        {
            root = new TrieNode<T>();
        }

        public void Add(IEnumerable<T> values)
        {
            TrieNode<T> node = root;
            foreach (T value in values)
            {
                if (!node.Children.ContainsKey(value))
                {
                    node.Children[value] = new TrieNode<T>();
                }
                node = node.Children[value];
            }
            node.IsEndOfWord = true;
        }

        public bool Detect(IEnumerable<T> value)
        {
            TrieNode<T> node = root;
            foreach (T val in value)
            {
                if (val is int)
                {

                    if (!node.Children.ContainsKey(val))
                    {
                        return false;
                    }
                    node = node.Children[val];
                }
            }
            return node.IsEndOfWord;
        }
    }



}
