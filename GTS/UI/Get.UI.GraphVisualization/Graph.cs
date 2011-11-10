using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Get.Model.Graph;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml.Serialization;


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
        protected Random _Random = new Random(DateTime.Now.Millisecond);
        protected Canvas _Canvas = null;
        protected ObservableCollection<VertexVisualization> _VertexVisualizationList;
        protected ObservableCollection<EdgeVisualization> _EdgeVisualizationList;

        static GraphVisualization()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphVisualization), new FrameworkPropertyMetadata(typeof(GraphVisualization)));

        }
        public GraphVisualization()
            : base()
        {
            _VertexVisualizationList = new ObservableCollection<VertexVisualization>();
            _EdgeVisualizationList = new ObservableCollection<EdgeVisualization>();

            DragDelta += new RoutedEventHandler(GraphVisualization_DragDelta);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.Template != null)
            {
                _Canvas = GraphVisualization.FindVisualChildren<Canvas>(this).First<Canvas>();
            }
        }

        protected virtual void GraphVisualization_DragDelta(object sender, RoutedEventArgs e)
        {
            DragDeltaEventArgs dragDeltaEventArgs = e as DragDeltaEventArgs;

            if (dragDeltaEventArgs.OriginalSource != null && dragDeltaEventArgs.OriginalSource.GetType().Equals(typeof(MoveAbelItem)))
            {
                MoveAbelItem moveAbelItem = dragDeltaEventArgs.OriginalSource as MoveAbelItem;
                ContentControl contentc = GetFrameworkElementParent<ContentControl>(moveAbelItem as FrameworkElement) as ContentControl;
                if (contentc == null) return;
                VertexVisualization v = contentc.Content as VertexVisualization;

                var edgelist = _EdgeVisualizationList.Where(p => p.VertexVisualizationU.Vertex == v.Vertex || p.VertexVisualizationV.Vertex == v.Vertex).ToList<EdgeVisualization>();

                foreach (EdgeVisualization edge in edgelist)
                {
                    if (edge.VertexVisualizationU.Vertex != v.Vertex)
                    {
                        edge.PositionV = new Point(moveAbelItem.Position.X + v.ActualWidth / 2, moveAbelItem.Position.Y + v.ActualHeight / 2);
                    }
                    else
                    {
                        edge.PositionU = new Point(moveAbelItem.Position.X + v.ActualWidth / 2, moveAbelItem.Position.Y + v.ActualHeight / 2);
                    }
                }

            }

        }

        protected virtual void InitialiseGraph(IList<Vertex> vertices, EdgeVisualization e)
        {
            foreach (Vertex a in vertices)
            {
                VertexVisualization u = addVertex(a);
                if (e != null)
                {
                    e.PositionV = getPositionFromVertexVisualization(u);
                    e.VertexVisualizationV = u;
                }

                foreach (Edge ed in a.Edges)
                {
                    EdgeVisualization edgeVisualization = new EdgeVisualization() { Edge = ed };
                    edgeVisualization.PositionU = getPositionFromVertexVisualization(u);
                    edgeVisualization.VertexVisualizationU = u;

                    InitialiseGraph(new List<Vertex>() { ed.V }, edgeVisualization);
                    _EdgeVisualizationList.Add(edgeVisualization);
                    Canvas.Children.Add(edgeVisualization);
                }
            }
        }
        protected virtual VertexVisualization addVertex(Vertex v)
        {
            ContentControl c = new ContentControl();
            c.Template = Canvas.FindResource("DesignerItemTemplate") as ControlTemplate;

            VertexVisualization vertexcontrol = new VertexVisualization();
            vertexcontrol.Vertex = v;
            //vertexcontrol.Background = Brushes.Green;

            c.Content = vertexcontrol;

            _VertexVisualizationList.Add(vertexcontrol);
            Canvas.Children.Add(c);

            Canvas.SetLeft(c, GetRandomNumber(0, Canvas.ActualWidth - 10));
            Canvas.SetTop(c, GetRandomNumber(0, Canvas.ActualHeight - 10));

            return vertexcontrol;

        }

        protected virtual Point getPositionFromVertexVisualization(VertexVisualization u)
        {
            double left = Canvas.GetLeft(u.Parent as UIElement);
            double top = Canvas.GetTop(u.Parent as UIElement);
            return new Point(left + (u.Width / 2), top + (u.Height / 2));
        }

        public virtual void Save()
        {

        }

        private double GetRandomNumber(double minimum, double maximum)
        {
            return _Random.NextDouble() * (maximum - minimum) + minimum;
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

        public static FrameworkElement GetFrameworkElementParent<T>(FrameworkElement element)
        {
            FrameworkElement parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
            if (parent != null)
            {
                if (parent.GetType().Equals(typeof(T)))
                {
                    return parent;
                }
                parent = GetFrameworkElementParent<T>(parent);
            }
            return parent;

        }
        public static FrameworkElement GetFrameworkElementParent(FrameworkElement element, string name)
        {
            FrameworkElement parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
            if (parent != null)
            {
                if (parent.Name == name)
                {
                    return parent;
                }
                return GetFrameworkElementParent(parent, name);
            }
            return null;

        }


        public event RoutedEventHandler DragDelta
        {
            add { AddHandler(Thumb.DragDeltaEvent, value); }
            remove { RemoveHandler(Thumb.DragDeltaEvent, value); }
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
                //TODO: Vertex die miteinander verbunden sind sollen in der nähe platziert werden!
                graphVisualization.InitialiseGraph(graph.Vertices, null);

            }
        }

        protected virtual ObservableCollection<VertexVisualization> VertexVisualizationList
        {
            get
            {
                return _VertexVisualizationList;
            }
            set
            {
                value = _VertexVisualizationList;
            }

        }
        protected virtual ObservableCollection<EdgeVisualization> EdgeVisualizationList
        {
            get
            {
                return _EdgeVisualizationList;
            }
            set
            {
                value = _EdgeVisualizationList;
            }

        }
        protected virtual Canvas Canvas
        {
            get
            {
                if (_Canvas == null) _Canvas = GraphVisualization.FindVisualChildren<Canvas>(this).First<Canvas>();
                return _Canvas;
            }
            set
            {
                value = _Canvas;
            }
        }


    }
}
