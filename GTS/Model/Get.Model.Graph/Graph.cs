using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

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
    }
}
