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

namespace Get.UI
{
    public partial class GraphVisualization : Canvas
    {
        #region Save Command

        public GraphVisualization()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open,Load_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphVisualization.AddVertex, AddVertex_Executed));
        }

        /// <summary>
        /// Will be executed when the Save command was called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            //XElement serializedItems = new XElement("DesignerItems",
            //               from item in designerItems
            //               let contentXaml = XamlWriter.Save(((DesignerItem)item).Content)
            //               select new XElement("DesignerItem",
            //                          new XElement("Left", Canvas.GetLeft(item)),
            //                          new XElement("Top", Canvas.GetTop(item)),
            //                          new XElement("Width", item.Width),
            //                          new XElement("Height", item.Height),
            //                          new XElement("ID", item.ID),
            //                          new XElement("zIndex", Canvas.GetZIndex(item)),
            //                          new XElement("IsGroup", item.IsGroup),
            //                          new XElement("ParentID", item.ParentID),
            //                          new XElement("Content", contentXaml)
            //                      )
            //           );

            //return serializedItems;

            XElement Xgraph = this.Graph.Save();

            //http://www.codeproject.com/Articles/24681/WPF-Diagram-Designer-Part-4
            XElement XFrameworkElement = new XElement("FrameworkElements",
                from item in this.Children.OfType<VertexVisualization>()
                //let Contentxaml = XamlWriter.Save(item.Conte)
                select new XElement(item.GetType().ToString(),
                    new XElement("ID",item.GetHashCode()),
                    new XElement("VertexID",item.Vertex.GetHashCode()),
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
        protected void Load_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XElement root = LoadSerializedDataFromFile();

            this.Graph=null;
            if(root != null && root.HasElements)
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

                //IEnumerable<XElement> itemsXML = root.Elements("FrameworkElements");
                //foreach (XElement itemXML in itemsXML)
                //{

                //}

            }
          

            //root.Elements(

            //IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");
            //foreach (XElement itemXML in itemsXML)
            //{
            //    Guid id = new Guid(itemXML.Element("ID").Value);
            //    DesignerItem item = DeserializeDesignerItem(itemXML, id, 0, 0);
            //    this.Children.Add(item);
            //    SetConnectorDecoratorTemplate(item);
            //}

            this.InvalidateVisual();

            //IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            //foreach (XElement connectionXML in connectionsXML)
            //{
            //    Guid sourceID = new Guid(connectionXML.Element("SourceID").Value);
            //    Guid sinkID = new Guid(connectionXML.Element("SinkID").Value);

            //    String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
            //    String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

            //    Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
            //    Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);

            //    Connection connection = new Connection(sourceConnector, sinkConnector);
            //    Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
            //    this.Children.Add(connection);
            //}
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

                gv.Graph.addVertec(new Vertex());
                addVertex(new Vertex());
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

        private XElement LoadSerializedDataFromFile()
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

        //private static DesignerItem DeserializeDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY)
        //{
        //    DesignerItem item = new DesignerItem(id);
        //    item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
        //    item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
        //    item.ParentID = new Guid(itemXML.Element("ParentID").Value);
        //    item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);
        //    Canvas.SetLeft(item, Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX);
        //    Canvas.SetTop(item, Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY);
        //    Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));
        //    Object content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
        //    item.Content = content;
        //    return item;
        //}

        #endregion
    }
}
