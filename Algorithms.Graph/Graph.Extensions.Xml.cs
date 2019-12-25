using DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Algorithms.Graph
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
        public static DataContractSerializerSettings GetDataContractSerializerSettings(List<Type> knownTypes, DataContractResolver dataContractResolver = null)
        {
            List<Type> types = new List<Type>() { typeof(Vertex<object>), typeof(Edge<object>) };
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
        public static XElement Save(this DataStructures.Graph g, Action<DataContractSerializerSettings> DataContractSerializerSettingsActionInvokrer = null)
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
        public static void Load(this DataStructures.Graph g, String pfilename, int maxDepth, Action<DataContractSerializerSettings> DataContractSerializerSettingsActionInvokrer = null)
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
        public static DataStructures.Graph Load(this XElement e, Action<DataContractSerializerSettings> DataContractSerializerSettingsActionInvokrer = null)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            DataStructures.Graph g = new DataStructures.Graph();
            //load graph
            MemoryStream memoryStream = new MemoryStream();
            e.Save(memoryStream);
            memoryStream.Position = 0;
            DataContractSerializerSettings dataContractSerializerSettings = GetDataContractSerializerSettings();
            DataContractSerializerSettingsActionInvokrer?.Invoke(dataContractSerializerSettings);
            DataContractSerializer ndcs = new DataContractSerializer(g.GetType(), dataContractSerializerSettings);
            DataStructures.Graph u = ndcs.ReadObject(memoryStream) as DataStructures.Graph;

            g.Start = u.Start;

            g.Directed = u.Directed;

            foreach (IVertex v in u.Vertices)
                g.Vertices.Add(v);
            return g;
        }
    }
}
