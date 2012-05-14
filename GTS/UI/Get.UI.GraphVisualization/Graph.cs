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
using System.Windows.Markup;
using Get.Model.Graph;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Xml.Linq;

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
    public partial class GraphVisualization : Canvas
    {
        #region Members
        /// <summary>
        /// Represents a pseudo-random number generator, a device that produces a sequence of numbers that meet certain statistical requirements for randomness.
        /// http://msdn.microsoft.com/en-us/library/system.random.aspx?queryresult=true
        /// </summary>
        protected Random _Random = new Random(DateTime.Now.Millisecond);

        public static RoutedCommand AddVertex = new RoutedCommand();

        public static readonly RoutedEvent MouseDoubleClickEvent = EventManager.RegisterRoutedEvent(
        "MouseDoubleClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GraphVisualization));

        #endregion

        static GraphVisualization()
        {
            //set the backgroundcolor 
            BackgroundProperty.OverrideMetadata(typeof(GraphVisualization), new FrameworkPropertyMetadata(Brushes.Transparent));
        }
        /// <summary>
        /// Called before the MouseLeftButtonDown event occurs.
        /// Preprares the drag and drop of the Vertex item. Raises the MouseDoubleClickEvent if the click counts equal two.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.Focus();

            if (e.ClickCount.Equals(2) && VisualTreeHelper.HitTest(this, e.GetPosition(this)).VisualHit.Equals(this))
            {
                RaiseMouseDoubleClickEvent();
            }

            if (e.Source != null && e.Source.GetType().Equals(typeof(VertexVisualization)) && e.LeftButton.Equals(MouseButtonState.Pressed) && SelectedItem == null && !e.OriginalSource.GetType().Equals(typeof(AdornerItem)))
            {
                this.SelectedItem = (FrameworkElement)e.Source;
                this.SelectedItem.Focus();
                Point p = new Point((e.GetPosition(this).X - SelectedItem.ActualWidth / 2), (e.GetPosition(this).Y - SelectedItem.ActualHeight / 2));
                setPosition(SelectedItem, p);
            }

            if (e.Source != null && e.Source.GetType().Equals(typeof(VertexVisualization)) && e.LeftButton.Equals(MouseButtonState.Pressed) && SelectedItem == null && e.OriginalSource.GetType().Equals(typeof(AdornerItem)))
            {
                //create a temporary edge
                EdgeVisualization ev = new EdgeVisualization();
                Canvas.SetZIndex(ev, -1);
                VertexVisualization vv = e.Source as VertexVisualization;
                ev.Edge = new Edge(vv.Vertex, null);

                ev.PositionU = getPosition(vv);
                ev.PositionV = e.GetPosition(this);
                this.SelectedItem = ev;
                this.Children.Add(ev);
            }
        }
        /// <summary>
        /// Called before the MouseMove event occurs. Moves the Vertex item.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton.Equals(MouseButtonState.Pressed) && SelectedItem != null && SelectedItem.GetType().Equals(typeof(VertexVisualization)) && !e.OriginalSource.GetType().Equals(typeof(AdornerItem)))
            {
                VertexVisualization v = SelectedItem as VertexVisualization;
                Point p = new Point((e.GetPosition(this).X - SelectedItem.ActualWidth / 2), (e.GetPosition(this).Y - SelectedItem.ActualHeight / 2));
                v.Position = p;
                setPosition(SelectedItem, p);
            }

            //move edge
            if (e.LeftButton.Equals(MouseButtonState.Pressed) && SelectedItem != null && SelectedItem.GetType().Equals(typeof(EdgeVisualization)))
            {
                EdgeVisualization ev = SelectedItem as EdgeVisualization;
                Point p = new Point((e.GetPosition(this).X-4), (e.GetPosition(this).Y)-4);
                ev.PositionV = p;
            }

        }
        /// <summary>
        /// Called before the MouseLeftButtonUp event occurs. Exits the drag and drop operation of the vertex item.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (e.Source != null && e.LeftButton.Equals(MouseButtonState.Released) && SelectedItem != null && SelectedItem.GetType().Equals(typeof(VertexVisualization)) && !e.OriginalSource.GetType().Equals(typeof(AdornerItem)))
            {
                VertexVisualization v = SelectedItem as VertexVisualization;
                Point p = new Point((e.GetPosition(this).X - SelectedItem.ActualWidth / 2), (e.GetPosition(this).Y - SelectedItem.ActualHeight / 2));
                v.Position = p;
                setPosition(SelectedItem, p);
                SelectedItem = null;
            }
            //add edge to graph or remove temporary edge
            if (e.Source != null && e.LeftButton.Equals(MouseButtonState.Released) && SelectedItem != null && SelectedItem.GetType().Equals(typeof(EdgeVisualization)))
            {
                
                HitTestResult result = VisualTreeHelper.HitTest(this, e.GetPosition(this));
                FrameworkElement u = result.VisualHit as FrameworkElement;
                VertexVisualization vv = u.TemplatedParent as VertexVisualization;

                EdgeVisualization ev = SelectedItem as EdgeVisualization;

                if (vv != null)
                {
                    //first find the vertex where we started 
                    Vertex v = VertexVisualizations.Where(z => z.Vertex.Equals(ev.Edge.U)).First().Vertex;
                    //add to model edge 
                    v.addEdge(vv.Vertex);
                }

                this.Children.Remove(SelectedItem);
                SelectedItem = null;
            }
        }
        protected FrameworkElement SelectedItem { get; set; }


        /// <summary>
        /// Measures the size of the current Canvas for the layout.
        /// http://msdn.microsoft.com/en-us/library/hh401019.aspx?queryresult=true
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            foreach (UIElement element in Children)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }

            // add some extra margin
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        /// <summary>
        /// Creates for each Vertex and Edge the proper control to display the Graph.
        /// </summary>
        /// <param name="vertices"></param>
        protected virtual void InitialiseGraph(ObservableCollection<Vertex> vertices)
        {
            InitialiseGraph(vertices, null);
        }
        /// <summary>
        /// Creates for each Vertex a VertexVisualization and for each Edge a EdgeVisualization Control. 
        /// </summary>
        /// <param name="vertices">List of Vertices</param>
        /// <param name="e">The last added EdgeVisualization. EdgeVisualization.VertexVisualizationV will be set.</param>
        protected virtual void InitialiseGraph(ObservableCollection<Vertex> vertices, EdgeVisualization e)
        {
            foreach (Vertex a in vertices)
            {
                a.Edges.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

                VertexVisualization u;
                bool vertexexists = getItem(a)==null ? false : true;

                if (!vertexexists)
                {
                    
                    u = addVertex(a);
                    Canvas.SetZIndex(u, 1);
                }
                else
                {
                    u = VertexVisualizations.Where(g => g.Vertex.Equals(a)).First();
                }

                if (e != null)
                {
                    e.PositionV = getPosition(u);

                    Binding binding = new Binding("Position");
                    binding.Source = u;
                    binding.Mode = BindingMode.TwoWay;
                    binding.NotifyOnSourceUpdated = true;
                    binding.NotifyOnTargetUpdated = true;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    binding.Converter = Converters.PointAdderConverter;
                    binding.ConverterParameter = new Point(u.Width / 2, u.Height / 2);
                    e.SetBinding(EdgeVisualization.PositionVProperty, binding);

                    Canvas.SetZIndex(e, -1);
                }

                if (vertexexists) return;

                foreach (Edge ed in a.Edges)
                {
                    EdgeVisualization edv = new EdgeVisualization() { Edge = ed };
                    edv.PositionU = getPosition(u);

                    ed.V.Edges.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

                    Binding binding = new Binding("Position");
                    binding.Source = u;
                    binding.Mode = BindingMode.TwoWay;
                    binding.NotifyOnSourceUpdated = true;
                    binding.NotifyOnTargetUpdated = true;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    binding.Converter = Converters.PointAdderConverter;
                    binding.ConverterParameter = new Point(u.Width / 2, u.Height / 2);
                    edv.SetBinding(EdgeVisualization.PositionUProperty, binding);

                    InitialiseGraph(new ObservableCollection<Vertex>() { ed.V }, edv);

                    this.Children.Add(edv);
                }
            }
        }

        protected void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
            if(e.Action.Equals(NotifyCollectionChangedAction.Add))
            {
                IList<object> items = e.NewItems.SyncRoot as IList<object>;
                object item = items.First();

                if (item.GetType().Equals(typeof(Edge)))
                {
                    Edge edge = item as Edge;
                    addEdge(edge);

                    //because vertex is duplicated remove from root
                    if (this.Graph.Vertices.Contains(edge.V) && this.Graph.Vertices.Count>1)
                    {
                        this.Graph.Vertices.CollectionChanged -= new NotifyCollectionChangedEventHandler(CollectionChanged);
                        this.Graph.Vertices.Remove(edge.V);
                        this.Graph.Vertices.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
                    }
                }
                if (item.GetType().Equals(typeof(Vertex)))
                {
                    addVertex(item as Vertex,Mouse.GetPosition(this).Add(-25,-25));
                }
            }
            if (e.Action.Equals(NotifyCollectionChangedAction.Remove))
            {
                IList<object> items = e.OldItems.SyncRoot as IList<object>;
                object item = items.First();
                if (item.GetType().Equals(typeof(Vertex)))
                {
                    //catch Visualization item
                    Vertex v = item as Vertex;
                    VertexVisualization vv= VertexVisualizations.Where(a => a.Vertex.Equals(v)).First();
                    this.Children.Remove(vv);
                }
            }
                Debug.WriteLine(this.Graph.Vertices.Count);
        }

        private static void OnGraphChanged(DependencyObject pDependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null && pDependencyObject != null && pDependencyObject.GetType().Equals(typeof(GraphVisualization)))
            {
                GraphVisualization graphVisualization = pDependencyObject as GraphVisualization;
                graphVisualization.Children.Clear();
                return;
            }
            if(e.NewValue.GetType() != (typeof(Graph))) return;
            if (pDependencyObject != null && pDependencyObject.GetType().Equals(typeof(GraphVisualization)))
            {
                GraphVisualization graphVisualization = pDependencyObject as GraphVisualization;
                Graph graph = e.NewValue as Graph;

                graphVisualization.Graph.Vertices.CollectionChanged += new NotifyCollectionChangedEventHandler(graphVisualization.CollectionChanged);

                graphVisualization.InitialiseGraph(graph.Vertices);
            }
        }
        /// <summary>
        /// Adds a Vertex to the _Canvas.
        /// The position of VertexVisualization will be set randomly on the canvas.
        /// </summary>
        /// <param name="v">Vertex which should be added to the VertexVisualization</param>
        /// <returns>Returns the created VertexVisualization</returns>
        protected virtual VertexVisualization addVertex(Vertex v)
        {
            return addVertex(v, new Point(GetRandomNumber(0, this.ActualWidth - 10), GetRandomNumber(0, this.ActualHeight - 10)));
        }
        /// <summary>
        /// Adds a Vertex to the _Canvas.
        /// </summary>
        /// <param name="v">Vertex which should be added to the VertexVisualization</param>
        /// <param name="point">Sets the position where the VertexVisualization should be placed on the _Canvas</param>
        /// <returns>Returns the created VertexVisualization</returns>
        protected virtual VertexVisualization addVertex(Vertex v, Point point)
        {
            VertexVisualization vertexcontrol = new VertexVisualization();
            vertexcontrol.Vertex = v;
            
            vertexcontrol.Position = point;
            SetLeft(vertexcontrol, point.X);
            SetTop(vertexcontrol, point.Y);

            this.Children.Add(vertexcontrol);
            v.Edges.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
            return vertexcontrol;
        }

        void Co(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        protected virtual EdgeVisualization addEdge(Edge e)
        {
            EdgeVisualization edv = new EdgeVisualization() { Edge = e };
            VertexVisualization uv = VertexVisualizations.Where(z => z.Vertex.Equals(e.U)).First();
            edv.PositionU = getPosition(uv);


            Binding bindingU = new Binding("Position");
            bindingU.Source = uv;
            bindingU.Mode = BindingMode.TwoWay;
            bindingU.NotifyOnSourceUpdated = true;
            bindingU.NotifyOnTargetUpdated = true;
            bindingU.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            bindingU.Converter = Converters.PointAdderConverter;
            bindingU.ConverterParameter = new Point(uv.Width / 2, uv.Height / 2);
            edv.SetBinding(EdgeVisualization.PositionUProperty, bindingU);

            this.Children.Add(edv);

            VertexVisualization vv = VertexVisualizations.Where(z => z.Vertex.Equals(e.V)).First();

            edv.PositionV = getPosition(vv);

            Binding bindingV = new Binding("Position");
            bindingV.Source = vv;
            bindingV.Mode = BindingMode.TwoWay;
            bindingV.NotifyOnSourceUpdated = true;
            bindingV.NotifyOnTargetUpdated = true;
            bindingV.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            bindingV.Converter = Converters.PointAdderConverter;
            bindingV.ConverterParameter = new Point(vv.Width / 2, vv.Height / 2);
            edv.SetBinding(EdgeVisualization.PositionVProperty, bindingV);

            Canvas.SetZIndex(edv, -1);

            return edv;

        }
        /// <summary>
        /// Returns the position of the delivered VertexVisualization control
        /// </summary>
        /// <param name="u">From which VertexVisualization the position should be returned</param>
        /// <returns>Position of the VertexVisualization control</returns>
        protected virtual Point getPosition(FrameworkElement u)
        {
            double left = Canvas.GetLeft(u as UIElement);
            double top = Canvas.GetTop(u as UIElement);
            return new Point(left + (u.Width / 2), top + (u.Height / 2));
        }
        protected virtual void setPosition(FrameworkElement f, Point p)
        {
            Canvas.SetLeft(f, p.X);
            Canvas.SetTop(f, p.Y);
        }
        /// <summary>
        /// Searches for the overgiven vertex and returns the vertexVisualization control which is representing it
        /// </summary>
        /// <param name="v">Vertex which should be looked up</param>
        /// <returns>Control which is using the Vertex</returns>
        protected virtual VertexVisualization getItem(Vertex v)
        {
            return VertexVisualizations.Where(a => a.Vertex.Equals(v)).FirstOrDefault<VertexVisualization>();
        }
        /// <summary>
        /// Searches for the overgiven edge and returns the EdgeVisualization control which is representing it
        /// </summary>
        /// <param name="v">edge which should be looked up</param>
        /// <returns>Control which is using the edge</returns>
        protected virtual EdgeVisualization getItem(Edge e)
        {
            return EdgeVisualizations.Where(a => a.Edge.Equals(e)).FirstOrDefault<EdgeVisualization>();
        }
        /// <summary>
        /// Calls the focus method on the VertexVisualization control
        /// </summary>
        /// <param name="v"></param>
        public virtual void setFocus(Vertex v)
        {
            if (getItem(v) != null)
                getItem(v).Focus();
        }
        /// <summary>
        /// Calls the focus method on the VertexVisualization control
        /// </summary>
        /// <param name="v"></param>
        public virtual void setFocus(Edge e)
        {
            if (getItem(e) != null)
                getItem(e).Focus();
        }
        public virtual void setFocus(Edge e, AsyncCallback b)
        {
            //todo delgeate zum mitgeben der ausgeführt werden soll wenn focus ausgeführt soll
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

        public Graph Graph
        {
            get { return (Graph)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Graph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Graph", typeof(Graph), typeof(GraphVisualization), new UIPropertyMetadata(null, OnGraphChanged));

        /// <summary>
        /// This method raises the MouseDoubleClick event
        /// </summary>
        private void RaiseMouseDoubleClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(GraphVisualization.MouseDoubleClickEvent);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// MouseDoubleClick CLR accessors for the event
        /// </summary>
        public event RoutedEventHandler MouseDoubleClick
        {
            add { AddHandler(MouseDoubleClickEvent, value); }
            remove { RemoveHandler(MouseDoubleClickEvent, value); }
        }
        /// <summary>
        /// Represents a dynamic data collection of EdgeVisualizations that provides notifications when items get added, removed, or when the whole list is refreshed.
        /// http://msdn.microsoft.com/en-us/library/ms668604.aspx?queryresult=true
        /// </summary>
        public IEnumerable<EdgeVisualization> EdgeVisualizations
        {
            get
            {
                return Children.OfType<EdgeVisualization>();
            }
        }
        /// <summary>
        /// Represents a dynamic data collection of VertexVisualizations that provides notifications when items get added, removed, or when the whole list is refreshed.
        /// http://msdn.microsoft.com/en-us/library/ms668604.aspx?queryresult=true
        /// </summary>
        public IEnumerable<VertexVisualization> VertexVisualizations
        {
            get
            {
                return Children.OfType<VertexVisualization>();
            }
        }
        /// <summary>
        /// Represents a dynamic data collection of Frameworkelements which got focused
        /// </summary>
        public FrameworkElement FocusedFrameworkElement
        {
            get
            {
                return this.Children.OfType<FrameworkElement>().Where(f => f.IsFocused.Equals(true)).First();
            }
        }


    }
}
