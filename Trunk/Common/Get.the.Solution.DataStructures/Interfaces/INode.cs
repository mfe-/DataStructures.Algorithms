using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// INode interface
    /// </summary>
    /// <typeparam name="D"></typeparam>
    public interface INode<D> : ISingleNode<D>
    {
        /// <summary>
        /// Get or set the left node
        /// </summary>
        INode<D> Left { get; set; }
        /// <summary>
        /// Get or sets the right node
        /// </summary>
        new INode<D> Right { get; set; }
    }


}
