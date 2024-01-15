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

        public TrieTree<T> tree;

        public CollisionDetector()
        {
            tree = new TrieTree<T>();
        }

        public void Add(IEnumerable<T> sample)
        {
            TrieTree<T> currentNode = tree;

            foreach (var branch in sample)
            {
                if (!currentNode.Branch.ContainsKey(branch))
                {
                    currentNode.AddBranch(branch, new TrieTree<T>());
                }

                currentNode = currentNode.Branch.ContainsKey(branch) ? currentNode.Branch[branch] : null;
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            TrieTree<T> currentNode = tree;

            foreach (var key in pattern)
            {
                currentNode = currentNode.Branch.ContainsKey(key) ? currentNode.Branch[key] : null;

                if (currentNode == null)
                {
                    return;
                }
            }
            Detected?.Invoke();
        }
    }

    public class TrieTree<T>
    {
        public Dictionary<T, TrieTree<T>> Branch;

        public TrieTree()
        {
            Branch = new Dictionary<T, TrieTree<T>>();
        }

        public void AddBranch(T key, TrieTree<T> branch)
        {
            Branch[key] = branch;
        }
    }
}
