using System;
using System.Collections.Generic;

public class CollisionDetector<T>
{
    public event EventHandler Detected;

    private readonly Dictionary<T, CollisionNode<T>> root;

    public CollisionDetector()
    {
        root = new Dictionary<T, CollisionNode<T>>();
    }

    public void Add(IEnumerable<T> sample)
    {
        Dictionary<T, CollisionNode<T>> currentNode = root;

        foreach (var item in sample)
        {
            if (!currentNode.ContainsKey(item))
            {
                currentNode[item] = new CollisionNode<T>();
            }

            currentNode = currentNode[item].Children;
        }

        currentNode[default(T)] = new CollisionNode<T>(true);
    }

    public void Detect(IEnumerable<T> pattern)
    {
        Dictionary<T, CollisionNode<T>> currentNode = root;

        foreach (var item in pattern)
        {
            if (!currentNode.ContainsKey(item))
            {
                return;
            }

            currentNode = currentNode[item].Children;
        }

        if (currentNode.ContainsKey(default(T)))
        {
            Detected?.Invoke(this, EventArgs.Empty);
        }
    }

    private class CollisionNode<T>
    {
        public Dictionary<T, CollisionNode<T>> Children { get; }

        public bool IsLeaf { get; }

        public CollisionNode(bool isLeaf = false)
        {
            Children = new Dictionary<T, CollisionNode<T>>();
            IsLeaf = isLeaf;
        }
    }
}