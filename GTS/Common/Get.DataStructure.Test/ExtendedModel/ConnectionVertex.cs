using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Get.DataStructure;

namespace RailNetwork
{
    public class ConnectionVertex : Vertex<double, Rail>
    {
        public ConnectionVertex()
        {
            this._Edges = new ObservableCollection<RailEdge>();

        }
    }
}
