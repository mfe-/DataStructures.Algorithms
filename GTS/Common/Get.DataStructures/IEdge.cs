using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public interface IEdge<W, T>
        where W : IComparable<W>
    {
        W Weight { get; set; }

        IVertex<W, T> U { get; set; }
        IVertex<W, T> V { get; set; }

        //        Node.U Node.V

        //IEdge Ö INode, ICompareable
        //Weight
        //Compareable soll value von Weight nehmen


    }
}
