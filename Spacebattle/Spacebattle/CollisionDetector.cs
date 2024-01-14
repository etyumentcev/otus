namespace Spacebattle
{
    internal delegate PrefixTreeNode<T> GetNextNode<T>(PrefixTreeNode<T> currentNode, T nextValue);

    public class CollisionDetector<T>
    {
        public event Action OnCollisionDetected;

        private PrefixTreeNode<T> _treeRootNode;

        public CollisionDetector()
        {
            _treeRootNode = new PrefixTreeNode<T>(default, GetIntermediateNode);
        }

        public void Add(IEnumerable<T> sample)
        {
            var currentNode = _treeRootNode;
            for (var i = 0; i < sample.Count(); i++)
            {
                if (i == sample.Count() - 1)
                {
                    currentNode.AddChildren(sample.ElementAt(i), GetEndNode);
                    break;
                }

                var child = currentNode.GetNextNodeFunc(currentNode, sample.ElementAt(i));

                if (child != null)
                { 
                    currentNode = child;
                    continue;
                }

                currentNode = currentNode.AddChildren(sample.ElementAt(i), GetIntermediateNode);
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            var currentNode = _treeRootNode;
            var i = 0;
            while (currentNode != null)
            {
                var child = currentNode.GetNextNodeFunc(currentNode, i < pattern.Count() ? pattern.ElementAt(i) : default);

                if (child != null)
                {
                    currentNode = child;
                    i++;
                    continue;
                }

                break;
            }

        }

        private PrefixTreeNode<T> GetIntermediateNode(PrefixTreeNode<T> currentNode, T nextValue)
        {
            if (currentNode.Children.Any(_ => _.Key.Equals(nextValue)))
            {
                var child = currentNode.Children.Where(_ => _.Key.Equals(nextValue)).FirstOrDefault();
                return child.Value;
            }

            return null;
        }

        private PrefixTreeNode<T> GetEndNode(PrefixTreeNode<T> currentNode, T nextValue)
        {
            OnCollisionDetected?.Invoke();

            return null;
        }
    }
}
