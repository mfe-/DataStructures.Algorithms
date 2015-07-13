using System;
using System.Diagnostics;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.DataStructure
{
    [DebuggerDisplay("Data={Value},Right={Right},Left={Left}")]
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

        public INode<T> Left
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
        public new INode<T> Right
        {
            get;
            set;
        }
    }
}