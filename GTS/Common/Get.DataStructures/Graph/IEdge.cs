using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public interface IEdge<W, T, D> : IData<D>
        where W : IComparable<W>
        where T : IVertex<W, D>
    {
        /// <summary>
        /// Weight of vertex
        /// </summary>
        W Weight { get; set; }

        T U { get; set; }
        T V { get; set; }
    }
}
