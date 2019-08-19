using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    [DebuggerDisplay("Data={Value},Right={Right}")]
    public class SingleNode<T> : ISingleNode<T>
    {
        protected readonly T _Value;

        public SingleNode(T data)
        {
            _Value = data;
        }

        public SingleNode(T data, ISingleNode<T> right)
            : this(data)
        {
            this.Right = right;
        }

        public virtual T Value
        {
            get { return _Value; }
        }
        protected ISingleNode<T> right;
        public virtual ISingleNode<T> Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection. (Inherited from IEnumerable<T>.)
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            ISingleNode<T> start = this;
            while (start != null)
            {
                yield return start.Value;
                start = start.Right;
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection. (Inherited from IEnumerable<T>.)
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            ISingleNode<T> start = this;
            while (start != null)
            {
                yield return start;
                start = start.Right;
            }
        }
    }
}
