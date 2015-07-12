using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    public class SingleNode<T> : ISingleNode<T, SingleNode<T>>
    {
        public SingleNode()
        {

        }
        public SingleNode(T data)
        {
            this.Value = data;
        }
        public SingleNode(T data, SingleNode<T> node)
            : this(data)
        {
            this.SetRight(node);
        }
        public virtual T Value
        {
            get;
            set;
        }
        private SingleNode<T> Right;
        public virtual SingleNode<T> GetRight()
        {
            return this.Right;
        }

        public virtual void SetRight(SingleNode<T> t)
        {
            this.Right = t;
        }
    }
}
