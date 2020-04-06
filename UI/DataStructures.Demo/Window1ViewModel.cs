using Algorithms.Graph;
using Prism.Commands;
using System;
using System.Windows.Input;

namespace DataStructures.Demo
{
    public class Window1ViewModel : Prism.Mvvm.BindableBase
    {
        public Window1ViewModel()
        {
            PropertyChanged += Window1ViewModel_PropertyChanged;
        }

        private void Window1ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (nameof(Graph).Equals(e.PropertyName) && Graph != null)
            {
                if (Graph != null)
                {
                    Graph.CreateVertexFunc = VertexFactory;
                }
            }
        }

        public IVertex VertexFactory()
        {
            return new DataStructures.UI.Vertex();
        }
        private ICommand _ClickCommand;
        public ICommand ClickCommand => _ClickCommand ?? (_ClickCommand = new DelegateCommand<IVertex>(OnClickCommand));

        private void OnClickCommand(IVertex param)
        {
            if (param != null)
            {

            }
        }

        private ICommand _RunStateMachineCommand;
        public ICommand RunStateMachineCommand => _RunStateMachineCommand ?? (_RunStateMachineCommand = new DelegateCommand<IVertex>(OnRunStateMachineCommand));

        private async void OnRunStateMachineCommand(IVertex param)
        {

        }

        private Graph _Graph;
        public Graph Graph
        {
            get { return _Graph; }
            set { SetProperty(ref _Graph, value, nameof(Graph)); }
        }


        private ICommand _KruskalCommand;
        public ICommand KruskalCommand => _KruskalCommand ?? (_KruskalCommand = new DelegateCommand(OnKruskalCommand));

        protected void OnKruskalCommand()
        {
            Graph = Graph.KruskalDepthFirstSearch();
        }

        private ICommand _GenerateGridGraph;
        public ICommand GenerateGridGraph => _GenerateGridGraph ?? (_GenerateGridGraph = new DelegateCommand(OnGenerateGridGraph));

        protected void OnGenerateGridGraph()
        {
            Graph = null;
            Graph = Generate_Grid_Graph(4, 4);
        }
        public Graph Generate_Grid_Graph(int amount_width_vertices, int amount_height_vertices)
        {
            Func<int, IVertex> funFactory = (weight) => new Vertex() { Weighted = weight };

            Graph g = new Graph();
            IVertex[] lastVerticesRow = new IVertex[amount_width_vertices];
            IVertex[] lastyVerticesRow = new IVertex[amount_width_vertices];

            for (int y = 0; y < amount_height_vertices; y++)
            {
                for (int x = 0; x < amount_width_vertices; x++)
                {
                    IVertex currentVertex = funFactory.Invoke(x);
                    lastVerticesRow[x] = currentVertex;
                    //connect previous vertex on x axis
                    if (x - 1 >= 0)
                    {
                        currentVertex.AddEdge(lastVerticesRow[x - 1], x, true);
                        lastVerticesRow[x - 1].AddEdge(currentVertex, x, true);
                    }
                    //connect previous vertex on y axis
                    if (lastyVerticesRow[x] != null)
                    {
                        currentVertex.AddEdge(lastyVerticesRow[x], y, false);
                    }
                }
                if (g.Start == null)
                {
                    g.Start = lastVerticesRow[0];
                }
                lastVerticesRow.CopyTo(lastyVerticesRow, 0);
            }
            return g;
        }

    }
}
