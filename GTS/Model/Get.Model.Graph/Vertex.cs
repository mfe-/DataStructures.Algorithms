using System.Collections.ObjectModel;

namespace Get.Model.Graph
{
    public class Vertex
    {
        protected ObservableCollection<Edge> _Edges = new ObservableCollection<Edge>();

        protected int weighted;

        public Vertex() { }

        public Vertex(int pweighted)
        {
            weighted = pweighted;
        }
        public void addEge(Vertex pu)
        {
            _Edges.Add(new Edge(this, pu));
        }
        public void addEge(Vertex pu, int pweighted)
        {
            _Edges.Add(new Edge(this, pu, pweighted));
        }

        public int Weighted { get { return weighted;} set {weighted = value;} }

        public ObservableCollection<Edge> Edges { get { return _Edges; } set { _Edges = value; } }

        public override string ToString()
        {
            return base.ToString() + Weighted;
        }

    }
}
