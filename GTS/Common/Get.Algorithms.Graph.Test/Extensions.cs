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
        public class Rail{ }
        public class RailEdge : Edge<double, Rail>
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
        public void ExtensionCalls()
        {
            Vertex<int, object> v1 = new Vertex<int, object>();
            v1.Depth_First_Traversal<int, object>(new List<Vertex<int, object>>());

            ConnectionVertex connv = new ConnectionVertex();
            connv.Depth_First_Traversal(new List<ConnectionVertex>());


        }
    }
}
