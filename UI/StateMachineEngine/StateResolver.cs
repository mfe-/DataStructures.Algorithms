using DataStructures;
using System;
using System.Runtime.Serialization;
using System.Xml;

namespace StateMachineEngine
{
    public class StateResolver : DataContractResolver
    {
        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            if (typeName == "VertexOfanyType" && typeNamespace == "http://schemas.get.com/winfx/2009/xaml/Graph")
            {
                return typeof(Vertex<object>);
            }
            if (typeName.Contains(typeof(Vertex<IState>).ToString()))
            {
                return typeof(Vertex<IState>);
            }
            return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
        }

        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            //forward the interface type of an actual implementation
            if (typeof(Vertex<IState>) == type)
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add(typeof(Vertex<IState>).ToString());
                typeNamespace = dictionary.Add("http://schemas.get.com/winfx/2009/xaml/IState");
                return true;
            }
            else
            {
                // Defer to the known type resolver
                return knownTypeResolver.TryResolveType(type, declaredType, null, out typeName, out typeNamespace);
            }
        }
    }
}
