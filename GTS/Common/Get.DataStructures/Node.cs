using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public class Node<T> : INode<T>
    {
        public Node()
            : base()
        {
        }
        public Node(T Data)
        {
            this.Value = Data;
        }
        public Node(T Data, INode<T> Left)
        {
            this.Value = Data;
            this.Left = Left;
        }

        public Node(T Data, INode<T> Left, INode<T> Right)
        {
            this.Value = Data;
            this.Left = Left;
            this.Right = Right;
        }
        public T Value
        {
            get;
            set;
        }

        public INode<T> Parent
        {
            get;
            set;
        }

        public INode<T> Left
        {
            get;
            set;
        }

        public INode<T> Right
        {
            get;
            set;
        }
    }
}
