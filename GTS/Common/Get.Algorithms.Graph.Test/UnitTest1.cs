using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Get.DataStructure;
using Get.Algorithms.Graph;

namespace Get.Algorithms.Graph.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Vertex<int, object> v1 = new Vertex<int, object>();
            v1.Depth_First_Traversal<int, object>(new List<Vertex<int, object>>());
        }
    }
}
