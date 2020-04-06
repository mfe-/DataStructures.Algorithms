using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace DataStructures.UI
{
    /// <summary>
    /// Maps VertexOfanyType and EdgeOfanyType to the overgiven type
    /// </summary>
    public class VertexEdgeMapResolver : DataContractResolver
    {
        private readonly Type _vertexType;
        private readonly Type _edgeType;

        public VertexEdgeMapResolver(Type vertex, Type edge)
        {
            _vertexType = vertex;
            _edgeType = edge;
        }
        public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            if (knownTypeResolver == null) throw new ArgumentNullException(nameof(knownTypeResolver));
            if (dataContractType == _vertexType)
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add("VertexOfanyType");
                typeNamespace = dictionary.Add("http://schemas.get.com/Graph/Vertex");
                return true;
            }
            if (dataContractType == _edgeType)
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add("EdgeOfanyType");
                typeNamespace = dictionary.Add("http://schemas.get.com/Graph/Edges");
                return true;
            }

            return knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace);

        }
        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            if (knownTypeResolver == null) throw new ArgumentNullException(nameof(knownTypeResolver));
            if (typeName == "VertexOfanyType" && typeNamespace == "http://schemas.get.com/Graph/Vertex")
            {
                return _vertexType;
            }
            if (typeName == "EdgeOfanyType" && typeNamespace == "http://schemas.get.com/Graph/Edges")
            {
                return _edgeType;
            }
            return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);

        }
    }
}
