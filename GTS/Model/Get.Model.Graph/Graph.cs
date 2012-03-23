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
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
            {
                NetDataContractSerializer serializer = new NetDataContractSerializer();
                Graph a = (Graph)serializer.ReadObject(reader, true);
                foreach (var v in a.Vertices)
                {
                    g.Vertices.Add(v);
                }
            }
        }
        /// <summary>
        /// Return all vertices which are reachable from s
        /// http://en.wikipedia.org/wiki/Depth_first_search
        /// </summary>
        /// <param name="s">Start vertex</param>
        /// <returns>All reachable vertices</returns>
        public static List<Vertex> Depth_first_Search(this Vertex s,List<Vertex> rv)
        {
            if(!rv.Contains(s))
                rv.Add(s); //visit   http://www.cse.ohio-state.edu/~gurari/course/cis680/cis680Ch14.html#QQ1-46-96
            foreach(Edge e in s.Edges)
            {
                if (!rv.Contains(e.V))
                {
                    rv.Add(e.V);
                    Depth_first_Search(e.V, rv);
                }
            }
            return rv;
        }
        //private static DFS1(Vertex v)
        //{

        //}
    }
}
