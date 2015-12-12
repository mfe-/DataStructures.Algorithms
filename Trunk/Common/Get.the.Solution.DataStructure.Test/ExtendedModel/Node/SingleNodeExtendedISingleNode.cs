using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure.Test.ExtendedModel
{
    public class SingleNodeExtended<T> : SingleNode<T> ,  ISingleNodeExtendedISingleNode<T> 
    {
        public int Amount
        {
            get;
            set;
        }
    }
}
