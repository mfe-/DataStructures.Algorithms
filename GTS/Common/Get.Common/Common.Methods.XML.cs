using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Get.Common
{
    public sealed static class XML
    {
        public static void WriteXmlSerializer(Type pTypeToSerialize, string pPathToXMLFile, Object pObjectToSave)
        {
            XmlSerializer s = new XmlSerializer(pTypeToSerialize);
            TextWriter w = new StreamWriter(pPathToXMLFile);
            s.Serialize(w, pObjectToSave);
            w.Close();
        }
        public static Object LoadXMLSerializer(Type pTypeToSerialize, string pPathToXMLFile)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(pTypeToSerialize);

            FileStream stream = new FileStream(pPathToXMLFile,FileMode.Open);
            XmlReader reader = new XmlTextReader(stream);

            return xmlSerializer.Deserialize(reader);
        }
    }
}
