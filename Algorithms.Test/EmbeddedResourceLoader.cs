using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Algorithms.Test
{
    public static class EmbeddedResourceLoader
    {
        /// <summary>
        /// loads the embedded string resource 
        /// </summary>
        /// <param name="sampleFile"></param>
        /// <returns>The loaded string</returns>
        internal static IEnumerable<string> GetFileContents(string sampleFile)
        {
            //loads a embedded resource file with namespace "DataStructures.Test.{0}"
            var asm = Assembly.GetExecutingAssembly();
            var resource = $"{asm.GetName().Name}.{sampleFile}";
            using (var stream = asm.GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            string s = reader.ReadLine();
                            yield return s;
                        }

                    }
                }
            }
        }
        internal static XmlElement GetElement(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }
    }
}
