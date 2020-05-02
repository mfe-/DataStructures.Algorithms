using Algorithms.Graph;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

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
        private IVertex _SelectedVertex;
        public IVertex SelectedVertex
        {
            get { return _SelectedVertex; }
            set
            {
                PreviousSelectedVertex = _SelectedVertex;
                SetProperty(ref _SelectedVertex, value, nameof(SelectedVertex));
            }
        }
        private IVertex _PreviousSelectedVertex;
        public IVertex PreviousSelectedVertex
        {
            get { return _PreviousSelectedVertex; }
            set { SetProperty(ref _PreviousSelectedVertex, value, nameof(PreviousSelectedVertex)); }
        }

        private ICommand _AStarCommand;
        public ICommand AStarCommand => _AStarCommand ?? (_AStarCommand = new DelegateCommand<IVertex>(OnAStarCommand));

        protected async void OnAStarCommand(IVertex vertex)
        {

            Point goalPoint = (vertex as IVertex<Point>).Value;
            Func<IVertex, double> funcManhattanDistanceHeuristic = new Func<IVertex, double>((vertex) =>
            {
                Point currentPoint = ((IVertex<Point>)vertex).Value;
                return Math.Abs(currentPoint.X - goalPoint.X) + Math.Abs(currentPoint.Y - goalPoint.Y);
            });

            Func<IVertex, double> funcEuclideanDistanceHeuristic = new Func<IVertex, double>((vertex) =>
            {
                Point currentPoint = ((IVertex<Point>)vertex).Value;
                return Math.Sqrt(Math.Pow((currentPoint.X - currentPoint.X), 2) + Math.Pow((currentPoint.Y - currentPoint.Y), 2));
            });


            var result = PreviousSelectedVertex.AStar(vertex, funcEuclideanDistanceHeuristic);
            var result2 = result.ReconstructPath(vertex);

            Queue<Action> queue = new Queue<Action>();
            foreach (IVertex item in result2)
            {
                queue.Enqueue(() => SelectedVertex = item);
            }

            while (queue.Count != 0)
            {
                Action action = queue.Dequeue();
                action();
                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(true);
            }

        }


        private ICommand _BreadthFirstSearchCommand;
        public ICommand BreadthFirstSearchCommand => _BreadthFirstSearchCommand ?? (_BreadthFirstSearchCommand = new DelegateCommand<IVertex>(OnBreadthFirstSearchCommand));

        protected async void OnBreadthFirstSearchCommand(IVertex vertex)
        {
            Queue<Action> queue = new Queue<Action>();

            var result = vertex.BreadthFirstSearchQueue((v) => queue.Enqueue(() => SelectedVertex = v));

            while (queue.Count != 0)
            {
                Action action = queue.Dequeue();
                action();
                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(true);
            }
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
            IVertex VertexFactoryDouble(int x, int y)
            {
                var vertex = new UI.Vertex<Point>();
                vertex.Value = new Point(x, y);
                return vertex;
            }
            Graph = GraphExtensions.GenerateGridGraph(4, 4, VertexFactoryDouble, edgeWeight: 0.1);
        }

    }
}
