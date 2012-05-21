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
        public static void Load(this Graph g, String pfilename)
        {
            using (FileStream fs = new FileStream(pfilename, FileMode.Open))
            {
                XmlDictionaryReaderQuotas xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas() { MaxDepth = 100 };
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

        /// <summary>
        /// Sei G ein Graph mit Knotenmengen V(G). Die Adjazenzmatrix A(G) ist eine qudratische nxn n-Matrix 
        /// mit aij = FAllunterscheidung ist vi und vj mit einer Kante verbunden 1 ansonsten 0
        /// http://en.wikipedia.org/wiki/Adjacency_list
        /// </summary>
        /// <param name="g">Graph on which the adjacency list should be created</param>
        /// <returns></returns>
        public static object AdjacencyList(this Graph g)
        {
            var vertices = Depth_First_Traversal(g).OrderBy(a => a.Weighted).ThenBy(a=>a.VertexSize).ThenBy(a=>a.GetHashCode()).ToArray();
            var edges = vertices.SelectMany(a => a.Edges).Distinct<Edge>();
            //create matrix
            int c = vertices.Count<Vertex>();
            int[][] m = new int[c][];
            for (int o = 0; o < c; o++)
            {
                int[] row = new int [c];
                for (int y = 0; y < c; y++)
                {
                    Vertex i = vertices[o];
                    Vertex j = vertices[y];

                    if (g.Directed)
                    {
                        row[y] = edges.Where(b => b.V.Equals(i) && b.U.Equals(j)).Count() == 0 ? 0 : 1;
                    }
                    else
                    {
                        //direction doesnt matter
                        row[y] = edges.Where(b => b.V.Equals(i) && b.U.Equals(j) || b.U.Equals(i) && b.V.Equals(j)).Count() == 0 ? 0 : 1;
                    }
                }

                m[o] = row;
            }

            return m;
        }

        public static Graph Kruskal(this Graph g, Vertex start)
        {
            return null;
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
        private static IEnumerable<Edge> DepthFirstSearch(Vertex p, List<Edge> edges, Vertex goal)
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
                    edges.Add(new Edge(p, e.V,e.Weighted));
                    if (goal == null || (goal != null && !e.V.Equals(goal)))
                        DepthFirstSearch(e.V, edges,goal);
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
            int distance=0;
            foreach (Edge e in edges)
                distance =+ e.Weighted+distance;
            return distance;
        }
    }
}
