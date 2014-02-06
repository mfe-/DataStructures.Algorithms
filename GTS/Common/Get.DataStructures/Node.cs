using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public class Node<D> : INode<Node<D>, D>
    {
        public Node()
            : base()
        {
        }
        public Node(D Data)
        {
            this.Value = Data;
        }
        public Node(D Data, Node<D> Left)
        {
            this.Value = Data;
            this.Left = Left;
        }

        public Node(D Data, Node<D> Left, Node<D> Right)
        {
            this.Value = Data;
            this.Left = Left;
            this.Right = Right;
        }
        public D Value
        {
            get;
            set;
        }

        public Node<D> Left
        {
            get;
            set;
        }

        public Node<D> Right
        {
            get;
            set;
        }
    }
}
