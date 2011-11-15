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

namespace Get.Demo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
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

            //Get.Common.XML.WriteXmlSerializer(typeof(VertexVisualization), , _GraphVisualization.VertexVisualizationList.First());
        }
        public static void WriteObject(string fileName, Type pTypToSerialize,object instanceofTypeToSerialize)
        {
            Console.WriteLine(
                "Creating " + fileName);
            FileStream writer = new FileStream(fileName, FileMode.Create);
            DataContractSerializer ser =
                new DataContractSerializer(pTypToSerialize);
            ser.WriteObject(writer, instanceofTypeToSerialize);
            writer.Close();

        }

        public static void ReadObject(string fileName, Type pTypToSerialize)
        {
            Console.WriteLine("Deserializing an instance of the object.");
            FileStream fs = new FileStream(fileName,
            FileMode.Open);
            XmlDictionaryReader reader =
                XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(pTypToSerialize);

            // Deserialize the data and read it from the instance.
            //Person deserializedPerson =
            //    (Person)ser.ReadObject(reader, true);
            //reader.Close();
            //fs.Close();
            //Console.WriteLine(String.Format("{0} {1}, ID: {2}",
            //deserializedPerson.FirstName, deserializedPerson.LastName,
            //deserializedPerson.ID));
        }

    }
}
