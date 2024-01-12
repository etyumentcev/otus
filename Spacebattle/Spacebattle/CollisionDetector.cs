namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        public event Action Detected;

        readonly Dictionary<T, CollisionDetector<T>> _node;

        public CollisionDetector()
        {
            _node = new Dictionary<T, CollisionDetector<T>>();
        }

        public void Add(IEnumerable<T> sample)
        {
            var cur = this;
            foreach (var s in sample)
            {
                if (!cur._node.ContainsKey(s))
                    cur._node[s] = new CollisionDetector<T>();


                cur = cur._node[s];
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            var cur = this;
            foreach (var p in pattern)
            {
                if (!cur._node.ContainsKey(p)) return;
                cur = cur._node[p];
            }
            Detected.Invoke();
        }
    }
}