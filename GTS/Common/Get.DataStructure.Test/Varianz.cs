using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Get.DataStructure.Test
{
    public class Varianz
    {
        public interface IData<T>
        {
            T Value { get; set; }
        }
        public interface IDataStructEdge<W,T> : IData<T>
            where W : IComparable<W>
        {
            W Weight { get; set; }
            IDataStructVertex<W,T> U { get; set; }
            IDataStructVertex<W,T> V { get; set; }
        }
        public interface IDataStructVertex<W,T>
            where W : IComparable<W>
        {
            W Weight { get; set; }
            IEnumerable<IDataStructEdge<W,T>> Edges { get; set; }
        }
        //Edge
        public class Edge<W,T> : IDataStructEdge<W,T>
            where W : IComparable<W>
        {

            public W Weight { get; set; }

            public IDataStructVertex<W,T> U { get; set; }

            public IDataStructVertex<W,T> V { get; set; }

            public T Value { get; set; }
        }
        //Vertex
        public class Vertex<W,T> : IDataStructVertex<W,T>
            where W : IComparable<W>
        {
            public W Weight { get; set; }
            public IEnumerable<IDataStructEdge<W,T>> Edges { get; set; }
        }
        public class RailVertex : Vertex<int,object>
        {
            public RailVertex()
            {
                this.Edges = new ObservableCollection<RailEdge>();
            }
        }
        public class RailEdge : Edge<int,object>
        {
            public RailEdge()
            {
                this.U = new RailVertex();
                this.V = new RailVertex();
            }
        }
    }
}
