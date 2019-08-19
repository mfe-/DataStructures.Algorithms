using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Get.the.Solution.DataStructure;
using Get.the.Solution.Algorithms;

namespace Get.the.Solution.Algorithms.Graph.Test
{
    [TestClass]
    public class ExtensionsTest
    {

        [TestMethod]
        public void ExtensionCalls()
        {
            Vertex<int,double> v1 = new Vertex<int,double>();
            v1.Depth_First_Traversal<int,double>(new List<Vertex<int,double>>());

            Get.the.Solution.Algorithms.Graph.Test.ExtendDataStructGraph.ConnectionVertex connv = new ExtendDataStructGraph.ConnectionVertex();

            connv.Depth_First_Traversal(new List<Get.the.Solution.Algorithms.Graph.Test.ExtendDataStructGraph.ConnectionVertex>());


        }
    }
}
