using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Get.the.Solution.DataStructure;
using System.Collections.ObjectModel;

namespace Get.the.Solution.Algorithms.Graph.Test
{
    [TestClass]
    public class ExtendDataStructGraph
    {
        public class Rail { }
        public class RailEdge : Edge<double,Rail>
        {
            public RailEdge()
            {
                this.U = new ConnectionVertex();
            }
        }
        public class ConnectionVertex : Vertex<double, Rail>
        {
            public ConnectionVertex()
            {
                this._Edges = new ObservableCollection<RailEdge>();

            }
        }
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
