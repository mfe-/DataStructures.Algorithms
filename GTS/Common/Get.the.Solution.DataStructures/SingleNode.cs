using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    [DebuggerDisplay("Data={Value},Right={Right}")]
    public class SingleNode<T> :ISingleNode<T>
    {
        public SingleNode()
        {
        }
        public SingleNode(T data)
        {
            this.Value = data;
        }
        public SingleNode(T data, ISingleNode<T> right)
            : this(data)
        {
            this.Right = right;
        }
        public virtual T Value
        {
            get;
            set;
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
    }
}
