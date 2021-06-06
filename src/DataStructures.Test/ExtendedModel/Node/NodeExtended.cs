using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure.Test.ExtendedModel
{
    public class NodeExtended<T> : Node<T>, INodeExtended<T>
    {
        public NodeExtended(T data) : base(data) { }
        public int Amount
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
