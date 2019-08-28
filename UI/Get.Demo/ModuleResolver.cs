//using DataStructures.Demo;
//using System;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Xml;

//namespace DataStructures
//{
//    public class ModuleResolver : DataContractResolver
//    {
//        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
//        {
//            if (typeName == "VertexOfanyType" && typeNamespace == "http://schemas.get.com/winfx/2009/xaml/Graph")
//            {
//                return typeof(Vertex<object>);
//            }
//            else
//            {
//                // Defer to the known type resolver
//                return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
//            }
//        }

//        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
//        {
//            if (type == typeof(Vertex<IModule>))
//            {
//                XmlDictionary dictionary = new XmlDictionary();
//                typeName = dictionary.Add("VertexOfModuleFunction");
//                typeNamespace = dictionary.Add("http://schemas.get.com/winfx/2009/xaml/ModuleFunction");
//                return true; // indicating that this resolver knows how to handle "Dog"
//            }
//            else
//            {
//                // Defer to the known type resolver
//                return knownTypeResolver.TryResolveType(type, declaredType, null, out typeName, out typeNamespace);
//            }
//        }
//    }
//}
