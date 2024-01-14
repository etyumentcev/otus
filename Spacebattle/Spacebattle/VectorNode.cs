namespace Spacebattle
{
    internal class VectorNode<T>
    {
        public VectorNode(T data)
        {
            Value = data;
            Children = new Dictionary<T, VectorNode<T>>();
        }

        public void AddChild(VectorNode<T> child) => Children.Add(child.Value, child);

        public T Value { get; set; }

        public Dictionary<T, VectorNode<T>> Children { get; set; }
    }
}
