namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        private readonly TrieNode<T> trieNode = new(value: default);

        public delegate void CollisionDetectorHandler();

        public event CollisionDetectorHandler ?OnCollisionDetected;

        public void Add(IEnumerable<T> sample)
        {
            var curNode = trieNode;
            foreach (T item in sample)
            {
                if (!curNode.ChildNodes.ContainsKey(item))
                {
                    curNode.ChildNodes.Add(item, new TrieNode<T>(item));
                }
                curNode = curNode.ChildNodes[item];
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            var curNode = trieNode;
            foreach (T item in pattern)
            {
                if (!curNode.ChildNodes.ContainsKey(item))
                {
                    return;
                }
                curNode = curNode.ChildNodes[item];
            }
            OnCollisionDetected?.Invoke();
        }
    }

    internal class TrieNode<T>
    {
        public TrieNode(T value)
        {
            Value = value;
            ChildNodes = new Dictionary<T, TrieNode<T>>();
        }

        public T Value { get; private set; }

        public Dictionary<T, TrieNode<T>> ChildNodes { get; private set; }
    }
}
