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

        public static readonly RoutedEvent DragDeltaEvent;

        static GraphVisualization()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphVisualization), new FrameworkPropertyMetadata(typeof(GraphVisualization)));
            DragDeltaEvent = EventManager.RegisterRoutedEvent("DragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(GraphVisualization));

        }
        public GraphVisualization()
            : base()
        {
            _VertexVisualizationList = new ObservableCollection<VertexVisualization>();

            Loaded += new RoutedEventHandler(GraphVisualization_Loaded);
            DragDelta += new RoutedEventHandler(GraphVisualization_DragDelta);
        }

        public event RoutedEventHandler DragDelta
        {
            add {AddHandler(Thumb.DragDeltaEvent,value);}
            remove{RemoveHandler(Thumb.DragDeltaEvent,value);}
        }



        void GraphVisualization_DragDelta(object sender, RoutedEventArgs e)
        {

        }

        void GraphVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseEdge();
        }
        private List<EdgeVisualization> elist = new List<EdgeVisualization>();
        protected virtual void InitialiseEdge()
        {
            //get all Vertices
            foreach (VertexVisualization u in VertexVisualizationList)
            {
                //get from the Vertex all edges
                foreach (Edge e in u.Vertex.Edges)
                {
                    //create the Edge control for the vertex u
                    EdgeVisualization edgeVisualization = new EdgeVisualization() { Edge = e };
                    //get the moveAbelItem for the position property of vertex u
                    MoveAbelItem moveAbelItemU = getMoveAbelItem(u);

                    //search and get the other proper Vertex
                    VertexVisualization v = VertexVisualizationList.Where(ue => ue.Vertex == e.V).First();
                    //get the moveAbelItem for the position property of vertex v
                    MoveAbelItem moveAbelItemV = getMoveAbelItem(v);

                    //set the binding for vertex u
                    Binding bindingU = new Binding("Position");
                    bindingU.Source = moveAbelItemU;
                    bindingU.Mode = BindingMode.OneWay;
                    bindingU.NotifyOnSourceUpdated = true;
                    bindingU.NotifyOnTargetUpdated = true;
                    bindingU.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    edgeVisualization.SetBinding(EdgeVisualization.PositionUProperty, bindingU);

                    Binding bindingV = new Binding("Position");
                    bindingV.Source = moveAbelItemV;
                    bindingV.Mode = BindingMode.OneWay;
                    bindingV.NotifyOnSourceUpdated = true;
                    bindingV.NotifyOnTargetUpdated = true;
                    bindingV.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    edgeVisualization.SetBinding(EdgeVisualization.PositionVProperty, bindingV);
                    Canvas.Children.Add(edgeVisualization);
                    elist.Add(edgeVisualization);
                }
            }
        }
        private MoveAbelItem getMoveAbelItem(VertexVisualization pVertexVisualization)
        {
            ContentControl c = pVertexVisualization.Parent as ContentControl;
            c.Template = Canvas.FindResource("DesignerItemTemplate") as ControlTemplate;

            return FindVisualChildren<MoveAbelItem>(c.Template.LoadContent()).First();
        }


        public Graph Graph
        {
            get { return (Graph)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.Template != null)
            {
                _Canvas = GraphVisualization.FindVisualChildren<Canvas>(this).First<Canvas>();
            }
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

        protected virtual void InitialiseGraph(IList<Vertex> vertices, EdgeVisualization e)
        {
            foreach (Vertex a in vertices)
            {
                VertexVisualization u = addVertex(a);
                if (e != null)
                {
                    //e.PositionV = getPositionFromVertexVisualization(u);
                    //setPositionFromVertexVisualizationBinding(u, e);
                }

                foreach (Edge ed in a.Edges)
                {
                    //EdgeVisualization edgeVisualization = new EdgeVisualization() { Edge = ed };
                    //edgeVisualization.PositionU = getPositionFromVertexVisualization(u);
                    //setPositionFromVertexVisualizationBinding(u, edgeVisualization);

                    InitialiseGraph(new List<Vertex>() { ed.V }, null);
                    //Canvas.Children.Add(edgeVisualization);
                }
            }

        }
        protected virtual VertexVisualization addVertex(Vertex v)
        {
            ContentControl c = new ContentControl();
            c.Template = Canvas.FindResource("DesignerItemTemplate") as ControlTemplate;

            VertexVisualization vertexcontrol = new VertexVisualization();
            vertexcontrol.Vertex = v;

            c.Content = vertexcontrol;


            _VertexVisualizationList.Add(vertexcontrol);
            Canvas.Children.Add(c);

            Canvas.SetLeft(c, GetRandomNumber(0, Canvas.ActualWidth - 10));
            Canvas.SetTop(c, GetRandomNumber(0, Canvas.ActualHeight - 10));

            return vertexcontrol;

        }

        protected virtual VertexVisualization getVertexVisualization(Vertex u)
        {
            return FindVisualChildren<VertexVisualization>(Canvas).Where(x => x.Vertex.Equals(u)).First();
        }

        protected virtual Point getPositionFromVertexVisualization(VertexVisualization u)
        {

            double left = Canvas.GetLeft(u.Parent as UIElement);
            double top = Canvas.GetTop(u.Parent as UIElement);
            return new Point(left + (u.Width / 2), top + (u.Height / 2));
        }

        protected virtual void setPositionFromVertexVisualizationBinding(VertexVisualization u, EdgeVisualization e)
        {
            ContentControl c = u.Parent as ContentControl;
            c.Template = Canvas.FindResource("DesignerItemTemplate") as ControlTemplate;

            MoveAbelItem moveAbelItem = FindVisualChildren<MoveAbelItem>(c.Template.LoadContent()).First();
            
            Binding bindingU = new Binding("Position");
            bindingU.Source = moveAbelItem;
            bindingU.Mode = BindingMode.OneWay;
            bindingU.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            e.SetBinding(EdgeVisualization.PositionUProperty, bindingU);

            Binding bindingy = new Binding("Position");
            bindingy.Source = moveAbelItem;
            bindingy.Mode = BindingMode.OneWay;
            bindingy.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            e.SetBinding(EdgeVisualization.PositionUProperty, bindingy);


        }

        private double GetRandomNumber(double minimum, double maximum)
        {
            return _Random.NextDouble() * (maximum - minimum) + minimum;
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
