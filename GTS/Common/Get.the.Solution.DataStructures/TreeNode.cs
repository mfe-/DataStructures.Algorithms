using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.DataStructure
{
    public class TreeNode<T> : Node<T>, ITreeNode<T>
    {
        public TreeNode(T value) : base(value)
        {

        }
        public ITreeNode<T> Parent
        {
            get;
            set;
        }

        public new ITreeNode<T> Left
        {
            get;
            set;
        }
        public new ITreeNode<T> Right
        {
            get;
            set;
        }
    }
}
