using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Spacebattle
{
    public class CollisionDetector<T>
    {
        private class TreeNode<T>
        {
            public delegate TreeNode<T>? NextDelegate<T>(T val);
            public bool NeedDetected { get; private set; }
            public NextDelegate<T> Next { get; set; }
            public Dictionary<T, TreeNode<T>> Nodes { get; set; } = new Dictionary<T, TreeNode<T>>();

            public TreeNode(bool needDetected = false)
            {
                NeedDetected = needDetected;
            }
        }

        private TreeNode<T> Node { get; set; }

        public event Action? OnDetected;

        private TreeNode<T> CreateNode(bool detect = false)
        {
            var newNode = new TreeNode<T>(detect);
            newNode.Next = (T) =>
            {
                if(newNode.Nodes.TryGetValue(T, out var foundNode) && foundNode.NeedDetected)
                    OnDetected?.Invoke();

                return foundNode;
            };

            return newNode;
        }

        public CollisionDetector()
        {
            Node = CreateNode();
        }

        public void Add(IEnumerable<T> sample)
        {
            var curNode = Node;
            for (var i = 0; i < sample.Count(); i++)
            {
                var item = sample.ElementAt(i);
                if (i == sample.Count() - 1)
                {
                    if (curNode.Next(item) == null)
                        curNode.Nodes.Add(item, CreateNode(true));
                }
                else
                {
                    var foundNode = curNode.Next(item);
                    if (foundNode != null)
                        curNode = foundNode;
                    else
                    {
                        var newNode = CreateNode();
                        curNode.Nodes.Add(item, newNode);
                        curNode = newNode;
                    }
                }
            }

        }

        public void Detect(IEnumerable<T> pattern)
        {
            var curNode = Node;
            foreach (var item in pattern)
            {
                var foundNode = curNode.Next(item);
                if (foundNode != null)
                    curNode = foundNode;
                else
                    return;
            }
        }
    }
}
