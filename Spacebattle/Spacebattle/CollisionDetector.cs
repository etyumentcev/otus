namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        private readonly List<Vector<T>> _vectors = new();

        public delegate void CollisionDetectedHandler();

        public event CollisionDetectedHandler OnCollisionDetected;

        public void Add(IEnumerable<T> sample) => _vectors.Add(new Vector<T>(sample));

        public void Detect(IEnumerable<T> pattern)
        {
            var patternVector = new Vector<T>(pattern);

            foreach (var vector in _vectors)
            {
                if (vector.Equals(patternVector))
                    OnCollisionDetected?.Invoke();
            }
        }
    }
}
