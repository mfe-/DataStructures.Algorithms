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
        
        //IEdge addEdge(Vertex pu);
        //IEdge addEdge(Vertex pu, int pweighted);
        //void addEdge(Vertex pu, int pweighted, bool undirected);
        //void removeEdge(Vertex pu);
        //void removeEdge(Vertex pu, bool directed);
    }
}
