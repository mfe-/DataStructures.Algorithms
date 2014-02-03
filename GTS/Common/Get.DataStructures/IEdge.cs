using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public interface IEdge<W,T,D> 
        where W : IComparable<W>
        where T : IVertex<W,D>
    {
        /// <summary>
        /// Weight of vertex
        /// </summary>
        W Weight { get; set; }

        IVertex<W, D> U { get; set; }
        IVertex<W, D> V { get; set; }
    }
}
