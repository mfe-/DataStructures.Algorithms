using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Single Node Interfaces
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <typeparam name="T"></typeparam>
    public interface ISingleNode<D> : IData<D>, IEnumerable<D>
    {
        /// <summary>
        /// Get or sets the right node
        /// </summary>
        ISingleNode<D> Right { get; set; }
    }
}
