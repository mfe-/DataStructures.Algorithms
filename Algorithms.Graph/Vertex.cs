using System.Collections.Generic;
using System.Linq;
using DataStructures;

namespace Algorithms.Graph
{
    public static partial class VertexAlgorithms
    {
        public static IList<IVertex> Depth_First_Traversal(this IVertex s, IList<IVertex> visited)
        {
            //visist x
            visited.Add(s);

            //FOR each y such that (x,y) is an edge DO 
            foreach (IEdge e in s.Edges)
            {
                if (!visited.Contains(e.V))
                {
                    visited = Depth_First_Traversal(e.V, visited);
                }
            }
            return visited;
        }
    }
}
