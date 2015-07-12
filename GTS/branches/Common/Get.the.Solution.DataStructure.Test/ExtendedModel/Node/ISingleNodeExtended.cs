using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure.Test.ExtendedModel
{
    public interface ISingleNodeExtendedISingleNode<D, T> : ISingleNode<D, T> where T : ISingleNode<D, T>
    {
        int Amount {  get; set; }
    }
}
