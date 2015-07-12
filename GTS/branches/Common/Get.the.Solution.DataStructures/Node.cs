using System;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.DataStructure
{
    public class Node<T> : SingleNode<T>, INode<T, Node<T>>
    {
        public Node()
            : base()
        {

        }
        public Node(T data)
            : base(data)
        {

        }
        public Node(T data, Node<T> left, Node<T> right)
            : base(data, left)
        {
            this.SetRight(right);
        }
        public virtual Node<T> GetLeft()
        {
            return this.Left;
        }

        public virtual void SetLeft(Node<T> t)
        {
            this.Left = t;
        }

        public new Node<T> GetRight()
        {
            return this.Right;
        }

        public void SetRight(Node<T> t)
        {
            this.Right = t;
        }

        private Node<T> Left;
        private Node<T> Right;
    }
}