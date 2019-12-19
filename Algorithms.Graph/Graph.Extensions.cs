using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Graph
{
    /// <summary>
    /// Provides a set of static methods for working with graph objects.
    /// </summary>
    public static partial class GraphExtensions
    {
        public static bool IsDirected(this DataStructures.Graph g)
        {
            //schauen ob alle vertex jeweils 2mal verbudnen sind also 1->2 und 2->1 nur dann ist es directed=false ansonsten directed=true
            //Schlichte ungerichtete Graphen haben daher eine symmetrische Adjazenzmatrix.
            //es muss daher von i nach j eine kantegeben und von j nach i, das entspricht aij=aji
            //also ist der graph genau dann ungerichtet wenn die matrix symmetrisch ist
            //http://en.wikipedia.org/wiki/Transpose
            //a transportieren also a^t = a symmetrisch
            int[][] matrix = g.AdjacencyList();
            int[][] matrixT = new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                matrixT[i] = new int[matrix.Length];
            }

            //transportieren
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    matrixT[j][i] = matrix[i][j];

            //check if result1 = matrixT1 --> symmetry --> v1->v2 & v1<-v2 = undirected
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] != matrixT[i][j]) return true;
                }

            return false;
        }
        public static IEnumerable<IVertex> Sort(this IEnumerable<IVertex> l)
        {
            return l.OrderBy(a => a.Weighted).ThenBy(a => a.Size).ThenBy(a => a.GetHashCode());
        }
        /// <summary>
        /// Converts a edge list to a vertices list
        /// </summary>
        /// <param name="l">The edge list</param>
        /// <returns></returns>
        public static IEnumerable<IVertex> ToVertexList(this IEnumerable<IEdge> l)
        {
            return l.SelectMany(a => new[] { a.U, a.V }).Distinct();
        }
        /// <summary>
        /// The undirected graph contains for two vertices two edges. 
        /// Pass one of the edge to get the opposite edge of it.
        /// U->V V->U
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public static IEdge GetOppositeEdge(this IEdge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            return (edge.V.Edges.FirstOrDefault(a => a.V.Equals(edge.U)));
        }
        /// <summary>
        /// Returns all vertices from the graph
        /// http://www.brpreiss.com/books/opus4/html/page551.html
        /// http://www.cse.ohio-state.edu/~gurari/course/cis680/cis680Ch14.html#QQ1-46-90
        /// </summary>
        /// <param name="s">Root IVertex of graph</param>
        /// <returns>All reachable vertices</returns>
        public static IEnumerable<IVertex> DepthFirstTraversal(this DataStructures.Graph s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            List<IVertex> l = new List<IVertex>();
            foreach (IVertex v in s.Vertices)
                l.AddRange(DepthFirstTraversal(v, new List<IVertex>()));
            return l;
        }
        private static List<IVertex> DepthFirstTraversal(this IVertex s, List<IVertex> visited)
        {
            //visist x
            visited.Add(s);

            //FOR each y such that (x,y) is an IEdge DO 
            foreach (IEdge e in s.Edges)
            {
                if (!visited.Contains(e.V))
                {
                    visited = DepthFirstTraversal(e.V, visited);
                }
            }
            return visited;
        }
        public static IEnumerable<IVertex> DephFirstSearch(this IVertex s)
        {
            List<IVertex> visited = new List<IVertex>();

            Stack<IVertex> stack = new Stack<IVertex>();

            stack.Push(s);

            while (stack.Count != 0)
            {
                IVertex v = stack.Pop();
                visited.Add(v);
                foreach (IEdge e in v.Edges)
                {
                    if (!visited.Contains(e.V))
                    {
                        stack.Push(e.V);
                    }
                }
            }

            return visited;
        }
        public static List<IEdge> BreadthFirstSearch(this IVertex start, IVertex target)
        {
            List<IEdge> l = new List<IEdge>();
            //TODO
            //Stack<IVertex> stack = new Stack<IVertex>();

            //IVertex current = start;

            //for (int i = 1; i <= current.IEdges.Count; i++)
            //{
            //    IVertex v = current.IEdges[i-1].V;
            //    if (v.Equals(target))
            //    {
            //        return l;
            //    }
            //    else if (i.Equals(current.IEdges.Count) && !v.Equals(target))
            //    {
            //        i = 1;
            //        stack.Push(v);
            //        current = stack.Pop();

            //    }
            //    else
            //    {
            //        stack.Push(v);
            //    }

            //}

            return l;


        }
        //http://www.informatik-forum.at/showthread.php?80262-Aufgabe-29 check auf bipartit graph

        /// <summary>
        /// Sei G ein Graph mit Knotenmengen V(G). Die Adjazenzmatrix A(G) ist eine qudratische nxn n-Matrix 
        /// mit aij = Fallunterscheidung ist vi und vj mit einer Kante verbunden 1 ansonsten 0
        /// http://en.wikipedia.org/wiki/Adjacency_list
        /// </summary>
        /// <param name="g">Graph on which the adjacency list should be created</param>
        /// <returns></returns>
        public static int[][] AdjacencyList(this DataStructures.Graph g)
        {
            var vertices = DepthFirstTraversal(g).Sort().Distinct().ToArray();
            var edges = vertices.SelectMany(a => a.Edges).Distinct<IEdge>();
            //create matrix
            int c = vertices.Length;
            int[][] m = new int[c][];
            for (int o = 0; o < c; o++)
            {
                int[] row = new int[c];
                for (int y = 0; y < c; y++)
                {
                    IVertex i = vertices[o];
                    IVertex j = vertices[y];

                    row[y] = !edges.Any(b => b.V.Equals(i) && b.U.Equals(j)) ? 0 : 1;
                }

                m[o] = row;
            }

            return m;
        }

        public static DataStructures.Graph KruskalDepthFirstSearch(this DataStructures.Graph g)
        {
            //works only with undircted graphs
            if (g.Directed.Equals(true))
                throw new DirectedException(false);

            //create g'
            DataStructures.Graph g_ = g;

            List<IVertex> vertices = new List<IVertex>();
            //order IEdges by pyramiding weighted
            IEdge[] IEdges = DepthFirstTraversal(g).SelectMany(a => a.Edges).OrderBy(e => e.Weighted).Distinct(new EdgeExtensions.EdgeComparer()).ToArray();
            //remove IEdges 
            foreach (IVertex z in g_.DepthFirstTraversal())
            {
                vertices.Add(z);

                z.Edges.Clear();
            }

            int weight = 0;
            for (int i = 0; i < IEdges.Length; i++)
            {
                IEdge e = IEdges[i];
                weight = weight + e.Weighted;
                IVertex u = vertices.First(a => a.Equals(e.U));
                IVertex v = vertices.First(a => a.Equals(e.V));
                //add 2x IEdges to create a undirected graph
                u.AddEdge(v, e.Weighted);
                v.AddEdge(u, e.Weighted);
                //check if circle

                var o = DepthFirstSearch(u, u);

                if (o.First().U.Equals(u) && o.Last().V.Equals(u))
                {
                    u.Edges.Remove(u.Edges.Last());
                    v.Edges.Remove(v.Edges.Last());
                    weight = weight - e.Weighted;
                }
            }

            return g_;
        }
        /// <summary>
        /// Searches from the <paramref name="start"/> the overgiven <paramref name="goal"/>
        /// http://en.wikipedia.org/wiki/Depth-first_search
        /// </summary>
        /// <param name="start">Where the look up should start</param>
        /// <param name="goal">Which IVertex should be found. When no goal supplied, the algorithm will visit all edges which exists in the graph.</param>
        /// <param name="graphIsUndirected">Determines whether the graph was directed. Default is false (undirected)</param>
        /// <returns>Returns a list of all <see cref="IEdge"/>s which are required to get the path beginning from the start to the goal</returns>
        public static IEnumerable<IEdge> DepthFirstSearch(this IVertex start, IVertex goal = null, bool graphIsdirected = true)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (graphIsdirected)
            {
                return DepthFirstSearchDirected(start, new List<IEdge>(), goal);
            }
            else
            {
                return DepthFirstSearchUndirected(start, new List<IEdge>(), goal);
            }
        }
        private static IEnumerable<IEdge> DepthFirstSearchUndirected(IVertex current, List<IEdge> edges, IVertex goal)
        {
            foreach (IEdge e in current.Edges)
            {
                if (!edges.Any(a => a.Equals(e)))
                {
                    //mark edges
                    edges.Add(e);
                    DepthFirstSearchUndirected(e.V, edges, goal);
                }
                if (edges.Any() && edges.Last().V.Equals(goal)) return edges;
            }

            return edges;
        }
        private static IEnumerable<IEdge> DepthFirstSearchDirected(IVertex current, List<IEdge> edges, IVertex goal)
        {
            foreach (IEdge e in current.Edges)
            {
                //check if already visited IVertex 
                //(use == operator instead of Equals for directed graphs,
                //as the overriden equals of the Edge implementeation returns true for transposed edges)
                if (!edges.Any(a => a == e))
                {
                    //mark edges
                    edges.Add(e);
                    DepthFirstSearchDirected(e.V, edges, goal);
                }
                if (edges.Any() && edges.Last().V.Equals(goal)) return edges;
            }
            return edges;
        }

        /// <summary>
        /// Determinds if the overgiven <paramref name="v"/> is adjacent to the current IVertex
        /// </summary>
        /// <param name="v">the IVertex to check</param>
        /// <returns>True if the overgiven IVertex is adjacent</returns>
        public static Boolean Adjacent(this IVertex v)
        {
            if (v == null) throw new ArgumentNullException(nameof(v));
            return !v.Edges.Any(a => a.U.Equals(v) || a.V.Equals(v));
        }
        /// <summary>
        /// Calculates the distance by summing the weighted of the IEdges.
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static int Distance(this IEnumerable<IEdge> edges)
        {
            int distance = 0;
            if (edges == null) return distance;

            foreach (IEdge e in edges)
                distance = +e.Weighted + distance;

            return distance;
        }


        public static object Connected(this DataStructures.Graph g, IVertex a, IVertex b)
        {
            // Eine Folge von karten e1,e2,...ek e E(G) eines ungerichteten G heißt katenfolge , wenn es knoten v,v1,v2,...vk1 w eV(G) mit 
            //% gibt ,d.h. man kann die katen e1,2,...ek,ohne absetzen durchlafeun. k... anzahl der kanten
            //Tiefensuchen hier nehmen Katenfolge von a -> b zurück geben
            return null;
        }
        //public static bool Connected(this Graph g, IVertex a, IVertex b)
        //{
        //    // Eine Folge von karten e1,e2,...ek e E(G) eines ungerichteten G heißt katenfolge , wenn es knoten v,v1,v2,...vk1 w eV(G) mit 
        //    //% gibt ,d.h. man kann die katen e1,2,...ek,ohne absetzen durchlafeun. k... anzahl der kanten
        //    //Tiefensuchen hier nehmen Katenfolge von a -> b zurück geben
        //    //return g.Connected(a,b) ==null ? false : true;
        //    return false;
        //}
    }
}
