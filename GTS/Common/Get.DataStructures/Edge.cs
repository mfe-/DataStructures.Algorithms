using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public class Edge<W,T> : IEdge<W,T> where W : IComparable<W>
    {
        public Edge()
        {
            
        }


        public W Weight
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IVertex<W, T> U
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IVertex<W, T> V
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
