using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure.Test.ExtendedModel
{
    public interface ISingleNodeExtendedISingleNode<D> : ISingleNode<D>
    {
        int Amount {  get; set; }
    }
}
