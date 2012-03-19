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

namespace Get.UI
{
    public partial class GraphVisualization : Canvas
    {
        #region Save Command

        public GraphVisualization()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            this.CommandBindings.Add(new CommandBinding(GraphVisualization.AddVertex, AddVertex_Executed));
        }

        /// <summary>
        /// Will be executed when the Save command was called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //http://www.codeproject.com/Articles/24681/WPF-Diagram-Designer-Part-4
            XElement items = new XElement("Items",
                from item in this.Children.OfType<FrameworkElement>()
                let Contentxaml = XamlWriter.Save(item)
                select new XElement("xelement", new XElement("Position", this.getPosition(item))));

            XElement root = new XElement("Root");
            root.Add(items);

            SaveFile(root);

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

        #region Helper Methods

        //protected virtual IEnumerable getType(Type t)
        //{
        //    return Children.OfType<t>().Where(a => a.GetType().Equals(t));
        //}

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

        #endregion
    }
}
