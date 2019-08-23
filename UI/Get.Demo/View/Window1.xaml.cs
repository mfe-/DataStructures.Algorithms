using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

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

            Vertex va = new Vertex(1);
            Vertex vb = new Vertex(2);
            Vertex vc = new Vertex(3);
            Vertex vd = new Vertex(4);
            Vertex ve = new Vertex(5);
            Vertex vf = new Vertex(6);
            Vertex vg = new Vertex(7);
            Vertex vh = new Vertex(8);
            Vertex vi = new Vertex(9);
            Vertex vj = new Vertex(10);
            Vertex vk = new Vertex(11);
            Vertex vl = new Vertex(12);

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

            que.Enqueue(() => vc.AddEdge(vb,3, graph.Directed));
            que.Enqueue(() => vc.AddEdge(vd,2, graph.Directed));

            que.Enqueue(() => vd.AddEdge(ve,2, graph.Directed));

            que.Enqueue(() => vg.AddEdge(vh,6, graph.Directed));
            que.Enqueue(() => vh.AddEdge(vi,10, graph.Directed));
            que.Enqueue(() => vi.AddEdge(vj,1, graph.Directed));
            que.Enqueue(() => vj.AddEdge(vf,2, graph.Directed));

            que.Enqueue(() => vh.AddEdge(vd,5, graph.Directed));
            que.Enqueue(() => vi.AddEdge(ve,7, graph.Directed));

            que.Enqueue(() => vg.AddEdge(vl,1, graph.Directed));
            que.Enqueue(() => vl.AddEdge(vk,8, graph.Directed));

            que.Enqueue(() => va.RemoveEdge(vb, graph.Directed));

            que.Enqueue(() => vh.Weighted=20);

            que.Enqueue(() => vc.Weighted = 11);

            graph.AddVertex(va);
            graph.Start = va;

            _GraphVisualization.Graph = graph;
            ManualResetEvent mre = new ManualResetEvent(false);

            Thread thread = new Thread(new ParameterizedThreadStart(delegate(object param)
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
        
        public void SimulateGraphLoadFromFile()
        {
            ApplicationCommands.Open.Execute(Environment.CurrentDirectory + "\\dijkstra.xml", _GraphVisualization);
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                //SimulateGraphChanges();
                SimulateGraphLoadFromFile();
                //GraphVisualization.SetDirectedRoutedCommand.Execute(false, _GraphVisualization);
                //_GraphVisualization.Graph = new Graph();

            }



            //var o = z.AdjacencyList();
            //var m = _GraphVisualization.Graph.AdjacencyList();

            //this.uout(m);
            //this.uout(o);

            //_GraphVisualization.Graph.addVertex(_GraphVisualization.Graph.Vertices.First().Edges.First().V);

            //var r = _GraphVisualization.Graph.AdjacencyList();





            //var o = Mathematics.gcd(2008, 6318);
            //int[] v1 = Mathematics.CreateMatrix(new int[] { 1, 5, 2 }, new int[] { 3, 2, 1 }, new int[] { 0, 1, 2 });

            //int[] v2 = Mathematics.CreateVector(1, 2, 3, 4, 5, 6, 7);
            //Vertex va = new Vertex(1);
            ////var r = v1.Add(v2);

            ////////graph.Load("Vertex.xml");

            //Int64 zu = 45;

            Graph graph = new Graph();

            Vertex va = new Vertex(1);
            Vertex vb = new Vertex(2);
            Vertex vc = new Vertex(3);
            Vertex vd = new Vertex(4);
            Vertex ve = new Vertex(5);
            Vertex vf = new Vertex(6);
            Vertex vg = new Vertex(7);
            Vertex vh = new Vertex(8);
            Vertex vi = new Vertex(9);
            Vertex vj = new Vertex(10);
            Vertex vk = new Vertex(11);
            Vertex vl = new Vertex(12);

            //va.addEdge(vb, 2);
            //va.addEdge(vd, 3);
            //va.addEdge(vc, 5);

            //vb.addEdge(vc, 4);
            //vb.addEdge(ve, 6);

            //vc.addEdge(ve, 4);
            //vc.addEdge(vd, 1);
            //vc.addEdge(vf, 1);

            //vd.addEdge(vf, 3);

            //vf.addEdge(ve, 2);

            //ve.addEdge(vg, 2);
            //ve.addEdge(vf, 2);

            //vg.addEdge(vf, 5);

            //vc.addEdge(vb);
            //vc.addEdge(vd);

            //vd.addEdge(ve);

            //vg.addEdge(vh);
            //vh.addEdge(vi);
            //vi.addEdge(vj);
            //vj.addEdge(vf);

            //vh.addEdge(vd);
            //vi.addEdge(ve);

            //vg.addEdge(vl);
            //vl.addEdge(vk);


            //graph.addVertex(va);
            //graph.StartVertex = va;

            //_GraphVisualization.Graph = graph;


            //var g = graph.Dijkstra(graph.StartVertex
            //Debug.WriteLine("a2");
            ////graph.Vertices.Add(new Vertex(3));

            //count all VertexControls
            ////////////int counter = _GraphVisualization.VertexVisualizationList.Count;

            ////////////timer.Interval = TimeSpan.FromMilliseconds((9 * 120));
            //////////////iterate from behind and save iteration position so that only one control in the eventhandler will be set IsFocused=true
            ////////////timer.Tick += new EventHandler(new EventHandler(delegate
            ////////////{
            ////////////    if (counter == -1) this.timer.Stop();
            ////////////    for (int i = counter; i > 0; i--)
            ////////////    {
            ////////////        Vertex v = _GraphVisualization.VertexVisualizationList[i - 1].Vertex;

            ////////////        _GraphVisualization.setFocus(v);
            ////////////        counter--;
            ////////////        return;

            ////////////    }
            ////////////}));
            //timer.Start();
            //http://www.mycsharp.de/wbb2/thread.php?postid=3702712#post3702712




            //Thread thread = new Thread(new ThreadStart(delegate
            //{
            //    foreach (Vertex v in _GraphVisualization.VertexVisualizationList.Vertex)
            //    {
            //        //do something with v

            //        _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
            //          delegate()
            //          {
            //              _GraphVisualization.setFocus(v);
            //          }));
            //        mre.WaitOne(9 * 120);
            //    }
            //}));
            //thread.Start();



            //Thread setFocusonControls = new Thread(new ThreadStart(delegate
            //{
            //    Vertex start;
            //    IList<Vertex> vlist = graph.Vertices;

            //    foreach (var a in vlist)
            //    {
            //        _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
            //            delegate()
            //            {
            //                _GraphVisualization.setFocus(a);
            //                Thread.Sleep(900);
            //                foreach (var b in a.Edges)
            //                {
            //                    _GraphVisualization.setFocus(b.U);
            //                    Thread.Sleep(900);
            //                    _GraphVisualization.setFocus(b.V);
            //                    Thread.Sleep(900);
            //                }
            //            }));
            //    }
            //}));
            //setFocusonControls.Start();

            //foreach (var a in graph.Vertices)
            //{
            //    _GraphVisualization.setFocus(a);
            //    Thread.Sleep(3000);
            //}

            //Get.Common.XML.WriteXmlSerializer(typeof(VertexVisualization), , _GraphVisualization.VertexVisualizationList.First());




            //brush.BeginAnimation(SolidColorBrush.ColorProperty, ani);

            //GetAllV(_GraphVisualization.Graph.Vertices.First(), null);

            //_GraphVisualization.Graph.Vertices.First().Depth_First_Traversal().ToList().ForEach(g=>{
            //    Debug.WriteLine(g);
            //});

            //Thread thread = new Thread(new ParameterizedThreadStart(delegate(object arr) { dft((Vertex)(arr), new List<Vertex>()); }));
            //thread.Start(_GraphVisualization.Graph.Vertices.First());

            //_GraphVisualization.Graph.Dijkstra(_GraphVisualization.Graph.StartVertex);

        }
        ManualResetEvent mre = new ManualResetEvent(false);
        public void GetAllV(Vertex v, Vertex firstVertex)
        {
            if (v == firstVertex) return;
            if (firstVertex == null) firstVertex = v;

            _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
              delegate()
              {
                  _GraphVisualization.SetFocus(v);
              }));
            mre.WaitOne(20 * 120);

            foreach (Edge e in v.Edges)
            {
                _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                  delegate()
                  {
                      _GraphVisualization.SetFocus(e);
                  }));
                mre.WaitOne(20 * 120);
                GetAllV(e.V, firstVertex);
                return;
            }

        }
        //http://www.cse.ohio-state.edu/~gurari/course/cis680/cis680Ch14.html#QQ1-46-90
        public void dft(Vertex v, List<Vertex> visited)
        {
            //visist x
            visited.Add(v);
            _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
              delegate()
              {
                  _GraphVisualization.SetFocus(v);
                  Debug.WriteLine(v + " visited");
              }));
            mre.WaitOne(10 * 120);
            //FOR each y such that (x,y) is an edge DO 
            foreach (Edge e in v.Edges)
            {
                _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                  delegate()
                  {
                      _GraphVisualization.SetFocus(e);
                      Debug.WriteLine(e + " visited");
                  }));
                mre.WaitOne(10 * 120);
                if (!visited.Exists(g => g.Equals(e.V)))
                {
                    dft(e.V, visited);
                }

            }
            Debug.WriteLine(visited.Count);

        }
        private void uout(int[][] _m)
        {
            string s = String.Empty;
            for (int z = 0; z < _m.Length; z++)
            {
                for (int y = 0; y < _m[z].Length; y++)
                {
                    s += _m[z][y].ToString() + " ";
                }
                s += "\n";
            }
            System.Diagnostics.Debug.WriteLine(s);
        }

    }
}
