namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        public event Action OnCollisionDetected;

        private PrefixTreeNode<T> _treeRootNode;

        public CollisionDetector() 
        {
            _treeRootNode = new PrefixTreeNode<T>(default(T), false);
        }

        public void Add(IEnumerable<T> sample)
        {
            var currentNode = _treeRootNode;
            for (var i = 0; i < sample.Count(); i++)
            {
                if (i == sample.Count() - 1)
                {
                    currentNode.AddChildren(sample.ElementAt(i), true);
                    break;
                }

                if (currentNode.Children.Any(_ => _.Key.Equals(sample.ElementAt(i))))
                {
                    var child = currentNode.Children.Where(_ => _.Key.Equals(sample.ElementAt(i))).FirstOrDefault();
                    currentNode = child.Value;
                    continue;
                }

                currentNode = currentNode.AddChildren(sample.ElementAt(i), false);
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            var currentNode = _treeRootNode;
            for (var i = 0; i <= pattern.Count(); i++)
            {
                if (i == pattern.Count())
                {
                    if (currentNode.IsEndNode)
                    {
                        OnCollisionDetected?.Invoke();
                    }

                    break;
                }

                if (currentNode.Children.Any(_ => _.Key.Equals(pattern.ElementAt(i))))
                {
                    var child = currentNode.Children.Where(_ => _.Key.Equals(pattern.ElementAt(i))).FirstOrDefault();
                    currentNode = child.Value;
                    continue;
                }

                break;
            }

        }
    }
}
