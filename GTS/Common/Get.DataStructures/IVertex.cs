using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Get.DataStructure
{
    public interface IVertex<W, T> : IData<T> where W : IComparable<W>  
    {
        W Weight { get; set; }

        IEnumerable<IEdge<W,T>> Edges { get; set; }

        int Size { get { return Edges.Count(); } }

        IEdge<W, T> AddEdge(IVertex<W, T> pu, W pweighted, Boolean undirected);

        void RemoveEdge(IVertex<W, T> pu, bool directed);
    }
}
