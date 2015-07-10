using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure.Test.ExtendedModel
{
    public class SingleNodeExtended<T> : ISingleNodeExtendedISingleNode<T, SingleNodeExtended<T>>
    {
        public SingleNodeExtended<T> Right { get; set; }

        public SingleNodeExtended<T> GetRight()
        {
            return this.Right;
        }

        public void SetRight(SingleNodeExtended<T> t) 
        {
            this.Right = t;
        }

        public T Value
        {
            get;
            set;
        }
        public int Amount
        {
            get;
            set;
        }
    }
}
