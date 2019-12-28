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
        /// <summary>
        /// Determines whether the graph is directed or undirected.
        /// Uses the <seealso cref="AdjacencyMatrix"/> to generate a matrix and checks whether its a symmetric
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool IsDirected(this DataStructures.Graph g)
        {
            //schauen ob alle vertex jeweils 2mal verbudnen sind also 1->2 und 2->1 nur dann ist es directed=false ansonsten directed=true
            //Schlichte ungerichtete Graphen haben daher eine symmetrische Adjazenzmatrix.
            //es muss daher von i nach j eine kantegeben und von j nach i, das entspricht aij=aji
            //also ist der graph genau dann ungerichtet wenn die matrix symmetrisch ist
            //http://en.wikipedia.org/wiki/Transpose
            //a transportieren also a^t = a symmetrisch
            int[][] matrix = g.AdjacencyMatrix();
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
        /// </summary>
        /// <param name="s">Root IVertex of graph</param>
        /// <returns>All reachable vertices</returns>
        public static IEnumerable<IVertex> DepthFirstTraversal(this DataStructures.Graph s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            IEnumerable<IVertex> l = Enumerable.Empty<IVertex>();
            foreach (IVertex v in s.Vertices)
            {
                var verticeList = DepthFirstSearchStack(v);
                l = l.Union(verticeList);
            }
            return l;
        }
        /// <summary>
        /// DepthFirstSearch implemented as Stack
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IEnumerable<IVertex> DepthFirstSearchStack(this IVertex s)
        {
            HashSet<IVertex> visited = new HashSet<IVertex>();
            Stack<IVertex> stack = new Stack<IVertex>();
            stack.Push(s);
            while (stack.Any())
            {
                IVertex v = stack.Pop();
                if (!visited.Contains(v))
                {
                    visited.Add(v);
                    foreach (IEdge e in v.Edges)
                    {
                        stack.Push(e.V);
                    }
                }
            }
            return visited;
        }
        /// <summary>
        /// Searches from the <paramref name="start"/> the overgiven <paramref name="goal"/>
        /// http://en.wikipedia.org/wiki/Depth-first_search
        /// Completeness: not complete
        /// Admissible: 
        /// </summary>
        /// <param name="start">Where the look up should start</param>
        /// <param name="goal">Which IVertex should be found. When no goal supplied, the algorithm will visit all edges which exists in the graph.</param>
        /// <param name="graphIsUndirected">Determines whether the graph was directed. Default is false (undirected)</param>
        /// <returns>Returns a list of all <see cref="IEdge"/>s which are required to get the path beginning from the start to the goal</returns>
        public static IEnumerable<IEdge> DepthFirstSearch(this IVertex start, IVertex goal = null, bool graphIsDirected = true)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if(graphIsDirected)
            {
                return DepthFirstSearchN(start, new List<IEdge>(), goal, graphIsDirected);
                //return DepthFirstSearchStack(start, goal, graphIsdirected);
            }
            else
            {
                return DepthFirstSearchN(start, new HashSet<IEdge>(), goal, graphIsDirected);
                //return DepthFirstSearchStack(start, goal, graphIsdirected);
            }
        }
        public static IEnumerable<IEdge> DepthFirstSearchN(IVertex current, ICollection<IEdge> edges, IVertex goal, bool graphIsdirected)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));
            foreach (IEdge e in current.Edges)
            {
                if (!edges.Any(a => EdgeExtensions.Equals(a, e, graphIsdirected)))
                {
                    //mark edges
                    edges.Add(e);
                    DepthFirstSearchN(e.V, edges, goal, graphIsdirected);
                }
                if (edges.Any() && edges.Last().V.Equals(goal)) return edges;
            }
            return edges;
        }
        public static IEnumerable<IEdge> DepthFirstSearchStack(IVertex current, IVertex goal, bool graphIsdirected)
        {
            ICollection<IEdge> edges = null;
            if (graphIsdirected)
            {
                edges = new List<IEdge>();
            }
            else
            {
                edges = new HashSet<IEdge>();
            }
            Stack<IEdge> stack = new Stack<IEdge>();
            stack.Push(current.Edges.FirstOrDefault());
            while (stack.Any())
            {
                foreach (IEdge e in current.Edges)
                {
                    if (!edges.Any(a => EdgeExtensions.Equals(a, e, graphIsdirected)))
                        stack.Push(e);
                }

                IEdge currentEdge = stack.Pop();
                if (!edges.Any(a => EdgeExtensions.Equals(a, currentEdge, graphIsdirected)))
                {
                    edges.Add(currentEdge);
                    current = currentEdge.V;
                }
                if (edges.Any() && edges.Last().V.Equals(goal)) return edges;

            }

            return edges;
        }
        //http://www.informatik-forum.at/showthread.php?80262-Aufgabe-29 check auf bipartit graph

        /// <summary>
        /// Sei G ein Graph mit Knotenmengen V(G). Die Adjazenzmatrix A(G) ist eine qudratische nxn n-Matrix 
        /// mit aij = Fallunterscheidung ist vi und vj mit einer Kante verbunden 1 ansonsten 0
        /// http://en.wikipedia.org/wiki/Adjacency_list
        /// </summary>
        /// <param name="g">Graph on which the adjacency list should be created</param>
        /// <returns></returns>
        public static int[][] AdjacencyMatrix(this DataStructures.Graph g)
        {
            var vertices = DepthFirstTraversal(g).Sort().ToArray();
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

                    IEdge edge = i?.Edges?.FirstOrDefault(a => a.V.Equals(j));
                    if (edge == null)
                    {
                        row[y] = 0;
                    }
                    else
                    {
                        row[y] = edge.Weighted == 0 ? 1 : edge.Weighted;
                    }
                }

                m[o] = row;
            }

            return m;
        }

        public static DataStructures.Graph KruskalDepthFirstSearch(this DataStructures.Graph g)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));
            //works only with undircted graphs
            if (g.Directed.Equals(true))
                throw new ArgumentException("Graph is not undirected");

            //create g'
            DataStructures.Graph g_ = g;

            List<IVertex> vertices = new List<IVertex>();
            //order IEdges by pyramiding weighted, distinct them 
            IEdge[] IEdges = DepthFirstTraversal(g).
                SelectMany(a => a.Edges).
                    OrderBy(e => e.Weighted).
                        Distinct().
                            ToArray();
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

                var o = DepthFirstSearch(u, u, false);

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
    }
}
