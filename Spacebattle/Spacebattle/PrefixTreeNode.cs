namespace Spacebattle
{
    internal class PrefixTreeNode<T>
    {
        public PrefixTreeNode(T value, GetNextNode<T> getNextNodeFunc) 
        {
            Value = value;
            GetNextNodeFunc = getNextNodeFunc;

            Children = new Dictionary<T, PrefixTreeNode<T>>();
        } 
        public T Value { get; }

        public GetNextNode<T> GetNextNodeFunc { get; }

        public Dictionary<T, PrefixTreeNode<T>> Children { get; }

        public PrefixTreeNode<T> AddChildren(T value, GetNextNode<T> getNextNodeFunc)
        {
            Children.Add(value, new PrefixTreeNode<T>(value, getNextNodeFunc));

            return Children[value];
        }
    }
}
