using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;

namespace RailNetwork
{
    public class RailEdge : Edge<double, Rail>
    {
        public RailEdge()
        {
            this.U = new ConnectionVertex();
            

        }
    }
}
