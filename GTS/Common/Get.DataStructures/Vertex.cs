using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public class Vertex<W, T> : IVertex<W, T>
        where W : IComparable<W>
    {
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

        public T Data
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


        public IEnumerable<IEdge<W, T>> Edges
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
