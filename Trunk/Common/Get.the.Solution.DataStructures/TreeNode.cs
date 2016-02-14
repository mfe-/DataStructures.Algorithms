using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.DataStructure
{
    [DebuggerDisplay("Data={Value},Right={TreeNode.Right},Left={TreeNode.Left}")]
    public class TreeNode<T> : Node<T>, ITreeNode<T>
    {
        public TreeNode(T value)
            : base(value)
        {
            this.Parent = null;
        }
        public TreeNode(T data, INode<T> left, INode<T> right)
            : this(data)
        {
            Left = (ITreeNode<T>)left;
            Right = (ITreeNode<T>)right;

        }
        public TreeNode(T data, ITreeNode<T> parent, INode<T> left, INode<T> right)
            : this(data, left, right)
        {
            Parent = parent;
        }
        public ITreeNode<T> Parent
        {
            get;
            set;
        }
        private ITreeNode<T> left;
        public virtual new ITreeNode<T> Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
                //because the base type is hide we assign it manual
                base.Left = value;
            }
        }
        private new ITreeNode<T> right;
        public virtual new ITreeNode<T> Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
                //because the base type is hide we assign it manual
                base.Right = value;
            }
        }
    }
}
