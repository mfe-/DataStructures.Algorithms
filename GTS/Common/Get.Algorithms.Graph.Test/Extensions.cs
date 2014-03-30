using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Get.DataStructure;
using Get.Algorithms.Graph;
using System.Collections.ObjectModel;

namespace Get.Algorithms.Graph.Test
{
    [TestClass]
    public class Extensions
    {

        [TestMethod]
        public void ExtensionCalls()
        {
            Vertex<int,double> v1 = new Vertex<int,double>();
            v1.Depth_First_Traversal<int,double>(new List<Vertex<int,double>>());

            Get.Algorithms.Graph.Test.ExtendDataStructGraph.ConnectionVertex connv = new ExtendDataStructGraph.ConnectionVertex();
            
            connv.Depth_First_Traversal(new List<Get.Algorithms.Graph.Test.ExtendDataStructGraph.ConnectionVertex>());


        }
    }
}
