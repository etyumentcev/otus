namespace Spacebattle
{
    internal class PrefixTreeNode<T>
    {
        public PrefixTreeNode(T value, bool isEndNode) 
        {
            Value = value;
            IsEndNode = isEndNode;

            Children = new Dictionary<T, PrefixTreeNode<T>>();
        } 
        public T Value { get; }

        public bool IsEndNode { get; }

        public Dictionary<T, PrefixTreeNode<T>> Children { get; }

        public PrefixTreeNode<T> AddChildren(T value, bool isEndNode)
        {
            Children.Add(value, new PrefixTreeNode<T>(value, isEndNode));

            return Children[value];
        }
    }
}
