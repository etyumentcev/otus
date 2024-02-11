using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spacebattle
{
    public delegate void DetectHandler();
    
    public class CollisionDetector<T> where T : struct
    {
        private Node<T> _root = new(default);

        //public event DetectHandler? Detected;  
        public event Action? Detected;
        
        public void Add(IEnumerable<T> vector)
        {
            var currentNode = _root;

            foreach (var coord in vector)
            {
                if (!currentNode.Childs.ContainsKey(coord))
                {
                    currentNode.Childs.Add(coord, new Node<T>(coord));
                }
                currentNode = currentNode.Childs[coord];
            }
        }

        public void Detect(IEnumerable<T> pattern)
        {
            var currentNode = _root;

            foreach (var coord in pattern)
            {
                if (!currentNode.Childs.ContainsKey(coord))
                {
                    return;
                }
                currentNode = currentNode.Childs[coord];
            }
            Detected?.Invoke();
        }
    }

    public class Node<T> where T : struct
    {
        public T Value { get; set; }

        public Dictionary<T, Node<T>> Childs { get; set; }
        
        public Node(T value)
        {
            Value = value;
            Childs = new Dictionary<T, Node<T>>();
        }
    } 
}
