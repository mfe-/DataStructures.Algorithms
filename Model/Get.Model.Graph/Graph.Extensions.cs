using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Collections;

namespace Get.Model.Graph
{
    /// <summary>
    /// Provides a set of static methods for working with graph objects.
    /// </summary>
    public static class GraphExtensions
    {
        public static void Save(this Graph g, String pfilename)
        {
            throw new NotImplementedException("NetDataContractSerializer was replaced by DataContractSerializer " +
                "because this type is not available in .NET Standard create proper de/serializer for DataContractSerializer");

            //http://msdn.microsoft.com/de-de/magazine/cc163569%28en-us%29.aspx
            using (FileStream fs = new FileStream(pfilename, FileMode.Create))
            using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs))
            {
                DataContractSerializer serializer = new DataContractSerializer(g.GetType());  // no type specified
                serializer.WriteObject(writer, g);

            }
        }
        /// <summary>
        /// Serialize the Graph into a XElement
        /// </summary>
        /// <param name="g">Graph to save</param>
        /// <returns>Serialized Graph</returns>
        public static XElement Save(this Graph g)
        {
            throw new NotImplementedException("NetDataContractSerializer was replaced by DataContractSerializer " +
                "because this type is not available in .NET Standard create proper de/serializer for DataContractSerializer");
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(ms))
                {
                    var dataContractSerializerSettings = new DataContractSerializerSettings();
                    dataContractSerializerSettings.PreserveObjectReferences = true;
                    DataContractSerializer serializer = new DataContractSerializer(g.GetType(),dataContractSerializerSettings);
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
                    DataContractSerializer serializer = new DataContractSerializer(g.GetType());
                    Graph a = (Graph)serializer.ReadObject(reader, true);
                    foreach (var v in a.Vertices)
                    {
                        g.Vertices.Add(v);
                    }
                }
            }
        }
        public static void Load(this Graph g, XElement e)
        {
            //load graph
            MemoryStream memoryStream = new MemoryStream();
            e.Save(memoryStream);
            memoryStream.Position = 0;
            DataContractSerializer ndcs = new DataContractSerializer(g.GetType());
            Graph u = ndcs.ReadObject(memoryStream) as Graph;

            g.Start = u.Start;

            g.Directed = u.Directed;

            foreach (Vertex v in u.Vertices)
                g.Vertices.Add(v);

        }

        public static IEnumerable<Vertex> Sort(this IEnumerable<Vertex> l)
        {
            return l.OrderBy(a => a.Weighted).ThenBy(a => a.Size).ThenBy(a => a.GetHashCode());
        }
        public static IEnumerable<Vertex> ToVertexList(this IEnumerable<Edge> l)
        {
            return l.SelectMany(a => new List<Vertex>() { a.U, a.V }).Distinct();
        }
        /// <summary>
        /// Returns all vertices from the graph
        /// http://www.brpreiss.com/books/opus4/html/page551.html
        /// http://www.cse.ohio-state.edu/~gurari/course/cis680/cis680Ch14.html#QQ1-46-90
        /// </summary>
        /// <param name="s">Root vertex of graph</param>
        /// <returns>All reachable vertices</returns>
        public static IEnumerable<Vertex> Depth_First_Traversal(this Graph s)
        {
            List<Vertex> l = new List<Vertex>();
            foreach (Vertex v in s.Vertices)
                l.AddRange(Depth_First_Traversal(v, new List<Vertex>()));
            return l;
        }
        private static List<Vertex> Depth_First_Traversal(this Vertex s, List<Vertex> visited)
        {
            //visist x
            visited.Add(s);

            //FOR each y such that (x,y) is an edge DO 
            foreach (Edge e in s.Edges)
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
        public static List<Edge> Breadth_First_Search(this Vertex start, Vertex target)
        {
            List<Edge> l = new List<Edge>();
            //TODO
            //Stack<Vertex> stack = new Stack<Vertex>();
            
            //Vertex current = start;
            
            //for (int i = 1; i <= current.Edges.Count; i++)
            //{
            //    Vertex v = current.Edges[i-1].V;
            //    if (v.Equals(target))
            //    {
            //        return l;
            //    }
            //    else if (i.Equals(current.Edges.Count) && !v.Equals(target))
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
            var edges = vertices.SelectMany(a => a.Edges).Distinct<Edge>();
            //create matrix
            int c = vertices.Count<Vertex>();
            int[][] m = new int[c][];
            for (int o = 0; o < c; o++)
            {
                int[] row = new int[c];
                for (int y = 0; y < c; y++)
                {
                    Vertex i = vertices[o];
                    Vertex j = vertices[y];

                    row[y] = edges.Where(b => b.V.Equals(i) && b.U.Equals(j)).Count() == 0 ? 0 : 1;
                }

                m[o] = row;
            }

            return m;
        }

        public static Graph Kruskal_DepthFirstSearch(this Graph g)
        {
            //work only with undircted graphs
            if (g.Directed.Equals(true))
                throw new DirectedException(false);


            //create g'
            Graph g_ = g;

            List<Vertex> vertices = new List<Vertex>();
            //order edges by pyramiding weighted
            Edge[] edges = Depth_First_Traversal(g).SelectMany(a => a.Edges).OrderBy(e => e.Weighted).Distinct(new EdgeExtensions.EdgeComparer()).ToArray();
            //remove edges 
            foreach (Vertex z in g_.Depth_First_Traversal())
            {
                vertices.Add(z);

                z.Edges.Clear();
            }

            int weight = 0;
            for (int i = 0; i < edges.Length; i++)
            {
                Edge e = edges[i];
                weight = weight + e.Weighted;
                Vertex u = vertices.Where(a => a.Equals(e.U)).First<Vertex>();
                Vertex v = vertices.Where(a => a.Equals(e.V)).First<Vertex>();
                //add 2x edges to create a undirected graph
                u.addEdge(v, e.Weighted);
                v.addEdge(u, e.Weighted);
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

        public static Edge Dijkstra(this Graph g, Vertex start)
        {
            //tabel
            int min = start.Edges.First().Weighted;
            foreach (Edge e in start.Edges)
            {
                if (min > e.Weighted)
                {
                    min = e.Weighted;
                }
            }
            return null;
        }
        /// <summary>
        /// Searches beginning from the start vertex to the goal vertex a path
        /// http://en.wikipedia.org/wiki/Depth-first_search
        /// </summary>
        /// <param name="g">Graph which is vertex containing</param>
        /// <param name="start">Where the look up should beginn</param>
        /// <param name="goal">Which vertex should be found</param>
        /// <returns>Returns a list of containing all edges which are required to get the path beginning from the start to the goal vertex</returns>
        public static IEnumerable<Edge> DepthFirstSearch(this Vertex start, Vertex goal)
        {
            return DepthFirstSearch(start, new List<Edge>(), goal);
        }
        private static IEnumerable<Edge> DepthFirstSearch(Vertex current, List<Edge> edges, Vertex goal)
        {
            //mark edge
            foreach (Edge e in current.Edges)
            {

                //check if we found the goal
                if (e.V.Equals(goal))
                {
                    //some special behaviour duo circlues which we have to consider resulting of the our model (undirected: 1->3, 3->1)
                    if (!edges.First().U.Edges.SelectMany(a => a.V.Edges).ToList().Exists(y => y.Equals(e))//schauen ob er zurückgehen will
                        || (edges.First().U.Edges.SelectMany(a => a.V.Edges).ToList().Exists(y => y.Equals(e) && //(kreis existiert mit edge ausgehend vom start), (schauen ob dazwischen noch andere edges sind)
                        edges.Find(delegate(Edge ed) { return ed.V == e.U && ed.U != e.V; }) != null)))
                    {
                        edges.Add(e);
                        return edges;
                    }
                }
                //do not add already visited vertex
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
        /// Determinds if the overgivven vertex is adjacent to the current vertex
        /// </summary>
        /// <param name="v">the vertex to check</param>
        /// <returns>True if the overgiven vertex is adjacent</returns>
        public static Boolean Adjacent(this Vertex v)
        {
            return v.Edges.Where(a => a.U.Equals(v) || a.V.Equals(v)).Count().Equals(0);
        }
        /// <summary>
        /// Calculates the distance by summing the weighted of the edges.
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static int Distance(this IEnumerable<Edge> edges)
        {
            int distance = 0;
            foreach (Edge e in edges)
                distance = +e.Weighted + distance;
            return distance;
        }


        public static object Connected(this Graph g, Vertex a, Vertex b)
        {
            // Eine Folge von karten e1,e2,...ek e E(G) eines ungerichteten G heißt katenfolge , wenn es knoten v,v1,v2,...vk1 w eV(G) mit 
            //% gibt ,d.h. man kann die katen e1,2,...ek,ohne absetzen durchlafeun. k... anzahl der kanten
            //Tiefensuchen hier nehmen Katenfolge von a -> b zurück geben
            return null;
        }
        //public static bool Connected(this Graph g, Vertex a, Vertex b)
        //{
        //    // Eine Folge von karten e1,e2,...ek e E(G) eines ungerichteten G heißt katenfolge , wenn es knoten v,v1,v2,...vk1 w eV(G) mit 
        //    //% gibt ,d.h. man kann die katen e1,2,...ek,ohne absetzen durchlafeun. k... anzahl der kanten
        //    //Tiefensuchen hier nehmen Katenfolge von a -> b zurück geben
        //    //return g.Connected(a,b) ==null ? false : true;
        //    return false;
        //}
    }
}
