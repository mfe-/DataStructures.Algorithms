
using System.Xml.Serialization;
namespace Get.Model.Graph
{
    [XmlRoot("Edge")]
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
        [XmlElement("U")]
        public Vertex U { get { return u; } set { u = value; } }
        [XmlElement("V")]
        public Vertex V { get { return v; } set { v = value; } }

        public override string ToString()
        {
            return base.ToString() + " " + U.ToString() + " ->" + V.ToString();
        }
    }
}
