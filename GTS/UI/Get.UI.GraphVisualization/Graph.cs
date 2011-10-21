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
using System.Windows.Markup;


namespace Get.UI
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Get.UI.GraphVisualization"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Get.UI.GraphVisualization;assembly=Get.UI.GraphVisualization"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    [ContentProperty("Graph")]
    public class GraphVisualization : Control
    {
        Canvas _Canvas = null;

        public GraphVisualization()
        {
            Loaded += new RoutedEventHandler(GraphVisualization_Loaded);
            //TODO: loaded weg tun und stattdessen event suchen, das ausgelöst wird, wenn das controltemplate geladen wurde!
        }

        private void GraphVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            _Canvas = GraphVisualization.FindVisualChildren<Canvas>(this).First<Canvas>();
        }

        static GraphVisualization()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphVisualization), new FrameworkPropertyMetadata(typeof(GraphVisualization)));
        }

        public Graph Graph
        {
            get { return (Graph)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Graph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Graph", typeof(Graph), typeof(GraphVisualization), new UIPropertyMetadata(null, OnGraphChanged));

        private static void OnGraphChanged(DependencyObject pDependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null || e.NewValue.GetType() != (typeof(Graph))) return;
            if (pDependencyObject != null && pDependencyObject.GetType().Equals(typeof(GraphVisualization)))
            {
                GraphVisualization graphVisualization = pDependencyObject as GraphVisualization;
                Graph graph = e.NewValue as Graph;

                foreach (Vertex a in graph.Vertices)
                {
                    graphVisualization.addVertex(a);
                    foreach (Edge ed in a.Edges)
                    {
                        graphVisualization.addVertex(ed.V);
                    }
                }

            }
        }

        protected virtual void addVertex(Vertex v)
        {
            if (Canvas == null) _Canvas = GraphVisualization.FindVisualChildren<Canvas>(this).First<Canvas>();

            VertexVisualization vertexcontrol = new VertexVisualization();
            vertexcontrol.Vertex = v;
            Canvas.Children.Add(vertexcontrol);
            Canvas.SetLeft(vertexcontrol, GetRandomNumber(DateTime.Now.Millisecond,0, Canvas.ActualWidth));
            Canvas.SetTop(vertexcontrol, GetRandomNumber(DateTime.Now.Millisecond,0, Canvas.ActualHeight));
            
        }

        private double GetRandomNumber(int seed, double minimum, double maximum)
        {
            Random random = new Random(seed);
            return random.NextDouble() * (maximum - minimum) + minimum;
        }


        protected virtual Canvas Canvas
        {
            get
            {
                return _Canvas;
            }
            set
            {
                value = _Canvas;
            }
        }

        /// <summary>
        /// 
        /// http://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


    }
}
