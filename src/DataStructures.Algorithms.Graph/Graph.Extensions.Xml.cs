using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace DataStructures.Algorithms.Graph.Xml
{
    /// <summary>
    /// Provides a set of static methods for working with graph objects.
    /// </summary>
    public static partial class GraphExtensionsXml
    {
        private static DataContractSerializerSettings GetDataContractSerializerSettings()
        {
            return GetDataContractSerializerSettings(new List<Type>());
        }
        private static DataContractSerializerSettings GetDataContractSerializerSettings(List<Type> knownTypes, DataContractResolver? dataContractResolver = null)
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
        /// <summary>
        /// Deserialize the Graph from a xml file
        /// </summary>
        /// <param name="g">The target instance which will be used to load the graph</param>
        /// <param name="pfilename">path to the graph xml file</param>
        /// <param name="maxDepth"></param>
        /// <param name="DataContractSerializerSettingsActionInvokrer"></param>
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
                    var readObject = serializer.ReadObject(reader, true);
                    if (readObject is DataStructures.Graph a)
                    {
                        foreach (var v in a.Vertices)
                        {
                            g.Vertices.Add(v);
                        }
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
                object? deserialized = ndcs.ReadObject(memoryStream);
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
                    throw new NotSupportedException($"When reading from the Stream the type {nameof(DataStructures.Graph)} was expected but got {deserialized?.GetType()}");
                }
            }
        }

    }
}
