using System.Linq;
using Xunit;

namespace DataStructures.Test
{
    public class EdgesTest
    {
        readonly Graph g;
        readonly IVertex v1;
        readonly IVertex v2;
        readonly IVertex v3;
        readonly IVertex v4;
        readonly IVertex v5;
        readonly IVertex v6;
        readonly IVertex v7;

        public EdgesTest()
        {
            v1 = new Vertex<object>() { Weighted = 1 };
            v2 = new Vertex<object>() { Weighted = 2 };
            v3 = new Vertex<object>() { Weighted = 3 };
            v4 = new Vertex<object>() { Weighted = 4 };
            v5 = new Vertex<object>() { Weighted = 5 };
            v6 = new Vertex<object>() { Weighted = 6 };
            v7 = new Vertex<object>() { Weighted = 7 };
            Graph g = new Graph();

            g.AddVertex(v1);

            v1.AddEdge(v2, 2);
            v1.AddEdge(v3, 5);
            v1.AddEdge(v4, 3);

            v2.AddEdge(v3, 4);
            v2.AddEdge(v5, 6);

            v3.AddEdge(v5, 4);
            v3.AddEdge(v6, 1);
            v3.AddEdge(v4, 1);

            v4.AddEdge(v6, 3);

            v5.AddEdge(v6, 2);

            v6.AddEdge(v7, 5);

            v7.AddEdge(v5, 2);

            this.g = g;
        }
        [Fact]
        public void Edge_from_V_to_U_and_Edge_from_U_to_V_should_have_the_same_HashCode()
        {
            //hascode of transported edges must be the same
            IEdge e1 = g.Vertices.First().Edges.First();
            //create a transported edge
            e1.V.AddEdge(e1.U, e1.Weighted);

            IEdge e2 = e1.V.Edges.Last();

            //transported edge needs same hascode
            Assert.Equal(e1.GetHashCode(), e2.GetHashCode());

            //random edge - diffrent hascode
            Assert.NotEqual(e1.GetHashCode(), v4.Edges.First().GetHashCode());
        }
        [Fact]
        public void Two_edges_with_the_same_U_and_V_should_return_the_same_HasHode()
        {
            IVertex vertex1 = new Vertex<object>();
            Vertex<object> vertex2 = new Vertex<object>();

            IEdge edge1 = vertex1.AddEdge(vertex2, directed: false);
            IEdge edge2 = vertex1.AddEdge(vertex2, directed: false);

            Assert.Equal(edge1.GetHashCode(), edge2.GetHashCode());
        }
        [Fact]
        public void Two_edges_with_the_diffrent_U_and_V_should_return_the_diffrent_HasHodes()
        {
            IVertex vertex1 = new Vertex<object>();
            Vertex<object> vertex2 = new Vertex<object>();

            IEdge edge1 = vertex1.AddEdge(vertex2, directed: false);
            IEdge edge2 = vertex2.AddEdge(vertex2, directed: false);

            Assert.NotEqual(edge1.GetHashCode(), edge2.GetHashCode());
        }
    }
}
