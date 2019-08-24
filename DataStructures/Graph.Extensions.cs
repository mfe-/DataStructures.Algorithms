using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace DataStructures
{
    /// <summary>
    /// Provides a set of static methods for working with graph objects.
    /// </summary>
    public static class GraphExtensions
    {
        /// <summary>
        /// Serialize the Graph into a XElement
        /// </summary>
        /// <param name="g">Graph to save</param>
        /// <returns>Serialized Graph</returns>
        public static XElement Save(this Graph g)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(ms))
                {
                    var dataContractSerializerSettings = new DataContractSerializerSettings();
                    dataContractSerializerSettings.PreserveObjectReferences = true;
                    DataContractSerializer serializer = new DataContractSerializer(g.GetType(), dataContractSerializerSettings);
                    serializer.WriteObject(writer, g);
                    writer.Flush();
                    ms.Position = 0;
                    return XElement.Load(ms);
                }
            }
        }
        /// <summary>
        /// Deserialize the Graph from a xml file
        /// </summary>
        /// <param name="g">where the graph should be into deserialize </param>
        /// <param name="pfilename">filename to the xml file which contains the data of the graph</param>
        public static void Load(this Graph g, String pfilename)
        {
            Load(g, pfilename, 100);
        }
        public static void Load(this Graph g, String pfilename, int maxDepth)
        {
            using (FileStream fs = new FileStream(pfilename, FileMode.Open))
            {
                XmlDictionaryReaderQuotas xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas() { MaxDepth = maxDepth };
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, xmlDictionaryReaderQuotas))
                {
                    var dataContractSerializerSettings = new DataContractSerializerSettings();
                    dataContractSerializerSettings.PreserveObjectReferences = true;
                    DataContractSerializer serializer = new DataContractSerializer(g.GetType(), dataContractSerializerSettings);
                    Graph a = (Graph)serializer.ReadObject(reader, true);
                    foreach (var v in a.Vertices)
                    {
                        g.Vertices.Add(v);
                    }
                }
            }
        }
        public static Graph Load(this XElement e)
        {
            Graph g = new Graph();
            //load graph
            MemoryStream memoryStream = new MemoryStream();
            e.Save(memoryStream);
            memoryStream.Position = 0;
            var dataContractSerializerSettings = new DataContractSerializerSettings();
            dataContractSerializerSettings.KnownTypes = new List<Type>() { typeof(Vertex), typeof(Edge) };
            dataContractSerializerSettings.PreserveObjectReferences = true;
            DataContractSerializer ndcs = new DataContractSerializer(g.GetType(), dataContractSerializerSettings);
            Graph u = ndcs.ReadObject(memoryStream) as Graph;

            g.Start = u.Start;

            g.Directed = u.Directed;

            foreach (IVertex v in u.Vertices)
                g.Vertices.Add(v);
            return g;
        }
        public static bool IsDirected(this Graph g)
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
        public static IEnumerable<IVertex> ToVertexList(this IEnumerable<IEdge> l)
        {
            return l.SelectMany(a => new List<IVertex>() { a.U, a.V }).Distinct();
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
            return (edge.V.Edges.FirstOrDefault(a => a.V.Equals(edge.U)));
        }
        /// <summary>
        /// Returns all vertices from the graph
        /// http://www.brpreiss.com/books/opus4/html/page551.html
        /// http://www.cse.ohio-state.edu/~gurari/course/cis680/cis680Ch14.html#QQ1-46-90
        /// </summary>
        /// <param name="s">Root IVertex of graph</param>
        /// <returns>All reachable vertices</returns>
        public static IEnumerable<IVertex> Depth_First_Traversal(this Graph s)
        {
            List<IVertex> l = new List<IVertex>();
            foreach (IVertex v in s.Vertices)
                l.AddRange(Depth_First_Traversal(v, new List<IVertex>()));
            return l;
        }
        private static List<IVertex> Depth_First_Traversal(this IVertex s, List<IVertex> visited)
        {
            //visist x
            visited.Add(s);

            //FOR each y such that (x,y) is an IEdge DO 
            foreach (IEdge e in s.Edges)
            {
                if (!visited.Contains(e.V))
                {
                    visited = Depth_First_Traversal(e.V, visited);
                }
            }
            return visited;
        }
        public static IEnumerable<IVertex> Deph_First_Search(this IVertex s)
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
        public static List<IEdge> Breadth_First_Search(this IVertex start, IVertex target)
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
        public static int[][] AdjacencyList(this Graph g)
        {
            var vertices = Depth_First_Traversal(g).Sort().Distinct().ToArray();
            var IEdges = vertices.SelectMany(a => a.Edges).Distinct<IEdge>();
            //create matrix
            int c = vertices.Count<IVertex>();
            int[][] m = new int[c][];
            for (int o = 0; o < c; o++)
            {
                int[] row = new int[c];
                for (int y = 0; y < c; y++)
                {
                    IVertex i = vertices[o];
                    IVertex j = vertices[y];

                    row[y] = IEdges.Where(b => b.V.Equals(i) && b.U.Equals(j)).Count() == 0 ? 0 : 1;
                }

                m[o] = row;
            }

            return m;
        }

        public static Graph Kruskal_DepthFirstSearch(this Graph g)
        {
            //works only with undircted graphs
            if (g.Directed.Equals(true))
                throw new DirectedException(false);

            //create g'
            Graph g_ = g;

            List<IVertex> vertices = new List<IVertex>();
            //order IEdges by pyramiding weighted
            IEdge[] IEdges = Depth_First_Traversal(g).SelectMany(a => a.Edges).OrderBy(e => e.Weighted).Distinct(new EdgeExtensions.EdgeComparer()).ToArray();
            //remove IEdges 
            foreach (IVertex z in g_.Depth_First_Traversal())
            {
                vertices.Add(z);

                z.Edges.Clear();
            }

            int weight = 0;
            for (int i = 0; i < IEdges.Length; i++)
            {
                IEdge e = IEdges[i];
                weight = weight + e.Weighted;
                IVertex u = vertices.Where(a => a.Equals(e.U)).First<IVertex>();
                IVertex v = vertices.Where(a => a.Equals(e.V)).First<IVertex>();
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

        public static IEdge Dijkstra(this Graph g, IVertex start)
        {
            //tabel
            int min = start.Edges.First().Weighted;
            foreach (IEdge e in start.Edges)
            {
                if (min > e.Weighted)
                {
                    min = e.Weighted;
                }
            }
            return null;
        }
        /// <summary>
        /// Searches beginning from the start IVertex to the goal IVertex a path
        /// http://en.wikipedia.org/wiki/Depth-first_search
        /// </summary>
        /// <param name="g">Graph which is IVertex containing</param>
        /// <param name="start">Where the look up should beginn</param>
        /// <param name="goal">Which IVertex should be found</param>
        /// <returns>Returns a list of containing all IEdges which are required to get the path beginning from the start to the goal IVertex</returns>
        public static IEnumerable<IEdge> DepthFirstSearch(this IVertex start, IVertex goal)
        {
            return DepthFirstSearch(start, new List<IEdge>(), goal);
        }
        private static IEnumerable<IEdge> DepthFirstSearch(IVertex current, List<IEdge> edges, IVertex goal)
        {
            //mark edges
            foreach (IEdge e in current.Edges)
            {
                //check if we found the goal
                if (e.V.Equals(goal))
                {
                    //some special behaviour duo circlues which we have to consider resulting of the our model (undirected: 1->3, 3->1)
                    if (!edges.First().U.Edges.SelectMany(a => a.V.Edges).ToList().Exists(y => y.Equals(e))//schauen ob er zurückgehen will
                        || (edges.First().U.Edges.SelectMany(a => a.V.Edges).ToList().Exists(y => y.Equals(e) && //(kreis existiert mit IEdge ausgehend vom start), (schauen ob dazwischen noch andere IEdges sind)
                        edges.Find(delegate (IEdge ed) { return ed.V == e.U && ed.U != e.V; }) != null)))
                    {
                        edges.Add(e);
                        return edges;
                    }
                }
                //do not add already visited IVertex
                if (!edges.ToVertexList().Contains(e.V))
                {
                    if ((!edges.Count.Equals(0) && (e.V != edges.First().U)) || edges.Count.Equals(0))
                    {
                        edges.Add(e);
                        DepthFirstSearch(e.V, edges, goal);
                    }
                }
                if (edges.Last().V.Equals(goal)) return edges;
            }


            return edges;
        }

        /// <summary>
        /// Determinds if the overgivven IVertex is adjacent to the current IVertex
        /// </summary>
        /// <param name="v">the IVertex to check</param>
        /// <returns>True if the overgiven IVertex is adjacent</returns>
        public static Boolean Adjacent(this IVertex v)
        {
            return v.Edges.Where(a => a.U.Equals(v) || a.V.Equals(v)).Count().Equals(0);
        }
        /// <summary>
        /// Calculates the distance by summing the weighted of the IEdges.
        /// </summary>
        /// <param name="IEdges"></param>
        /// <returns></returns>
        public static int Distance(this IEnumerable<IEdge> IEdges)
        {
            int distance = 0;
            foreach (IEdge e in IEdges)
                distance = +e.Weighted + distance;
            return distance;
        }


        public static object Connected(this Graph g, IVertex a, IVertex b)
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
