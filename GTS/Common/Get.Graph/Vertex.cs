using System.Collections.ObjectModel;

namespace Get.Model.Graph
{
    public class Vertex
    {
        protected ObservableCollection<Edge> Edges = new ObservableCollection<Edge>();

        protected int weighted;

        public Vertex() { }

        public Vertex(int pweighted)
        {
            weighted = pweighted;
        }
        public void addEge(Vertex pu)
        {
            Edges.Add(new Edge(this, pu));
        }
        public void addEge(Vertex pu, int pweighted)
        {
            Edges.Add(new Edge(this, pu, pweighted));
        }

        public int Weighted { get { return weighted;} set {weighted = value;} }

    }
}
