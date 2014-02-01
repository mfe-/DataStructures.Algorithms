using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public interface IEdge<W>
        where W : IComparable<W>
    {
        /// <summary>
        /// Weight of vertex
        /// </summary>
        W Weight { get; set; }
        /// <summary>
        /// Get or sets the vertex of the edge
        /// </summary>
        IVertex<W> U { get; set; }
        /// <summary>
        /// Get or sets the vertex of the edge
        /// </summary>
        IVertex<W> V { get; set; }
    }
}
