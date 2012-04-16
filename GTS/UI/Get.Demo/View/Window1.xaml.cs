using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Get.Model.Graph;
using Get.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Diagnostics;
using Get.Common.Mathematics;

namespace Get.Demo
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
        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                ApplicationCommands.Open.Execute(Environment.CurrentDirectory + "\\graph.xml", _GraphVisualization);
            }
            //var o = Mathematics.gcd(2008, 6318);

            //Graph graph = new Graph();
            ////graph.Load("Vertex.xml");

            //Vertex va = new Vertex(1);
            //Vertex vb = new Vertex(2);
            //Vertex vc = new Vertex(3);
            //Vertex vd = new Vertex(4);
            //Vertex ve = new Vertex(5);
            //Vertex vf = new Vertex(6);
            //Vertex vg = new Vertex(7);
            //Vertex vh = new Vertex(8);
            //Vertex vi = new Vertex(9);
            //Vertex vj = new Vertex(10);
            //Vertex vk = new Vertex(11);
            //Vertex vl = new Vertex(12);

            //va.addEdge(vc);
            //va.addEdge(vd);
            //va.addEdge(ve);
            //va.addEdge(vf);
            //va.addEdge(vk);

            //vc.addEdge(vg);
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


            //graph.addVertec(va);

            //_GraphVisualization.Graph = graph;

            //graph.Vertices.Add(new Vertex(3));

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

            //GetAllV(graph.Vertices.First(), null);

            //_GraphVisualization.Graph.Vertices.First().Depth_First_Traversal().ToList().ForEach(g=>{
            //    Debug.WriteLine(g);
            //});

            Thread thread = new Thread(new ParameterizedThreadStart(delegate(object arr) { dft((Vertex)(arr), new List<Vertex>()); }));
            thread.Start(_GraphVisualization.Graph.Vertices.First());

        }
        ManualResetEvent mre = new ManualResetEvent(false);
        public void GetAllV(Vertex v, Vertex firstVertex)
        {
            if (v == firstVertex) return;
            if (firstVertex == null) firstVertex = v;

            _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
              delegate()
              {
                  _GraphVisualization.setFocus(v);
              }));
            mre.WaitOne(20 * 120);

            foreach (Edge e in v.Edges)
            {
                _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                  delegate()
                  {
                      _GraphVisualization.setFocus(e);
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
                  _GraphVisualization.setFocus(v);
                  Debug.WriteLine(v + " visited");
              }));
            mre.WaitOne(10 * 120);
            //FOR each y such that (x,y) is an edge DO 
            foreach (Edge e in v.Edges)
            {
                _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                  delegate()
                  {
                      _GraphVisualization.setFocus(e);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            //Thread setFocusonControls = new Thread(new ThreadStart(delegate
            //{

            //    _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
            //        delegate()
            //        {
            //            foreach (var o in this._GraphVisualization.VertexVisualizationList)
            //            {
            //                o.Focus();
            //                Thread.Sleep(400);
            //            }
            //        }));

            //}));
            //setFocusonControls.Start();
            //foreach (var item in _GraphVisualization.VertexVisualizationList)
            //{

            //    item.Focus();

            //}


            //http://www.mycsharp.de/wbb2/thread.php?postid=3701905#post3701905
            //Thread setFocusonControls = new Thread(new ThreadStart(delegate
            //{

            //    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
            //        delegate()
            //        {
            //            foreach (var item in this.buttonlist)
            //            {
            //                item.Background = Brushes.Green;
            //                Thread.Sleep(900);


            //            }
            //        }));

            //}));
            //setFocusonControls.Start();






        }

    }
}
