using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

using Get.Model.Graph;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Graph", "Get.UI")]
namespace Get.UI
{
    /// <summary>
    /// Represents a control that enables a user to edit a Graph using a visual Graph display.
    /// http://en.wikipedia.org/wiki/Graph_(mathematics)
    /// 
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
        #region Members
        /// <summary>
        /// Represents a pseudo-random number generator, a device that produces a sequence of numbers that meet certain statistical requirements for randomness.
        /// http://msdn.microsoft.com/en-us/library/system.random.aspx?queryresult=true
        /// </summary>
        protected Random _Random = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// Canvas on which we place our VertexVisualization and EdgeVisualization. So the ControlTemplate of the GraphVisualization requires a Canvas which is named _Canvas.
        /// </summary>
        protected Canvas _Canvas = null;
        /// <summary>
        /// Represents a dynamic data collection of VertexVisualizations that provides notifications when items get added, removed, or when the whole list is refreshed.
        /// http://msdn.microsoft.com/en-us/library/ms668604.aspx?queryresult=true
        /// </summary>
        protected ObservableCollection<VertexVisualization> _VertexVisualizationList;
        /// <summary>
        /// Represents a dynamic data collection of EdgeVisualizations that provides notifications when items get added, removed, or when the whole list is refreshed.
        /// http://msdn.microsoft.com/en-us/library/ms668604.aspx?queryresult=true
        /// </summary>
        protected ObservableCollection<EdgeVisualization> _EdgeVisualizationList;
        #endregion

