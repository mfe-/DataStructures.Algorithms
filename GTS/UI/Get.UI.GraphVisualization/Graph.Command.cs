using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Input;
using Get.Model.Graph;
using System.Windows;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Get.UI
{
    public partial class GraphVisualization : Canvas
    {
        #region Save Command

        public GraphVisualization()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Load_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphVisualization.AddVertexRoutedCommand, AddVertex_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Print_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphVisualization.SetDirectedRoutedCommand,SetDirected_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphVisualization.KruskalRoutedCommand,Kruskal_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphVisualization.ClearGraphCommand, ClearGraph_Executed));

        }
        private void ClearGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && sender.GetType().Equals(typeof(GraphVisualization)))
            {
                this.Graph = null;
                this.Graph = new Graph();
            }

        }
        private void Kruskal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && sender.GetType().Equals(typeof(GraphVisualization)))
            {
                
                Graph kruskal = this.Graph.Kruskal_DepthFirstSearch();
                this.Graph = null;
                this.Graph = kruskal;
            }

        }

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && sender.GetType().Equals(typeof(GraphVisualization)))
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                { 
                    printDialog.PrintVisual(sender as GraphVisualization, "GraphVisualization"); 
                }
            }

        }


        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //http://stackoverflow.com/questions/2522380/get-a-bitmap-image-from-a-control-view
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)FocusedFrameworkElement.ActualWidth, (int)FocusedFrameworkElement.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(this);
            //todo funktioniert noch nicht
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            MemoryStream stream = new MemoryStream();
            png.Save(stream);
            //System.Drawing.Image image = Image.FromStream(stream);

            Clipboard.SetImage(rtb);

        }
        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (FocusedFrameworkElement.GetType().Equals(typeof(VertexVisualization)))
            {
                VertexVisualization vv = FocusedFrameworkElement as VertexVisualization;

                Vertex newVertex = new Vertex();

                newVertex.Weighted = vv.Vertex.Weighted;

                this.Graph.addVertex(newVertex);

            }
        }

        #region Delete Command

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (FocusedFrameworkElement.GetType().Equals(typeof(VertexVisualization)))
            {
                VertexVisualization vv = FocusedFrameworkElement as VertexVisualization;

                List<EdgeVisualization> elist = EdgeVisualizations.Where(a => a.Edge.U.Equals(vv.Vertex)).ToList<EdgeVisualization>();
                //case Vertex is connected
                if (elist.Count < 0)
                {

                }
                else
                {
                    //case vertex isnt connected
                    this.Graph.Vertices.Remove(vv.Vertex);
                }


            }
        }

        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = FocusedFrameworkElement != null;
        }

        #endregion

        /// <summary>
        /// Will be executed when the Save command was called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XElement Xgraph = this.Graph.Save();

            //http://www.codeproject.com/Articles/24681/WPF-Diagram-Designer-Part-4
            XElement XFrameworkElement = new XElement("FrameworkElements",
                from item in this.Children.OfType<VertexVisualization>()
                //let Contentxaml = XamlWriter.Save(item.Conte)
                select new XElement(item.GetType().ToString(),
                    new XElement("ID", item.GetHashCode()),
                    new XElement("VertexID", item.Vertex.GetHashCode()),
                    //todo:  new XElement("Position", this.getPosition(item)), -> returns a string omiting the culture of the operating system
                    //use instead point.ToString(CultureInfo.InvariantCulture)
                    new XElement("Position", this.getPosition(item)),
                    new XElement(WidthProperty.Name, item.Width),
                    new XElement(HeightProperty.Name, item.Height),
                    new XElement("ZIndex", Canvas.GetZIndex((UIElement)item)))
                    );

            XElement root = new XElement("Root");
            root.Add(Xgraph);
            root.Add(XFrameworkElement);

            SaveFile(root);

        }
        /// <summary>
        /// Will be executed when the Load command was called
        /// </summary>
        /// <param name="sender">Object which raised Command</param>
        /// <param name="e">Provides event informations</param>
        protected void Load_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XElement root = LoadSerializedDataFromFile(e.Parameter != null ? e.Parameter.ToString() : String.Empty);

            this.Graph = null;
            if (root != null && root.HasElements)
            {
                if (root.Elements().Where(a => a.Name.LocalName.Equals("Graph")).Count().Equals(0)) return;
                if (root.Elements().Where(a => a.Name.LocalName.Equals("FrameworkElements")).Count().Equals(0)) return;

                //load graph
                MemoryStream memoryStream = new MemoryStream();

                XElement XGraph = root.Elements().Where(a => a.Name.LocalName.Equals("Graph")).FirstOrDefault<XElement>();
                XGraph.Save(memoryStream);
                memoryStream.Position = 0;
                NetDataContractSerializer ndcs = new NetDataContractSerializer();
                Get.Model.Graph.Graph g = ndcs.ReadObject(memoryStream) as Graph;

                this.Graph = g;

                //set positions of items
                root.Elements("FrameworkElements").First().Elements(typeof(VertexVisualization).FullName).ToList().ForEach(
                    y =>
                    {
                        XElement itemXML = y;
                        VertexVisualization vv = Children.OfType<VertexVisualization>().Where(a => a.Vertex.GetHashCode().ToString().Equals(itemXML.Element("VertexID").Value)).FirstOrDefault<VertexVisualization>();
                        //todo:  new XElement("Position", this.getPosition(item)), -> returns a string omiting the culture of the operating system
                        //use instead point.ToString(CultureInfo.InvariantCulture)
                        setPosition(vv, Point.Parse(itemXML.Element("Position").Value.Replace(',', '.').Replace(';', ',')));
                        Canvas.SetZIndex(vv, Int32.Parse(itemXML.Element("ZIndex").Value));
                    });

                //setFocus position of edges
                Children.OfType<EdgeVisualization>().ToList().ForEach(i =>
                {
                    VertexVisualization u = getItem(i.Edge.U);
                    Point pu = getPosition(u);
                    Point pv = getPosition(getItem(i.Edge.V));

                    i.PositionU = new Point(pu.X - u.Width / 2, pu.Y - u.Height / 2);
                    i.PositionV = new Point(pv.X - u.Width / 2, pv.Y - u.Height / 2);
                });

            }
            this.InvalidateVisual();

        }

        protected void SetDirected_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && sender.GetType().Equals(typeof(GraphVisualization)))
            {
                if (e.Parameter != null && e.Parameter.GetType().Equals(typeof(bool)))
                {
                    this.Graph.Directed = (bool)e.Parameter;
                }
                else
                {
                    this.Graph.Directed = !this.Graph.Directed;
                }
            }
        }
        /// <summary>
        /// Will be executed when the AddVertex command was called.
        /// </summary>
        /// <param name="sender">Object which raised the event</param>
        /// <param name="e">An ExecutedRoutedEventArgs that contains the event data. </param>
        protected void AddVertex_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && sender.GetType().Equals(typeof(GraphVisualization)))
            {
                GraphVisualization gv = sender as GraphVisualization;

                //todo wenn man beim graph vertex added soll das graph visualization control auto. selber den neuen vertex dazu tun
                Point p = (Mouse.GetPosition(sender as IInputElement));
                Vertex v = new Vertex();

                gv.Graph.addVertex(new Vertex());

            }
        }

        #endregion

        #region Helper Methods http://www.codeproject.com/Articles/24681/WPF-Diagram-Designer-Part-4#Save

        private void SaveFile(XElement xElement)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    xElement.Save(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private XElement LoadSerializedDataFromFile(string path)
        {
            if (!path.Equals(String.Empty))
            {
                return XElement.Load(path);
            }
            else
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Designer Files (*.xml)|*.xml|All Files (*.*)|*.*";

                if (openFile.ShowDialog() == true)
                {
                    try
                    {
                        return XElement.Load(openFile.FileName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                return null;
            }

        }

        #endregion
    }
}
