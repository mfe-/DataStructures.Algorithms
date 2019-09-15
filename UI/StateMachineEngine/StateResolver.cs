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
            //if (typeName.Contains("VertexOfStateModule"))
            //{
            //    return typeof(Vertex<IState>);
            //}
            //else
            //{
            // Defer to the known type resolver
            return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
            //}
        }

        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            //forward the interface type of an actual implementation
            if (typeof(Vertex<IState>) == type)
            {
                //Graph.CreateVertexFunc creates Vertices of Type Vertex<IState> but deserializing them will create Vertex<StateModule>...
                //because of this currently editing of loaded vertices is not possible as the cast is done wrong

                Type StateType = typeof(Vertex<StateModule>);
                return knownTypeResolver.TryResolveType(StateType, declaredType, null, out typeName, out typeNamespace);

                //XmlDictionary dictionary = new XmlDictionary();
                //typeName = dictionary.Add("VertexOfModuleFunction");
                //typeNamespace = dictionary.Add("http://schemas.get.com/winfx/2009/xaml/ModuleFunction");
                //return true; // indicating that this resolver knows how to handle "Dog"
            }
            else
            {
                // Defer to the known type resolver
                return knownTypeResolver.TryResolveType(type, declaredType, null, out typeName, out typeNamespace);
            }
        }
    }
}
