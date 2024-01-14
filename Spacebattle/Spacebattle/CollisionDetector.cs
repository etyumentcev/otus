namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        private readonly VectorNode<T> vectorTrie = new(default);

        public delegate void CollisionDetectedHandler();

        public event CollisionDetectedHandler OnCollisionDetected;

        public void Add(IEnumerable<T> sample)
        {
            var currentLevel = vectorTrie;
            for (var i = 0; i < sample.Count(); i++)
            {
                var element = sample.ElementAt(i);
                if (!currentLevel.Children.ContainsKey(element))
                {
                    currentLevel.Children.Add(element, new VectorNode<T>(element));
                }
                currentLevel = currentLevel.Children[element];
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            var currentLevel = vectorTrie;
            for (var i = 0; i < pattern.Count(); i++)
            {
                var element = pattern.ElementAt(i);
                if (!currentLevel.Children.ContainsKey(element))
                {
                    return;
                }
                currentLevel = currentLevel.Children[element];
            }

            OnCollisionDetected.Invoke();
        }
    }
}
