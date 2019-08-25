using System;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Collections.Specialized;

namespace DataStructures.UI
{
    public partial class GraphControl : Canvas
    {
        #region Save Command

        public GraphControl() : base()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Load_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphControl.AddVertexRoutedCommand, AddVertex_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Print_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphControl.SetDirectedRoutedCommand, SetDirected_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphControl.KruskalRoutedCommand, Kruskal_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphControl.ClearGraphCommand, ClearGraph_Executed));

        }
        private void ClearGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && sender.GetType().Equals(typeof(GraphControl)))
            {
                this.Graph = null;
                this.Graph = new Graph();
            }

        }
        private void Kruskal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && sender.GetType().Equals(typeof(GraphControl)))
            {
                Graph kruskal = this.Graph.Kruskal_DepthFirstSearch();
                this.Graph = null;
                this.Graph = kruskal;
            }

        }

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && sender.GetType().Equals(typeof(GraphControl)))
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(sender as GraphControl, "GraphVisualization");
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
            if (FocusedFrameworkElement.GetType().Equals(typeof(VertexControl)))
            {
                VertexControl vv = FocusedFrameworkElement as VertexControl;

                Vertex<object> newVertex = new Vertex<object>();

                newVertex.Weighted = vv.Vertex.Weighted;

                this.Graph.AddVertex(newVertex);

            }
        }

        #region Delete Command

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (FocusedFrameworkElement != null && FocusedFrameworkElement.GetType().Equals(typeof(VertexControl)))
            {
                VertexControl vv = FocusedFrameworkElement as VertexControl;

                //case Vertex is connected
                if (vv.Vertex.Edges.Count.Equals(0))
                {
                    this.Graph.Vertices.Remove(vv.Vertex);
                }

            }
            if (FocusedFrameworkElement != null && FocusedFrameworkElement.GetType().Equals(typeof(EdgeControl)))
            {
                EdgeControl ev = FocusedFrameworkElement as EdgeControl;

                IEdge edge = ev.Edge;

                IVertex u = edge.U;
                IVertex v = edge.V;

                u.RemoveEdge(v, this.Graph.Directed);

                //the vertex is unconnected so move it to the unconnected vertices list
                if (u.Edges.Count.Equals(0))
                {
                    this.Graph.Vertices.CollectionChanged -= new NotifyCollectionChangedEventHandler(CollectionChanged);
                    this.Graph.Vertices.Add(u);
                    this.Graph.Vertices.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
                    u = null;
                }
                if (v.Edges.Count.Equals(0))
                {
                    this.Graph.Vertices.CollectionChanged -= new NotifyCollectionChangedEventHandler(CollectionChanged);
                    this.Graph.Vertices.Add(v);
                    this.Graph.Vertices.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
                    v = null;
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
                from item in this.Children.OfType<VertexControl>()
                    //let Contentxaml = XamlWriter.Save(item.Conte)
                select new XElement(item.GetType().ToString(),
                    new XElement("ID", item.GetHashCode()),
                    new XElement("VertexID", item.Vertex.GetHashCode()),
                    //todo:  new XElement("Position", this.getPosition(item)), -> returns a string omiting the culture of the operating system
                    //use instead point.ToString(CultureInfo.InvariantCulture)
                    new XElement("Position", this.GetPosition(item)),
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
            try
            {
                XElement root = LoadSerializedDataFromFile(e.Parameter != null ? e.Parameter.ToString() : String.Empty);

                this.Graph = null;
                if (root != null && root.HasElements)
                {
                    if (!root.Elements().Any(a => a.Name.LocalName.Equals("Graph"))) return;
                    if (!root.Elements().Any(a => a.Name.LocalName.Equals("FrameworkElements"))) return;

                    //load graph
                    XElement XGraph = root.Elements().FirstOrDefault(a => a.Name.LocalName.Equals("Graph"));

                    this.Graph = XGraph.Load();
                    //set positions of items
                    var framework = root.Elements("FrameworkElements").First().Elements(typeof(VertexControl).FullName).ToList();
                    foreach (var y in framework)
                    {
                        XElement itemXML = y;
                        VertexControl vv = Children.OfType<VertexControl>().Where(a => a.Vertex.GetHashCode().ToString().Equals(itemXML.Element("VertexID").Value)).FirstOrDefault<VertexControl>();
                        //todo:  new XElement("Position", this.getPosition(item)), -> returns a string omiting the culture of the operating system
                        //use instead point.ToString(CultureInfo.InvariantCulture)
                        if (vv != null)
                        {

                            string position = itemXML.Element("Position").Value;
                            position = position.Replace(';', ',');
                            SetPosition(vv, Point.Parse(position));
                            Canvas.SetZIndex(vv, Int32.Parse(itemXML.Element("ZIndex").Value));
                        }
                    }

                    //setFocus position of edges
                    Children.OfType<EdgeControl>().ToList().ForEach(i =>
                    {
                        VertexControl u = GetItem(i.Edge.U);
                        Point pu = GetPosition(u);
                        Point pv = GetPosition(GetItem(i.Edge.V));

                        i.PositionU = new Point(pu.X - u.Width / 2, pu.Y - u.Height / 2);
                        i.PositionV = new Point(pv.X - u.Width / 2, pv.Y - u.Height / 2);
                    });

                }
                this.InvalidateVisual();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                ClearGraph_Executed(this, null);
            }
        }

        protected void SetDirected_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender != null && sender.GetType().Equals(typeof(GraphControl)))
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
            if (sender != null && sender.GetType().Equals(typeof(GraphControl)))
            {
                GraphControl gv = sender as GraphControl;

                //todo wenn man beim graph vertex added soll das graph visualization control auto. selber den neuen vertex dazu tun
                Point p = (Mouse.GetPosition(sender as IInputElement));
                Vertex<object> v = new Vertex<object>();

                gv.Graph.AddVertex(new Vertex<object>());

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
