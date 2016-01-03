using System;
using System.Diagnostics;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Reprsents a node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Data={Value},INode.Right={Right},INode.Left={Left}")]
    public class Node<T> : SingleNode<T>, INode<T>
    {
        public Node()
            : base()
        {

        }
        public Node(T data)
            : base(data)
        {

        }
        public Node(T data, INode<T> left, ISingleNode<T> right)
            : base(data, right)
        {
            this.Left = left;
        }

        public virtual INode<T> Left
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// <remarks>
        /// The base interface can not be assigned because the Right property was redefined with the keyword new.
        /// http://stackoverflow.com/questions/729527/is-it-possible-to-assign-a-base-class-object-to-a-derived-class-reference-with-a
        /// </remarks>
        /// </summary>
        protected new INode<T> right;
        public virtual new INode<T> Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
                //because the base type is hidden we assign it manual
                base.Right = value;
            }
        }
    }
}