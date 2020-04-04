using DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Algorithms.Graph
{
    /// <summary>
    /// Provides a set of static methods for working with graph objects.
    /// </summary>
    public static partial class GraphExtensions
    {
        /// <summary>
        /// Returns all vertices from the graph using <seealso cref="GraphExtensions.DepthFirstSearchStack"/>
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
        /// <param name="start">start vertex</param>
        /// <returns>All collected vertices</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IVertex> DepthFirstSearchStack(this IVertex start)
        {
            HashSet<IVertex> visited = new HashSet<IVertex>();
            Stack<IVertex> stack = new Stack<IVertex>();
            stack.Push(start);
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
        /// Stack based implementation of DepthFirstSearch
        /// Searches from the <paramref name="start"/> the overgiven <paramref name="goal"/>
        /// http://en.wikipedia.org/wiki/Depth-first_search
        /// Completeness: Not complete
        /// Admissible: 
        /// </summary>
        /// <param name="start">start vertex</param>
        /// <param name="goal">Vertex to lookup. When no goal supplied, the algorithm will visit all edges which exists in the graph.</param>
        /// <param name="graphIsUndirected">Determines whether the graph was directed. Default is false (undirected)</param>
        /// <param name="edgeVisitedAction">Action which should be executed when adding a edge to the stack</param>
        /// <returns>Returns a list of all <see cref="IEdge"/>s which are required to get the path beginning from the start to the goal</returns>
        public static IEnumerable<IEdge> DepthFirstSearch(this IVertex start, IVertex goal = null, bool graphIsDirected = true, Action<IEdge> edgeVisitedAction = null)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (graphIsDirected)
            {
                return DepthFirstSearchStack(start, new List<IEdge>(), goal, graphIsDirected, edgeVisitedAction);
            }
            else
            {
                return DepthFirstSearchStack(start, new HashSet<IEdge>(), goal, graphIsDirected, edgeVisitedAction);
            }
        }
        /// <summary>
        /// Stack based implementation of DepthFirstSearch
        /// </summary>
        /// <param name="start">start vertex</param>
        /// <param name="visited">The list which should be used to store the marked edges</param>
        /// <param name="goal">Vertex to lookup. When no goal supplied, the algorithm will visit all edges which exists in the graph.</param>
        /// <param name="graphIsUndirected">Determines whether the graph was directed. Default is false (undirected)</param>
        /// <param name="edgeVisitedAction">Action which should be executed when adding a edge to the stack</param>
        /// <returns>Returns a list of all <see cref="IEdge"/>s which are required to get the path beginning from the start to the goal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<IEdge> DepthFirstSearchStack(IVertex start, ICollection<IEdge> visited, IVertex goal, bool graphIsDirected, Action<IEdge> edgeVisitedAction = null)
        {
            if (start == null) return Enumerable.Empty<IEdge>();
            if (visited == null) return Enumerable.Empty<IEdge>();
            Stack<IEdge> stack = new Stack<IEdge>();
            foreach (var edge in start.Edges)
            {
                stack.Push(edge);
            }
            while (stack.Any())
            {
                IEdge e = stack.Pop();
                edgeVisitedAction?.Invoke(e);
                //use for directed graphs == operator and 
                //for undirected equals method by using "Contains" of the collection
                if (graphIsDirected ? !visited.Any(a => a == e) : !visited.Contains(e))
                {
                    visited.Add(e);
                    if (e.V == goal) return visited;
                    foreach (IEdge edge in e.V.Edges)
                    {
                        stack.Push(edge);
                    }
                }
            }
            return visited;
        }
        /// <summary>
        /// Recurisv implementation of DepthFirstSearch
        /// </summary>
        /// <remarks>HashSet uses the equals method for comparison of edges</remarks>
        /// <param name="current">Start vertex</param>
        /// <param name="goal">Vertex to lookup. When no goal supplied, the algorithm will visit all edges which exists in the graph.</param>
        /// <param name="graphIsDirected">True if graph is directed. False for graph is undirected</param>
        /// <returns>Returns a list of all <see cref="IEdge"/>s which are required to get the path beginning from the start to the goal</returns>
        public static IEnumerable<IEdge> DepthFirstSearchRecurisv(this IVertex current, IVertex goal, bool graphIsDirected)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));
            if (graphIsDirected)
            {
                return DepthFirstSearchRecurisv(current, new List<IEdge>(), goal, graphIsDirected);
            }
            else
            {
                return DepthFirstSearchRecurisv(current, new HashSet<IEdge>(), goal, graphIsDirected);
            }
        }
        /// <summary>
        /// Recurisv implementation of DepthFirstSearch
        /// </summary>
        /// <param name="current">start vertex</param>
        /// <param name="visited">The list of already visited vertices</param>
        /// <param name="goal">Vertex to lookup. When no goal supplied, the algorithm will visit all edges which exists in the graph.</param>
        /// <param name="graphIsDirected">True if graph is directed. False for graph is undirected</param>
        /// <returns>All collected edges</returns>
        private static IEnumerable<IEdge> DepthFirstSearchRecurisv(IVertex current, ICollection<IEdge> visited, IVertex goal, bool graphIsDirected)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));
            if (visited == null) throw new ArgumentNullException(nameof(visited));
            foreach (IEdge e in current.Edges)
            {
                //make sure the correct comparison method for edges is used
                if (graphIsDirected ? !visited.Any(a => a == e) : !visited.Contains(e))
                {
                    //mark edges
                    visited.Add(e);
                    DepthFirstSearchRecurisv(e.V, visited, goal, graphIsDirected);
                }
                if (visited.Any() && visited.Last().V.Equals(goal)) return visited;
            }
            return visited;
        }
        /// <summary>
        /// Queue based implementation of BreadthFirstSearch
        /// </summary>
        /// <param name="vertex">start vertex</param>
        /// <param name="vertexVisitedAction">The action to execute when a vertex is dequeued</param>
        /// <returns>The list of visited vertices</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IVertex> BreadthFirstSearchQueue(this IVertex vertex, Action<IVertex> vertexVisitedAction = null)
        {
            HashSet<IVertex> visited = new HashSet<IVertex>() { vertex };
            // Create a queue for BFS 
            Queue<IVertex> queue = new Queue<IVertex>();
            // Mark the current node as visited and enqueue it 
            queue.Enqueue(vertex);
            while (queue.Count != 0)
            {
                // Dequeue a vertex
                vertex = queue.Dequeue();
                vertexVisitedAction?.Invoke(vertex);
                // Get all adjacent vertices of the dequeued vertex
                // If a adjacent has not been visited, then mark it 
                // visited and enqueue it 
                var i = vertex.Edges.GetEnumerator();
                while (i.MoveNext())
                {
                    IEdge n = i.Current;
                    if (!visited.Contains(n.V))
                    {
                        visited.Add(n.V);
                        queue.Enqueue(n.V);
                    }
                }
            }
            return visited;
        }
        /// <summary>
        /// Kruskal implementation with DFS
        /// </summary>
        /// <param name="g"></param>
        /// <returns>The new created graph without cycles</returns>
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
        /// <returns>Calculated distance</returns>
        public static int Distance(this IEnumerable<IEdge> edges)
        {
            int distance = 0;
            if (edges == null) return distance;

            foreach (IEdge e in edges)
                distance = +e.Weighted + distance;

            return distance;
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
        /// Returns a value indicating whether this instance is equal to the edge.
        /// </summary>
        /// <param name="edge">The edge to compare to this instance.</param>
        /// <param name="graphIsdirected">If the parameter is false transported edges will be handled as equal</param>
        /// <returns>True if the instance and the overgiven edge are euqa; otherwiese, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(this IEdge e, IEdge edge, bool graphIsdirected = true)
        {
            Contract.Requires(e != null);
            //use == operator instead of Equals for directed graphs,
            //as the overriden equals of the edge implementeation returns true for transposed edges)
            return graphIsdirected ? e == edge : e.Equals(edge);
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
    }
}
