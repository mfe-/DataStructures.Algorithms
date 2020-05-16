using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace DataStructures.Algorithms.Graph
{
    /// <summary>
    /// Provides a set of static methods for working with graph objects.
    /// </summary>
    public static partial class GraphExtensions
    {
        public static DataContractSerializerSettings GetDataContractSerializerSettings()
        {
            return GetDataContractSerializerSettings(new List<Type>());
        }
        public static DataContractSerializerSettings GetDataContractSerializerSettings(List<Type> knownTypes, DataContractResolver? dataContractResolver = null)
        {
            List<Type> types = new List<Type>() { typeof(Edge), typeof(Vertex), typeof(Vertex<object>), typeof(Edge<object>) };
            if (knownTypes != null)
            {
                types.AddRange(knownTypes);
            }
            var dataContractSerializerSettings = new DataContractSerializerSettings();
            dataContractSerializerSettings.PreserveObjectReferences = true;
            dataContractSerializerSettings.KnownTypes = types;
            dataContractSerializerSettings.DataContractResolver = dataContractResolver;
            return dataContractSerializerSettings;
        }
        /// <summary>
        /// Serialize the Graph into a XElement
        /// </summary>
        /// <param name="g">Graph to save</param>
        /// <returns>Serialized Graph</returns>
        public static XElement Save(this DataStructures.Graph g, Action<DataContractSerializerSettings>? DataContractSerializerSettingsActionInvokrer = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(ms))
                {
                    DataContractSerializerSettings dataContractSerializerSettings = GetDataContractSerializerSettings();
                    DataContractSerializerSettingsActionInvokrer?.Invoke(dataContractSerializerSettings);
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
        public static void Load(this DataStructures.Graph g, String pfilename)
        {
            Load(g, pfilename, 100);
        }
        public static void Load(this DataStructures.Graph g, String pfilename, int maxDepth, Action<DataContractSerializerSettings>? DataContractSerializerSettingsActionInvokrer = null)
        {
            using (FileStream fs = new FileStream(pfilename, FileMode.Open))
            {
                XmlDictionaryReaderQuotas xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas() { MaxDepth = maxDepth };
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, xmlDictionaryReaderQuotas))
                {
                    DataContractSerializerSettings dataContractSerializerSettings = GetDataContractSerializerSettings();
                    DataContractSerializerSettingsActionInvokrer?.Invoke(dataContractSerializerSettings);
                    DataContractSerializer serializer = new DataContractSerializer(g.GetType(), dataContractSerializerSettings);
                    DataStructures.Graph a = (DataStructures.Graph)serializer.ReadObject(reader, true);
                    foreach (var v in a.Vertices)
                    {
                        g.Vertices.Add(v);
                    }
                }
            }
        }
        public static DataStructures.Graph Load(this XElement e, Action<DataContractSerializerSettings>? DataContractSerializerSettingsActionInvokrer = null)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            DataStructures.Graph g = new DataStructures.Graph();
            //load graph
            using (MemoryStream memoryStream = new MemoryStream())
            {
                e.Save(memoryStream);
                memoryStream.Position = 0;
                DataContractSerializerSettings dataContractSerializerSettings = GetDataContractSerializerSettings();
                DataContractSerializerSettingsActionInvokrer?.Invoke(dataContractSerializerSettings);

                DataContractSerializer ndcs = new DataContractSerializer(g.GetType(), dataContractSerializerSettings);
                object deserialized = ndcs.ReadObject(memoryStream);
                if (deserialized is DataStructures.Graph gp)
                {
                    DataStructures.Graph u = gp;

                    g.Directed = u.Directed;
                    foreach (IVertex v in u.Vertices)
                    {
                        g.Vertices.Add(v);
                    }
                    g.Start = u.Start;
                    return g;
                }
                else
                {
                    throw new NotSupportedException($"When reading from the Stream the type {nameof(DataStructures.Graph)} was expected but got {deserialized.GetType()}");
                }
            }
        }
        public static string Serialize(this DataStructures.IVertex v, string path)
        {
            // Create an XmlWriterSettings object with the correct options. 
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            IList<IVertex> vertices = new List<IVertex>();
            string xml;
            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, settings))
                {

                    XmlDocument xmlGraphDocument = new XmlDocument();
                    //(1) the xml declaration
                    XmlDeclaration xmlDeclaration = xmlGraphDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = xmlGraphDocument.DocumentElement;
                    xmlGraphDocument.InsertBefore(xmlDeclaration, root);

                    //(2) graph
                    XmlElement graphXmlElement = xmlGraphDocument.CreateElement(string.Empty, nameof(DataStructures.Graph), string.Empty);
                    xmlGraphDocument.AppendChild(graphXmlElement);


                    v.BreadthFirstSearchQueue(WriteVertexXml);

                    xmlGraphDocument.WriteTo(writer);

                    Guid GetGuid(IVertex vertex)
                    {
                        Type type = vertex.GetType();
                        FieldInfo[] fields = type.GetFields(
                             BindingFlags.NonPublic |
                             BindingFlags.Instance);

                        MemberInfo memberInfo = fields.FirstOrDefault(a => a.FieldType == typeof(Guid));
                        if (memberInfo == null)
                        {
                            throw new NotSupportedException("We are expe");
                        }
                        switch (memberInfo.MemberType)
                        {
                            case MemberTypes.Field:
                                return (Guid)((FieldInfo)memberInfo).GetValue(vertex);
                            case MemberTypes.Property:
                                return (Guid)((PropertyInfo)memberInfo).GetValue(vertex);
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    void WriteVertexXml(IVertex vertex)
                    {
                        XmlElement xmlElementVertex = CreateVertexXml(vertex, graphXmlElement);
                        if (vertex.Edges.Any())
                        {
                            XmlElement element2 = xmlGraphDocument.CreateElement(string.Empty, nameof(DataStructures.IVertex.Edges), string.Empty);
                            xmlElementVertex.AppendChild(element2);
                            foreach (var edge in vertex.Edges)
                            {
                                CreateEdgeXml(edge, element2);
                            }
                        }

                    }
                    XmlElement CreateVertexXml(IVertex vertex, XmlElement xmlElementGraph)
                    {
                        XmlElement xmlElementVertex = xmlGraphDocument.CreateElement(string.Empty, nameof(DataStructures.IVertex), string.Empty);
                        xmlElementGraph.AppendChild(xmlElementVertex);

                        XmlElement element3 = xmlGraphDocument.CreateElement(string.Empty, nameof(Guid), string.Empty);
                        XmlText text1 = xmlGraphDocument.CreateTextNode(GetGuid(vertex).ToString());
                        element3.AppendChild(text1);
                        xmlElementVertex.AppendChild(element3);

                        XmlElement element4 = xmlGraphDocument.CreateElement(string.Empty, nameof(IVertex.Weighted), string.Empty);
                        XmlText text2 = xmlGraphDocument.CreateTextNode(((int)(vertex.Weighted)).ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
                        element4.AppendChild(text2);
                        xmlElementVertex.AppendChild(element4);
                        return xmlElementVertex;
                    }
                    void CreateEdgeXml(IEdge edge, XmlElement xmlElementVertex)
                    {
                        XmlElement elementEdge = xmlGraphDocument.CreateElement(string.Empty, nameof(DataStructures.IEdge), string.Empty);
                        xmlElementVertex.AppendChild(elementEdge);

                        XmlElement element3 = xmlGraphDocument.CreateElement(string.Empty, nameof(DataStructures.IEdge.U), string.Empty);
                        //XmlText text1 = xmlGraphDocument.CreateTextNode("muh");
                        //element3.AppendChild(text1);
                        element3.SetAttribute(nameof(Guid), GetGuid(edge.U).ToString());
                        elementEdge.AppendChild(element3);

                        XmlElement element4 = xmlGraphDocument.CreateElement(string.Empty, nameof(DataStructures.IEdge.V), string.Empty);
                        element4.SetAttribute(nameof(Guid), GetGuid(edge.V).ToString());
                        elementEdge.AppendChild(element4);
                    }

                    //writer.WriteEndElement();

                    writer.Flush();

                }
                xml = sw.ToString();
            }
            return xml;
        }

        public class VertexSerializationSurrogate : ISerializationSurrogate
        {
            /// <summary>
            /// Serialize the Employee object to save the object's name and address fields.
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="info"></param>
            /// <param name="context"></param>
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {

            }

            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {
                return obj;
            }
        }

    }
}
