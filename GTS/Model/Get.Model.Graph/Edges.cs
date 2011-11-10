
namespace Get.Model.Graph
{
    public class Edge
    {
        protected Vertex u;
        protected Vertex v;

        protected int weighted;

        public Edge(Vertex pu, Vertex pv)
        {
            u = pu;
            v = pv;
        }

        public Edge(Vertex pu, Vertex pv, int pweighted)
        {
            u = pu;
            v = pv;
            weighted = pweighted;
        }

        public Vertex U { get { return u; } set { u = value; } }
        public Vertex V { get { return v; } set { v = value; } }

        public override string ToString()
        {
            return base.ToString() +" " + U.ToString() +" ->" + V.ToString();
        }
    }
}
