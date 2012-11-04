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
            //http://msdn.microsoft.com/de-de/magazine/cc163569%28en-us%29.aspx
            using (FileStream fs = new FileStream(pfilename, FileMode.Create))
            using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs))
            {
                NetDataContractSerializer serializer = new NetDataContractSerializer();  // no type specified
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
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(ms))
                {
                    NetDataContractSerializer serializer = new NetDataContractSerializer();
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
                    NetDataContractSerializer serializer = new NetDataContractSerializer();
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
            NetDataContractSerializer ndcs = new NetDataContractSerializer();
            Graph u = ndcs.ReadObject(memoryStream) as Graph;

            g.StartVertex = u.StartVertex;
            g.EndVertex = u.EndVertex;
            g.Directed = u.Directed;

            foreach (Vertex v in u.Vertices)
                g.Vertices.Add(v);

        }
        public static IEnumerable<Vertex> Sort(this IEnumerable<Vertex> l)
        {
            return l.OrderBy(a => a.Weighted).ThenBy(a => a.Size).ThenBy(a => a.GetHashCode());
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

        public static Graph Kruskal(this Graph g, Vertex start)
        {
            //create g'
            Graph g_ = new Graph();
            //copy object instance
            XElement xg_ = g.Save();
            g_.Load(xg_);
            //remove edges 
            foreach (Vertex z in g_.Depth_First_Traversal())
            {
                if (!g_.Depth_First_Traversal().ToArray().Contains(z))
                {
                    g_.addVertex(z);
                }
                z.Edges.Clear();
            }

            var vertices = Depth_First_Traversal(g);
            //order edges by pyramiding weighted
            Edge[] edges = vertices.SelectMany(a => a.Edges).Distinct().OrderBy(e => e.Weighted).ToArray();

            for (int i = 0; i < edges.Length; i++)
            {
                Edge e = edges[i];
                Vertex u = g_.Vertices.Where(a => a.Equals(e.U)).First<Vertex>();
                Vertex v = g_.Vertices.Where(a => a.Equals(e.V)).First<Vertex>();
                //add 2x edges to create a undirected graph
                u.addEdge(v, e.Weighted);
                v.addEdge(u, e.Weighted);
                //check if circle
                var o = DepthFirstSearch(u, u, true);
                //er macht irgendwie die liste falsch mit e.count = 7 und zwar 5->7 , 5->6 , 6->3, 3->6 , 6->5 ... schaut nach ob last 5 hat und -> zirkel.... also falsch
                if (o.Last().V.Equals(u))
                {
                    u.Edges.Remove(u.Edges.Last());
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
            return DepthFirstSearch(start, new List<Edge>(), goal, false);
        }
        public static IEnumerable<Edge> DepthFirstSearch(this Vertex start, Vertex goal, bool directed)
        {
            IList<Edge> l = DepthFirstSearch(start, new List<Edge>(), goal, directed).ToList<Edge>();
            //when graph is directed and there exists edges with cycle remove it a->b & b->a ... we need only one edge of them
            if (directed == true && l.Count >= 2 && (l.Last().V.Equals(l[l.Count - 2].U) && l.Last().U.Equals(l[l.Count - 2].V)))
            {
                l.Remove(l.Last());
            }

            return l;
        }
        private static IEnumerable<Edge> DepthFirstSearch(Vertex p, List<Edge> edges, Vertex goal, bool directed)
        {
            foreach (Edge e in p.Edges)
            {
                //check if the recursiv call terminated because it found the goal
                if (goal != null && (edges.Count > 0 && edges.Last<Edge>().V.Equals(goal)))
                    return edges;
                //only add unmarked edges
                if (edges.Where(a => a.U.Equals(p) && a.V.Equals(e.V)).Count() == 0)
                {
                    //mark edge
                    edges.Add(new Edge(p, e.V, e.Weighted));
                    //when directed dont go back!
                    if (directed == true && (edges.Last().U.Equals(edges.First().V) && edges.Last().V.Equals(edges.First().U)) && edges.Count.Equals(2))
                        edges.Remove(edges.Last());

                    if (goal == null || (goal != null && !e.V.Equals(goal)))
                        DepthFirstSearch(e.V, edges, goal, directed);
                    else if (goal != null && e.V.Equals(goal))
                        return edges;
                }
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


        //public static IEnumerable<Edge> DepthFirstSearch(this Graph g)
        //{
        //    //possible ways
        //    List<Edge> edges = new List<Edge>();
        //    Vertex current = g.Vertices.First();

        //    foreach (Vertex v in g.Vertices)
        //    {
        //        DepthFirstSearch(v, edges,null);
        //    }

        //    return edges;
        //}
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