        #region Constructors
        static GraphVisualization()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphVisualization), new FrameworkPropertyMetadata(typeof(GraphVisualization)));
        }
        /// <summary>
        /// Initializes a new instance of the GraphVisualization class. 
        /// </summary>
        public GraphVisualization()
            : base()
        {
            _VertexVisualizationList = new ObservableCollection<VertexVisualization>();
            _EdgeVisualizationList = new ObservableCollection<EdgeVisualization>();
        }
        #endregion

        #region Overriden Methods
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call ApplyTemplate.
        /// http://msdn.microsoft.com/de-de/library/system.windows.frameworkelement.onapplytemplate.aspx#
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.Template != null)
            {
                _Canvas = this.Template.FindName("PART_DesignerCanvas", this) as Canvas;
                DragDelta += new RoutedEventHandler(GraphVisualization_DragDelta);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Occurs one or more times as the mouse changes position when a Thumb control btw. MoveAbelItem has logical focus and mouse capture. 
        /// The Thumb control receives focus and mouse capture when the user presses the left mouse button while pausing the mouse pointer over the Thumb control. 
        /// The Thumb btw. MoveAbelItem control loses mouse capture when the user releases the left mouse button, or when the CancelDrag method is called.
        /// A new DragDelta event occurs each time the mouse position moves on the screen.
        /// Therefore, this event can be raised multiple times without a limit when a Thumb control has mouse capture.
        /// http://msdn.microsoft.com/en-us/library/system.windows.controls.primitives.thumb.dragdeltaevent.aspx?queryresult=true
        /// </summary>
        /// <param name="sender">Object which raised Event. Keep in mind that this Event bubbled from the MoveAbelItem to the GraphVisualization</param>
        /// <param name="e">Contains state information and event data associated with a routed event. </param>
        protected virtual void GraphVisualization_DragDelta(object sender, RoutedEventArgs e)
        {
            DragDeltaEventArgs dragDeltaEventArgs = e as DragDeltaEventArgs;

            if (dragDeltaEventArgs.OriginalSource != null && dragDeltaEventArgs.OriginalSource.GetType().Equals(typeof(MoveAbelItem)))
            {
                //get MoveAbelItem which saves Position of Vertex
                MoveAbelItem moveAbelItem = dragDeltaEventArgs.OriginalSource as MoveAbelItem;
                //get the vertex which belongs to the MoveAbelItem
                DesignerItem contentc = GetFrameworkElementParent<DesignerItem>(moveAbelItem as FrameworkElement) as DesignerItem;
                if (contentc == null) return;
                VertexVisualization v = contentc.Content as VertexVisualization;
                //get all edges of the vertex
                var edgelist = _EdgeVisualizationList.Where(p => p.VertexVisualizationU.Vertex == v.Vertex || p.VertexVisualizationV.Vertex == v.Vertex).ToList<EdgeVisualization>();
                //set new position to the edges
                foreach (EdgeVisualization edge in edgelist)
                {
                    //determind which Position U / V should be changed
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
        /// <summary>
        /// Creates for each Vertex and Edge the proper control to display the Graph.
        /// </summary>
        /// <param name="vertices"></param>
        protected virtual void InitialiseGraph(IList<Vertex> vertices)
        {
            InitialiseGraph(vertices, null);
        }
        /// <summary>
        /// Creates for each Vertex a VertexVisualization and for each Edge a EdgeVisualization Control. 
        /// </summary>
        /// <param name="vertices">List of Vertices</param>
        /// <param name="e">The last added EdgeVisualization. EdgeVisualization.VertexVisualizationV will be set.</param>
        protected virtual void InitialiseGraph(IList<Vertex> vertices, EdgeVisualization e)
        {
            foreach (Vertex a in vertices)
            {
                VertexVisualization u;
                bool vertexexists = VertexVisualizationList.Where(h => h.Vertex.Equals(a)).Count().Equals(0) ? false : true;

                if (!vertexexists)
                {
                    u = addVertex(a);
                    Canvas.SetZIndex(u, 1);
                }
                else
                {
                    u = VertexVisualizationList.Where(g => g.Vertex.Equals(a)).First();
                }

                if (e != null)
                {
                    e.PositionV = getPositionFromVertexVisualization(u);
                    e.VertexVisualizationV = u;
                    Canvas.SetZIndex(e, -1);
                }

                if (vertexexists) return;


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
        /// <summary>
        /// Adds a Vertex to the _Canvas.
        /// Important to know is that the VertexVisualization will be encapsulated by a ContentControl which is using the DesignerItemTemplate ControlTemplate. 
        /// The DesignerItemTemplate contains the MoveAbelItem which enables dragging on the "VertexVisualization" control.
        /// The position of VertexVisualization will be set randomly on the canvas.
        /// </summary>
        /// <param name="v">Vertex which should be added to the VertexVisualization</param>
        /// <returns>Returns the created VertexVisualization</returns>
        protected virtual VertexVisualization addVertex(Vertex v)
        {
            return addVertex(v, new Point(GetRandomNumber(0, Canvas.ActualWidth - 10), GetRandomNumber(0, Canvas.ActualHeight - 10)));
        }
        /// <summary>
        /// Adds a Vertex to the _Canvas.
        /// Important to know is that the VertexVisualization will be encapsulated by a ContentControl which is using the DesignerItemTemplate ControlTemplate. 
        /// The DesignerItemTemplate contains the MoveAbelItem which enables dragging on the "VertexVisualization" control.
        /// </summary>
        /// <param name="v">Vertex which should be added to the VertexVisualization</param>
        /// <param name="point">Sets the position where the VertexVisualization should be placed on the _Canvas</param>
        /// <returns>Returns the created VertexVisualization</returns>
        protected virtual VertexVisualization addVertex(Vertex v, Point point)
        {
            //Create a ContentControl
            DesignerItem c = new DesignerItem();
            //Add the DesignerItemTemplate ControlTemplate that contains the MoveAbelItem
            //c.Template = FindResource("DesignerItemTemplate") as ControlTemplate;
            //create the VertexVisualization control
            VertexVisualization vertexcontrol = new VertexVisualization();
            vertexcontrol.Vertex = v;
            //add the VertexVisualization to the contentcontrol
            c.Content = vertexcontrol;

            _VertexVisualizationList.Add(vertexcontrol);
            Canvas.Children.Add(c);
            
            Canvas.SetLeft(c, point.X);
            Canvas.SetTop(c, point.Y);

            return vertexcontrol;
        }
        /// <summary>
        /// Returns the position of the delivered VertexVisualization control
        /// </summary>
        /// <param name="u">From which VertexVisualization the position should be returned</param>
        /// <returns>Position of the VertexVisualization control</returns>
        protected virtual Point getPositionFromVertexVisualization(VertexVisualization u)
        {
            double left = Canvas.GetLeft(u.Parent as UIElement);
            double top = Canvas.GetTop(u.Parent as UIElement);
            return new Point(left + (u.Width / 2), top + (u.Height / 2));
        }

        public virtual void Save()
        {

        }
        /// <summary>
        /// Generates a random number with the parameter minimum and maximum
        /// </summary>
        /// <param name="minimum">Minimum random value</param>
        /// <param name="maximum">Maximum random value</param>
        /// <returns>Randoum number between minimum and maximum</returns>
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

        private static void OnGraphChanged(DependencyObject pDependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null || e.NewValue.GetType() != (typeof(Graph))) return;
            if (pDependencyObject != null && pDependencyObject.GetType().Equals(typeof(GraphVisualization)))
            {
                GraphVisualization graphVisualization = pDependencyObject as GraphVisualization;
                Graph graph = e.NewValue as Graph;

                graphVisualization.InitialiseGraph(graph.Vertices);

                foreach (VertexVisualization v in graphVisualization.VertexVisualizationList)
                    Debug.WriteLine(Canvas.GetZIndex(v));

                foreach (EdgeVisualization v in graphVisualization.EdgeVisualizationList)
                    Debug.WriteLine(Canvas.GetZIndex(v));

            }
        }

        #endregion

        /// <summary>
        /// Represents the method that will handle various routed events that do not have specific event data beyond the data that is common for all routed events. 
        /// http://msdn.microsoft.com/en-us/library/system.windows.routedeventhandler.aspx?queryresult=true
        /// </summary>
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



        #region Properties
        /// <summary>
        /// Contains all VertexVisualization which are displayed on the Graph
        /// </summary>
        public virtual ObservableCollection<VertexVisualization> VertexVisualizationList
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
        /// <summary>
        /// Contains all EdgeVisualization which are displayed on the Graph
        /// </summary>
        public virtual ObservableCollection<EdgeVisualization> EdgeVisualizationList
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
        /// <summary>
        /// Canvas of the GraphVisualization ControlTemplate where are the VertexVisualization and EdgeVisualization will be displayed
        /// </summary>
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
        #endregion

    }
}
