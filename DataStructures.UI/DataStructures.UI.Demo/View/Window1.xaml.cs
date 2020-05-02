using Algorithms.Graph;
using DataStructures.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static Algorithms.Graph.GraphExtensions;

namespace DataStructures.Demo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        public Window1()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window1_Loaded);

        }

        public void SimulateGraphChanges()
        {
            Graph graph = new Graph(false);

            UI.Vertex va = new UI.Vertex(1);
            UI.Vertex vb = new UI.Vertex(2);
            UI.Vertex vc = new UI.Vertex(3);
            UI.Vertex vd = new UI.Vertex(4);
            UI.Vertex ve = new UI.Vertex(5);
            UI.Vertex vf = new UI.Vertex(6);
            UI.Vertex vg = new UI.Vertex(7);
            UI.Vertex vh = new UI.Vertex(8);
            UI.Vertex vi = new UI.Vertex(9);
            UI.Vertex vj = new UI.Vertex(10);
            UI.Vertex vk = new UI.Vertex(11);
            UI.Vertex vl = new UI.Vertex(12);

            Queue<Action> que = new Queue<Action>();

            que.Enqueue(() => va.AddEdge(vb, 2, graph.Directed));
            que.Enqueue(() => va.AddEdge(vd, 3, graph.Directed));
            que.Enqueue(() => va.AddEdge(vc, 5, graph.Directed));

            que.Enqueue(() => vb.AddEdge(vc, 4, graph.Directed));
            que.Enqueue(() => vb.AddEdge(ve, 6, graph.Directed));

            que.Enqueue(() => vc.AddEdge(ve, 4, graph.Directed));
            que.Enqueue(() => vc.AddEdge(vd, 1, graph.Directed));
            que.Enqueue(() => vc.AddEdge(vf, 1, graph.Directed));

            que.Enqueue(() => vd.AddEdge(vf, 3, graph.Directed));

            que.Enqueue(() => vf.AddEdge(ve, 2, graph.Directed));

            que.Enqueue(() => ve.AddEdge(vg, 2, graph.Directed));
            que.Enqueue(() => ve.AddEdge(vf, 2, graph.Directed));

            que.Enqueue(() => vg.AddEdge(vf, 5, graph.Directed));

            que.Enqueue(() => vc.AddEdge(vb, 3, graph.Directed));
            que.Enqueue(() => vc.AddEdge(vd, 2, graph.Directed));

            que.Enqueue(() => vd.AddEdge(ve, 2, graph.Directed));

            que.Enqueue(() => vg.AddEdge(vh, 6, graph.Directed));
            que.Enqueue(() => vh.AddEdge(vi, 10, graph.Directed));
            que.Enqueue(() => vi.AddEdge(vj, 1, graph.Directed));
            que.Enqueue(() => vj.AddEdge(vf, 2, graph.Directed));

            que.Enqueue(() => vh.AddEdge(vd, 5, graph.Directed));
            que.Enqueue(() => vi.AddEdge(ve, 7, graph.Directed));

            que.Enqueue(() => vg.AddEdge(vl, 1, graph.Directed));
            que.Enqueue(() => vl.AddEdge(vk, 8, graph.Directed));

            que.Enqueue(() => va.RemoveEdge(vb, graph.Directed));

            que.Enqueue(() => vh.Weighted = 20);

            que.Enqueue(() => vc.Weighted = 11);

            graph.AddVertex(va);
            graph.Start = va;

            _GraphVisualization.Graph = graph;
            ManualResetEvent mre = new ManualResetEvent(false);

            Thread thread = new Thread(new ParameterizedThreadStart(delegate (object param)
            {
                while (que.Count != 0)
                {
                    Action action = que.Dequeue();
                    Dispatcher.BeginInvoke(action);

                    mre.WaitOne(10 * 120);
                }
            }));
            thread.Start();

        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {

            _GraphVisualization.DataContractSerializerSettingsActionInvoker =
                new Action<DataContractSerializerSettings>(dataContractSerializerSettingsActionInvoker =>
                {
                    List<Type> types = new List<Type>(dataContractSerializerSettingsActionInvoker.KnownTypes);
                    types.Add(typeof(UI.Vertex<Point>));
                    //types.Add(typeof(StateModule));
                    //types.Add(typeof(Vertex<StateModule>));
                    //types.Add(typeof(Edge<StateModule>));
                    dataContractSerializerSettingsActionInvoker.KnownTypes = types;
                    //do type test
                    var vertex = typeof(UI.Vertex);
                    var edge = typeof(UI.Edge);
                    dataContractSerializerSettingsActionInvoker.DataContractResolver = new VertexEdgeMapResolver(vertex, edge);
                });
            _GraphVisualization.EdgeFactory = (v) => new UI.Edge(v, null);
            _GraphVisualization.LoadGraphFunc = GraphExtensions.Load;
            _GraphVisualization.GraphSaveFunc = GraphExtensions.Save;
            if (Debugger.IsAttached)
            {
                ApplicationCommands.Open.Execute(Environment.CurrentDirectory + "\\dijkstra.xml", _GraphVisualization);
                //SimulateGraphChanges();


                //GraphVisualization.SetDirectedRoutedCommand.Execute(false, _GraphVisualization);
                //_GraphVisualization.Graph = new Graph();

            }

        }
    }
}
