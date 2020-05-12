using DataStructures.Algorithms.Graph;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataStructures.Demo
{
    public class Window1ViewModel : Prism.Mvvm.BindableBase
    {
        public Window1ViewModel()
        {
            PropertyChanged += Window1ViewModel_PropertyChanged;
            ClickCommand = new DelegateCommand<IVertex>(OnClickCommand);
            AStarCommand = new DelegateCommand<IVertex>(OnAStarCommand);
            BreadthFirstSearchCommand = new DelegateCommand<IVertex>(OnBreadthFirstSearchCommand);
            KruskalCommand = new DelegateCommand(OnKruskalCommand);
            GenerateGridGraph = new DelegateCommand(OnGenerateGridGraph);
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
        public ICommand ClickCommand { get; }

        private void OnClickCommand(IVertex param)
        {
            if (param != null)
            {
                //currently nothing to do here
            }
        }

        private Graph? _Graph;
        public Graph? Graph
        {
            get { return _Graph; }
            set { SetProperty(ref _Graph, value, nameof(Graph)); }
        }
        private IVertex? _SelectedVertex;
        public IVertex? SelectedVertex
        {
            get { return _SelectedVertex; }
            set
            {
                PreviousSelectedVertex = _SelectedVertex;
                SetProperty(ref _SelectedVertex, value, nameof(SelectedVertex));
            }
        }
        private IVertex? _PreviousSelectedVertex;
        public IVertex? PreviousSelectedVertex
        {
            get { return _PreviousSelectedVertex; }
            set { SetProperty(ref _PreviousSelectedVertex, value, nameof(PreviousSelectedVertex)); }
        }

        public ICommand AStarCommand { get; }

        protected async void OnAStarCommand(IVertex vertex)
        {
            Func<IVertex, double>? funcManhattanDistanceHeuristic = null;
            if (vertex is IVertex<Point> v)
            {
                Point goalPoint = v.Value;
                funcManhattanDistanceHeuristic = new Func<IVertex, double>((vertex) =>
                {
                    Point currentPoint = ((IVertex<Point>)vertex).Value;
                    return Math.Abs(currentPoint.X - goalPoint.X) + Math.Abs(currentPoint.Y - goalPoint.Y);
                });
                //Func<IVertex, double> funcEuclideanDistanceHeuristic = new Func<IVertex, double>((vertex) =>
                //{
                //    Point currentPoint = ((IVertex<Point>)vertex).Value;
                //    return Math.Sqrt(Math.Pow((currentPoint.X - currentPoint.X), 2) + Math.Pow((currentPoint.Y - currentPoint.Y), 2));
                //});
            }

            var result = PreviousSelectedVertex.AStar(vertex, funcManhattanDistanceHeuristic);
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

        public ICommand BreadthFirstSearchCommand { get; }

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

        public ICommand KruskalCommand { get; }

        protected void OnKruskalCommand()
        {
            Graph = Graph.KruskalDepthFirstSearch();
        }


        public ICommand GenerateGridGraph { get; } 

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
