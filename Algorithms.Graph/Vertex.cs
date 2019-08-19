using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.Algorithms.Graph
{
    public static partial class VertexAlgorithms
    {
        public static IEnumerable<IVertex<W, D>> Depth_First_Traversal<W, D>(this IVertex<W, D> s, IEnumerable<IVertex<W, D>> visited)
            where W : IComparable<W>
        {
            //visist x
            visited.ToList<IVertex<W, D>>().Add(s);

            //FOR each y such that (x,y) is an edge DO 
            foreach (IEdge<W, D> e in s.Edges)
            {
                if (!visited.Contains(e.V))
                {
                    visited = Depth_First_Traversal<W, D>(e.V, visited).ToList<IVertex<W, D>>();
                }
            }
            return visited;
        }
    }
}
