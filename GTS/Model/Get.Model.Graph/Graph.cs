using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Get.Model.Graph
{
    [DataContract(Namespace = "http://schemas.get.com/Graph/")]
    public class Graph
    {
        [DataMember(Name = "Vertices")]
        public ObservableCollection<Vertex> _Vertices = new ObservableCollection<Vertex>();

        public Graph()
        {
        }
        public void addVertec(Vertex pVertice)
        {
            _Vertices.Add(pVertice);
        }

        public ObservableCollection<Vertex> Vertices
        {
            get
            {
                return _Vertices;
            }
        }


    }
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
        public static IEnumerable<Vertex> Depth_First_Traversal(this Vertex s)
        {
            return Depth_First_Traversal(s, new List<Vertex>());
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
        //private static DFS1(Vertex v)
        //{

        //}
    }
}
