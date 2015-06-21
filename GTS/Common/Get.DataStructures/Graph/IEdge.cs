using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    public interface IEdge<W, D> : IData<D>
        where W : IComparable<W>
    {
        /// <summary>
        /// Weight of vertex
        /// </summary>
        W Weight { get; set; }

        IVertex<W, D> U { get; set; }
        IVertex<W, D> V { get; set; }
    }
}
