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

namespace Get.Demo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        List<Button> buttonlist = new List<Button>();
        public Window1()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window1_Loaded);
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            Graph graph = new Graph();

            Vertex v1 = new Vertex(1);
            Vertex v2 = new Vertex(2);

            Vertex v3 = new Vertex(3);
            Vertex v4 = new Vertex(4);

            Vertex v5 = new Vertex(5);
            Vertex v6 = new Vertex(6);

            v1.addEdge(v2);
            v2.addEdge(v3);
            v3.addEdge(v4);
            v4.addEdge(v1);
            v1.addEdge(v3);



            graph.addVertec(v1);
            //Get.Common.XML.WriteXmlSerializer(typeof(Graph), Environment.CurrentDirectory +"\\graph.xml", graph);
            WriteObject(Environment.CurrentDirectory + "\\vertex.xml", typeof(Graph), graph);
            _GraphVisualization.Graph = graph;

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
        }
        public static void WriteObject(string fileName, Type pTypToSerialize, object instanceofTypeToSerialize)
        {
            Console.WriteLine(
                "Creating " + fileName);
            FileStream writer = new FileStream(fileName, FileMode.Create);
            DataContractSerializer ser =
                new DataContractSerializer(pTypToSerialize);
            ser.WriteObject(writer, instanceofTypeToSerialize);
            writer.Close();

        }

        public object ReadObject(string fileName, Type pTypToSerialize)
        {
            Console.WriteLine("Deserializing an instance of the object.");
            FileStream fs = new FileStream(fileName,
            FileMode.Open);
            XmlDictionaryReader reader =
                XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(pTypToSerialize);


            // Deserialize the data and read it from the instance.
            object deserializedobject = ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            return deserializedobject;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this._GraphVisualization.VertexVisualizationList.First().Focus();

            //Thread setFocusonControls = new Thread(new ThreadStart(delegate
            //{

            //    _GraphVisualization.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
            //        delegate()
            //        {

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
