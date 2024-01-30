namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        private Dictionary<T, CollisionDetector<T>> nodes;
        public event Action Detected;

        public CollisionDetector()
        {
            nodes = new Dictionary<T, CollisionDetector<T>>();
        }

        public void Add(IEnumerable<T> sample)
        {
            CollisionDetector<T> current = this;

            foreach (var item in sample)
            {
                if (!current.nodes.ContainsKey(item))
                {
                    current.nodes[item] = new CollisionDetector<T>();
                }
                current = current.nodes[item];
            }
            current.Detected += () => Detected?.Invoke();
        }

        public void Detect(IEnumerable<T> pattern)
        {
            CollisionDetector<T> current = this;

            foreach (var item in pattern)
            {
                if (current.nodes.ContainsKey(item))
                {
                    current = current.nodes[item];
                }
                else
                {
                    return;
                }
            }
            current.Detected?.Invoke();
        }
    }
}
