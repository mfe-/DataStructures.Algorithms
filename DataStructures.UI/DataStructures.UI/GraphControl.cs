using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace DataStructures.UI
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
    public partial class GraphControl : Canvas
    {
        /// <summary>
        /// Represents a pseudo-random number generator, a device that produces a sequence of numbers that meet certain statistical requirements for randomness.
        /// http://msdn.microsoft.com/en-us/library/system.random.aspx?queryresult=true
        /// </summary>
        private Random _Random = new Random(DateTime.Now.Millisecond);

        public static readonly RoutedCommand AddVertexRoutedCommand = new RoutedCommand();
        public static readonly RoutedCommand SetDirectedRoutedCommand = new RoutedCommand();
        public static readonly RoutedCommand KruskalRoutedCommand = new RoutedCommand();
        public static readonly RoutedCommand ClearGraphCommand = new RoutedCommand();

        public static readonly RoutedEvent MouseDoubleClickEvent = EventManager.RegisterRoutedEvent(
        "MouseDoubleClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GraphControl));

        static GraphControl()
        {
            //set the backgroundcolor 
            BackgroundProperty.OverrideMetadata(typeof(GraphControl), new FrameworkPropertyMetadata(Brushes.Transparent));
            //enables commands in contextmenue if no item got is focused
            FocusableProperty.OverrideMetadata(typeof(GraphControl), new FrameworkPropertyMetadata(true));
        }

        #region Drag and Drop
        /// <summary>
        /// Called before the MouseLeftButtonDown event occurs.
        /// Preprares the drag and drop of the Vertex item. Raises the MouseDoubleClickEvent if the click counts equal two.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e != null)
            {
                if (e.Source is GraphControl)
                {
                    this.Focus();
                }
                var hitTestResult = VisualTreeHelper.HitTest(this, e.GetPosition(this));
                if (e.ClickCount.Equals(2) && hitTestResult != null && hitTestResult.VisualHit.Equals(this))
                {
                    RaiseMouseDoubleClickEvent();
                }

                if (e.Source != null && e.Source.GetType().Equals(typeof(VertexControl)) && e.LeftButton.Equals(MouseButtonState.Pressed) && SelectedMouseItem == null && !e.OriginalSource.GetType().Equals(typeof(AdornerItem)))
                {
                    this.SelectedMouseItem = (FrameworkElement)e.Source;
                    this.SelectedMouseItem.Focus();
                    Point p = new Point((e.GetPosition(this).X - SelectedMouseItem.ActualWidth / 2), (e.GetPosition(this).Y - SelectedMouseItem.ActualHeight / 2));
                    SetPosition(SelectedMouseItem, p);
                }

                if (e.Source != null && e.Source.GetType().Equals(typeof(VertexControl)) && e.LeftButton.Equals(MouseButtonState.Pressed) && SelectedMouseItem == null && e.OriginalSource.GetType().Equals(typeof(AdornerItem)))
                {
                    //create a temporary edge
                    EdgeControl ev = new EdgeControl();
                    Canvas.SetZIndex(ev, -1);
                    if (e.Source is VertexControl vv)
                    {

                        if (EdgeFactory == null) throw new ArgumentNullException($"Please set{nameof(EdgeFactory)}");
                        ev.Edge = EdgeFactory.Invoke(vv.Vertex);

                        ev.PositionU = GetPosition(vv);
                        ev.PositionV = e.GetPosition(this);
                        this.SelectedMouseItem = ev;
                        this.Children.Add(ev);
                    }
                }
            }
        }
        public Func<IVertex, IEdge> EdgeFactory { get; set; }
        /// <summary>
        /// Called before the MouseMove event occurs. Moves the Vertex item.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Debugger.IsAttached)
            {
                TextBlock textblock = Children.OfType<TextBlock>().FirstOrDefault<TextBlock>();
                if (textblock == null)
                {
                    textblock = new TextBlock();
                    textblock.Text = Mouse.GetPosition(this).ToString();
                    SetLeft(textblock, 0);
                    SetTop(textblock, 0);
                    Children.Add(textblock);
                }
                else
                {
                    textblock.Text = Mouse.GetPosition(this).ToString();
                }

            }

            if (e.LeftButton.Equals(MouseButtonState.Pressed) && SelectedMouseItem != null && SelectedMouseItem.GetType().Equals(typeof(VertexControl)) && !e.OriginalSource.GetType().Equals(typeof(AdornerItem)))
            {
                if (SelectedMouseItem is VertexControl v)
                {
                    Point p = new Point((e.GetPosition(this).X - SelectedMouseItem.ActualWidth / 2), (e.GetPosition(this).Y - SelectedMouseItem.ActualHeight / 2));
                    v.Position = p;
                    SetPosition(SelectedMouseItem, p);
                }
            }

            //move edge
            if (e.LeftButton.Equals(MouseButtonState.Pressed) && SelectedMouseItem != null && SelectedMouseItem.GetType().Equals(typeof(EdgeControl)))
            {
                if (SelectedMouseItem is EdgeControl ev)
                {
                    Point p = new Point((e.GetPosition(this).X - 4), (e.GetPosition(this).Y) - 4);
                    ev.PositionV = p;
                }
            }

        }
        /// <summary>
        /// Called before the MouseLeftButtonUp event occurs. Exits the drag and drop operation of the vertex item.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (e != null)
            {
                if (e.Source != null && e.LeftButton.Equals(MouseButtonState.Released) &&
                    SelectedMouseItem != null && typeof(VertexControl).Equals(SelectedMouseItem.GetType()) && !e.OriginalSource.GetType().Equals(typeof(AdornerItem)))
                {
                    VertexControl v = (VertexControl)SelectedMouseItem;
                    Point p = new Point((e.GetPosition(this).X - SelectedMouseItem.ActualWidth / 2), (e.GetPosition(this).Y - SelectedMouseItem.ActualHeight / 2));
                    v.Position = p;
                    SetPosition(SelectedMouseItem, p);
                    SelectedMouseItem = null;
                }
                //add edge to graph or remove temporary edge
                if (e.Source != null && e.LeftButton.Equals(MouseButtonState.Released) &&
                    SelectedMouseItem != null && typeof(EdgeControl).Equals(SelectedMouseItem.GetType()))
                {
                    HitTestResult result = VisualTreeHelper.HitTest(this, e.GetPosition(this));
                    FrameworkElement u = (FrameworkElement)result.VisualHit;
                    if (u != null)
                    {
                        VertexControl? vv = null;
                        if (!(u.TemplatedParent is VertexControl))
                        {
                            vv = TryFindParent<VertexControl>(u);
                        }
                        else
                        {
                            vv = (VertexControl)u.TemplatedParent;
                        }

                        EdgeControl ev = (EdgeControl)SelectedMouseItem;

                        //maybe we hit the ItemTemplate Control
                        if (vv == null && u.DataContext != null && u.DataContext is IVertex)
                        {
                            IVertex vertex = (IVertex)u.DataContext;
                            vv = VertexVisualizations.FirstOrDefault(a => a.Vertex == vertex);
                        }

                        if (vv != null)
                        {
                            //first find the vertex where we started 
                            IVertex v = VertexVisualizations.First(z => z.Vertex.Equals(ev.Edge.U)).Vertex;
                            //add to model edge 
                            if (Graph != null)
                            {
                                v.AddEdge(vv.Vertex, 0, Graph.Directed);
                            }
                        }
                        this.Children.Remove(SelectedMouseItem);
                    }
                    SelectedMouseItem = null;
                }
            }

        }

        #endregion Drag and Drop
        public static T? TryFindParent<T>(DependencyObject current) where T : class
        {
            DependencyObject parent = VisualTreeHelper.GetParent(current);
            if (parent == null)
                parent = LogicalTreeHelper.GetParent(current);
            if (parent == null)
                return null;

            if (parent is T)
                return parent as T;
            else
                return TryFindParent<T>(parent);
        }
        /// <summary>
        /// Measures the size of the current Canvas for the layout.
        /// http://msdn.microsoft.com/en-us/library/hh401019.aspx?queryresult=true
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            if (Children != null)
            {
                foreach (UIElement element in Children)
                {
                    double left = Canvas.GetLeft(element);
                    double top = Canvas.GetTop(element);
                    left = double.IsNaN(left) ? 0 : left;
                    top = double.IsNaN(top) ? 0 : top;

                    if (element != null)
                    {
                        element.Measure(constraint);

                        Size desiredSize = element.DesiredSize;
                        if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                        {
                            size.Width = Math.Max(size.Width, left + desiredSize.Width);
                            size.Height = Math.Max(size.Height, top + desiredSize.Height);
                        }
                    }

                }
            }

            // add some extra margin for scrollviewer
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        /// <summary>
        /// Creates for each Vertex and Edge the proper control to display the Graph.
        /// </summary>
        /// <param name="vertices"></param>
        protected virtual void InitialiseGraph(ObservableCollection<IVertex> vertices)
        {
            InitialiseGraph(vertices, null);
        }
        /// <summary>
        /// Creates for each Vertex a VertexVisualization and for each Edge a EdgeVisualization Control. 
        /// </summary>
        /// <param name="vertices">List of Vertices</param>
        /// <param name="e">The last added EdgeVisualization. EdgeVisualization.VertexVisualizationV will be set.</param>
        protected virtual void InitialiseGraph(ObservableCollection<IVertex> vertices, EdgeControl? e)
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            foreach (IVertex vertex in vertices)
            {
                if (vertex != null && vertex.Edges != null)
                {
                    if (vertex.Edges is ObservableCollection<IEdge> elist)
                    {
                        elist.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
                    }

                    VertexControl visualvertex;
                    bool vertexexists = GetItem(vertex) != null;

                    if (!vertexexists)
                    {

                        visualvertex = AddVertex(vertex);
                        Canvas.SetZIndex(visualvertex, 1);
                    }
                    else
                    {
                        visualvertex = VertexVisualizations.First(g => g.Vertex.Equals(vertex));
                    }
                    //add edge to vertex2
                    if (e != null)
                    {
                        EdgeControl edv = AddEdge(e, visualvertex, EdgeControl.PositionVProperty);

                        this.Children.Add(edv);
                        //bug? position of edge missing?
                        Canvas.SetZIndex(edv, -1);
                    }

                    if (vertexexists) return;
                    //add edge from vertex1
                    foreach (IEdge ed in vertex.Edges)
                    {
                        EdgeControl edv = AddEdge(ed, visualvertex, EdgeControl.PositionUProperty);

                        InitialiseGraph(new ObservableCollection<IVertex>() { ed.V }, edv);
                    }
                }
            }
        }

        protected void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (NotifyCollectionChangedAction.Add.Equals(e?.Action) && e?.NewItems.SyncRoot is IList<object> items)
            {
                object item = items.FirstOrDefault();
                if (item is IEdge edge)
                {

                    //check if vertex is kept in Graph.Vertices and will be moved into the depper graph - if yes its duplicated so remove from graph.vertices
                    if (this.Graph != null && this.Graph.Vertices.Contains(edge.V) && this.Graph.Vertices.Count > 1)
                    {
                        this.Graph.Vertices.CollectionChanged -= new NotifyCollectionChangedEventHandler(CollectionChanged);
                        this.Graph.Vertices.Remove(edge.V);
                        this.Graph.Vertices.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
                    }
                    if (this.Graph != null && this.Graph.Vertices.Contains(edge.U) && this.Graph.Vertices.Contains(edge.V) && this.Graph.Vertices.Count > 1)
                    {
                        this.Graph.Vertices.CollectionChanged -= new NotifyCollectionChangedEventHandler(CollectionChanged);
                        this.Graph.Vertices.Remove(edge.U);
                        this.Graph.Vertices.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
                    }

                    //check if  both vertices u and v exists, otherwise create visualvertex
                    if (GetItem(edge.U) == null)
                    {
                        AddVertex(edge.U);
                    }

                    if (GetItem(edge.V) == null)
                    {
                        AddVertex(edge.V);
                    }

                    //added item already exists - nothing to do
                    if (GetItem(edge) != null)
                    {
                        return;
                    }
                    else
                    {
                        AddEdge(edge);
                    }
                }
                if (item is IVertex v)
                {
                    AddVertex(v, Mouse.GetPosition(this).Add(-25, -25));
                }
            }
            if (NotifyCollectionChangedAction.Remove.Equals(e?.Action) && e?.OldItems.SyncRoot is IList<object> vitems)
            {
                object item = vitems.First();
                if (item is IVertex v)
                {
                    //catch Visualization item
                    VertexControl vv = VertexVisualizations.First(a => a.Vertex.Equals(v));
                    this.Children.Remove(vv);
                }

                if (item is IEdge edge)
                {
                    RemoveEdge(edge);
                }
            }
            Debug.WriteLine(this.Graph?.Vertices?.Count);
        }

        private static void OnGraphChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null && dependencyObject != null && typeof(GraphControl).Equals(dependencyObject.GetType()))
            {
                GraphControl graphVisualization = (GraphControl)dependencyObject;
                graphVisualization?.Children?.Clear();
                return;
            }
            if (e.NewValue != null && e.NewValue.GetType() != (typeof(Graph))) return;
            if (e.NewValue != null && dependencyObject != null && typeof(GraphControl).Equals(dependencyObject.GetType()))
            {
                GraphControl graphVisualization = (GraphControl)dependencyObject;
                Graph graph = (Graph)e.NewValue;
                if (graphVisualization.Graph != null)
                {
                    graphVisualization.Graph.Vertices.CollectionChanged += new NotifyCollectionChangedEventHandler(graphVisualization.CollectionChanged);
                    graphVisualization.InitialiseGraph(graph.Vertices);
                }
            }
        }

        #region addVertex
        /// <summary>
        /// Adds a Vertex to the _Canvas.
        /// The position of VertexVisualization will be set randomly on the canvas.
        /// </summary>
        /// <param name="v">Vertex which should be added to the VertexVisualization</param>
        /// <returns>Returns the created VertexVisualization</returns>
        protected virtual VertexControl AddVertex(IVertex v)
        {
            return AddVertex(v, new Point(GetRandomNumber(0, this.ActualWidth - 10), GetRandomNumber(0, this.ActualHeight - 10)));
        }
        /// <summary>
        /// Adds a Vertex to the _Canvas.
        /// </summary>
        /// <param name="v">Vertex which should be added to the VertexVisualization</param>
        /// <param name="point">Sets the position where the VertexVisualization should be placed on the _Canvas</param>
        /// <returns>Returns the created VertexVisualization</returns>
        protected virtual VertexControl AddVertex(IVertex v, Point point)
        {
            VertexControl vertexcontrol = new VertexControl();
            vertexcontrol.Vertex = v;

            vertexcontrol.Position = point;
            SetLeft(vertexcontrol, point.X);
            SetTop(vertexcontrol, point.Y);

            this.Children.Add(vertexcontrol);
            if (v.Edges is ObservableCollection<IEdge> e)
            {
                e.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
            }
            else
            {
                throw new NotImplementedException($"Overgiven instance of {v} has not {nameof(ObservableCollection<IEdge>)} implemented for {nameof(IVertex.Edges)}");
            }
            return vertexcontrol;
        }

        #endregion

        #region addEdge
        /// <summary>
        /// Creates a EdgeVisualization by searching the proper vertices v1 and v2 in the graph and connects them (u->v)
        /// </summary>
        /// <param name="e">The Edge which should be visualised</param>
        /// <returns>Returns the created EdgeVisualization</returns>
        protected virtual EdgeControl AddEdge(IEdge e)
        {
            return AddEdge(e, true);
        }
        /// <summary>
        /// Creates a EdgeVisualization by searching the proper vertices v1 and v2 in the graph and connects them (u->v)
        /// </summary>
        /// <param name="e">The Edge which should be visualised</param>
        /// <param name="pAddtoCanvas">Determineds whether the EdgeVisualization should be displayed.</param>
        /// <returns>Returns the created EdgeVisualization</returns>
        protected virtual EdgeControl AddEdge(IEdge e, bool pAddtoCanvas)
        {
            //get vertex1
            VertexControl uv = VertexVisualizations.First(z => z.Vertex.Equals(e.U));

            if (uv == null)
                throw new ArgumentNullException(nameof(e), "There was no proper VertexVisualization found for the Ege.U. Please check if there exists the requried vertex on which you try to create an EdgeVisualization.");

            EdgeControl edv = AddEdge(e, uv, EdgeControl.PositionUProperty);


            //get vertex2
            VertexControl vv = VertexVisualizations.First(z => z.Vertex.Equals(e.V));
            if (vv == null)
                throw new ArgumentNullException(nameof(e), "There was no proper VertexVisualization found for the Ege.V. Please check if there exists the requried vertex on which you try to create an EdgeVisualization.");

            edv = AddEdge(edv, vv, EdgeControl.PositionVProperty);

            //add to canvas
            if (pAddtoCanvas)
            {
                this.Children.Add(edv);
                //bug? position of edge missing?
                Canvas.SetZIndex(edv, -1);
            }

            return edv;
        }
        /// <summary>
        /// Creates a EdgeVisualization by using the overgiven VertexVisualization. Use this when at runtime the second VertexVisualization doesnt exist.
        /// </summary>
        /// <param name="e">The Edge which should be visualised</param>
        /// <param name="pVertexVisualization">The VertexVisualization which should be used</param>
        /// <param name="dependencyProperty">On which Position U/V the VertexVisualization should be set</param>
        /// <returns>Returns the created EdgeVisualization</returns>
        protected virtual EdgeControl AddEdge(IEdge pEdge, VertexControl pVertexVisualization, DependencyProperty dependencyProperty)
        {
            if (dependencyProperty == null) throw new ArgumentNullException(nameof(dependencyProperty));
            EdgeControl edv = new EdgeControl() { Edge = pEdge };
            VertexControl uv = pVertexVisualization;

            if (EdgeControl.PositionUProperty.Equals(dependencyProperty))
            {
                edv.PositionU = GetPosition(pVertexVisualization);
            }
            else
            {
                edv.PositionV = GetPosition(pVertexVisualization);
            }

            CreatePositionBinding(edv, uv, dependencyProperty, Converters.PointAdderConverter, new Point(uv.Width / 2, uv.Height / 2));

            CreateDirectedBinding(edv, this.Graph);

            return edv;
        }
        /// <summary>
        /// Adds the second VertexVisualization to the EdgeVisualization, by using the overgiven VertexVisualization
        /// </summary>
        /// <param name="edgeVisualization">The EdgeVisualization which should be used</param>
        /// <param name="vertexVisualization">The VertexVisualization which should be used</param>
        /// <param name="dependencyProperty">On which Position U/V the VertexVisualization should be set</param>
        /// <returns>Returns the modified EdgeVisualization</returns>
        protected virtual EdgeControl AddEdge(EdgeControl edgeVisualization, VertexControl vertexVisualization, DependencyProperty dependencyProperty)
        {
            if (dependencyProperty == null) throw new ArgumentNullException(nameof(dependencyProperty));
            if (edgeVisualization == null) throw new ArgumentNullException(nameof(edgeVisualization));
            if (vertexVisualization == null) throw new ArgumentNullException(nameof(vertexVisualization));
            EdgeControl edv = edgeVisualization;
            VertexControl uv = vertexVisualization;

            if (dependencyProperty.Equals(EdgeControl.PositionUProperty))
            {
                edv.PositionU = GetPosition(vertexVisualization);
                CreatePositionBinding(edv, uv, dependencyProperty, null, new Point(uv.Width / 2, uv.Height / 2));
            }
            else
            {
                edv.PositionV = GetPosition(vertexVisualization);
                CreatePositionBinding(edv, uv, dependencyProperty, Converters.PointAdderConverter, new Point(uv.Width / 2, uv.Height / 2));
            }
            return edv;
        }
        /// <summary>
        /// Creates a binding between the Edge.Position and the Vertex.Position
        /// </summary>
        /// <param name="setBindingSource">EdgeVisualization on which the binding should be set</param>
        /// <param name="pBindingSource">From which VertexVisualization the position should be used</param>
        /// <param name="pDependencyProperty">On which Position Property the binding should be set. Use EdgeVisualization.PositionUProperty or dgeVisualization.PositionVProperty))</param>
        /// <param name="Converter">Set a Converter if needed</param>
        /// <param name="ConverterParameter">Parameter for the Converter</param>
        protected virtual void CreatePositionBinding(EdgeControl setBindingSource, VertexControl pBindingSource, DependencyProperty pDependencyProperty, IValueConverter Converter, object ConverterParameter)
        {
            if (setBindingSource == null) throw new ArgumentNullException(nameof(setBindingSource));
            if (setBindingSource.GetBindingExpression(pDependencyProperty) == null)
            {
                Binding bindingU = new Binding("Position");
                bindingU.Source = pBindingSource;
                bindingU.Mode = BindingMode.TwoWay;
                bindingU.NotifyOnSourceUpdated = true;
                bindingU.NotifyOnTargetUpdated = true;
                bindingU.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                bindingU.Converter = Converters.PointAdderConverter;
                bindingU.ConverterParameter = ConverterParameter;
                setBindingSource.SetBinding(pDependencyProperty, bindingU);
            }
        }
        //not tested yet
        protected virtual void CreateDirectedBinding(EdgeControl setBindingSource, Graph pGraphSource)
        {
            if (setBindingSource == null) throw new ArgumentNullException(nameof(setBindingSource));
            if (setBindingSource.GetBindingExpression(EdgeControl.DirectedProperty) == null)
            {
                Binding bindingDirected = new Binding("Directed");
                bindingDirected.Source = pGraphSource;
                bindingDirected.Mode = BindingMode.OneWay;
                bindingDirected.NotifyOnSourceUpdated = true;
                bindingDirected.NotifyOnTargetUpdated = false;
                bindingDirected.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                setBindingSource.SetBinding(EdgeControl.DirectedProperty, bindingDirected);
            }
        }
        #endregion

        protected virtual void RemoveEdge(IEdge pEdge)
        {
            IEdge edge = pEdge;
            if (EdgeVisualizations.Any(a => a.Edge.Equals(edge)))
            {
                EdgeControl ev = EdgeVisualizations.First(a => a.Edge.Equals(edge));
                this.Children.Remove(ev);
            }

            if (Graph.Directed.Equals(false))
            {
                //graph is undirected so remove opposite edge, if exists
                if (EdgeVisualizations.Any(a => a.Edge.U.Equals(pEdge.V) && a.Edge.V.Equals(pEdge.U)).Equals(0))
                {
                    RemoveEdge(EdgeVisualizations.First(a => a.Edge.U.Equals(pEdge.V) && a.Edge.V.Equals(pEdge.U)).Edge);
                }
            }
        }

        /// <summary>
        /// Returns the position of the delivered VertexVisualization control
        /// </summary>
        /// <param name="u">From which VertexVisualization the position should be returned</param>
        /// <returns>Position of the VertexVisualization control</returns>
        protected virtual Point GetPosition(FrameworkElement u)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            double left = Canvas.GetLeft(u as UIElement);
            double top = Canvas.GetTop(u as UIElement);
            return new Point(left + (u.Width / 2), top + (u.Height / 2));
        }
        protected virtual void SetPosition(FrameworkElement f, Point p)
        {
            Canvas.SetLeft(f, p.X);
            Canvas.SetTop(f, p.Y);
        }
        /// <summary>
        /// Searches for the overgiven vertex and returns the vertexVisualization control which is representing it
        /// </summary>
        /// <param name="v">Vertex which should be looked up</param>
        /// <returns>Control which is using the Vertex</returns>
        protected virtual VertexControl GetItem(IVertex v)
        {
            return VertexVisualizations.FirstOrDefault(a => a.Vertex.Equals(v));
        }
        /// <summary>
        /// Searches for the overgiven edge and returns the EdgeVisualization control which is representing it
        /// </summary>
        /// <param name="v">edge which should be looked up</param>
        /// <returns>Control which is using the edge</returns>
        protected virtual EdgeControl GetItem(IEdge e)
        {
            return EdgeVisualizations.FirstOrDefault(a => a.Edge.Equals(e));
        }
        /// <summary>
        /// Calls the focus method on the VertexVisualization control
        /// </summary>
        /// <param name="v"></param>
        public virtual void SetFocus(IVertex v)
        {
            if (GetItem(v) != null)
                GetItem(v).Focus();
        }
        /// <summary>
        /// Calls the focus method on the VertexVisualization control
        /// </summary>
        /// <param name="v"></param>
        public virtual void SetFocus(IEdge e)
        {
            if (GetItem(e) != null)
                GetItem(e).Focus();
        }
        public virtual void SetFocus(IEdge e, AsyncCallback b)
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

        public Graph? Graph
        {
            get { return (Graph?)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Graph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Graph", typeof(Graph), typeof(GraphControl), new UIPropertyMetadata(null, OnGraphChanged));

        /// <summary>
        /// This method raises the MouseDoubleClick event
        /// </summary>
        private void RaiseMouseDoubleClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(GraphControl.MouseDoubleClickEvent);
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


        private FrameworkElement? _SelectedMouseItem;
        protected FrameworkElement? SelectedMouseItem
        {
            get { return _SelectedMouseItem; }
            set
            {
                if (value != null)
                {
                    SelectedItem = value;
                    if (SelectedItem is VertexControl v)
                    {
                        SelectedVertex = v.Vertex;
                    }
                }
                _SelectedMouseItem = value;
            }
        }


        public IVertex SelectedVertex
        {
            get { return (IVertex)GetValue(SelectedVertexProperty); }
            set { SetValue(SelectedVertexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedVertex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedVertexProperty =
            DependencyProperty.Register("SelectedVertex", typeof(IVertex), typeof(GraphControl), new UIPropertyMetadata(null, OnSelectedVertexChanged));

        private static void OnSelectedVertexChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is GraphControl && e.NewValue != null && e.NewValue is IVertex)
            {
                GraphControl graphControl = (GraphControl)dependencyObject;
                var vertexControl = graphControl.GetItem((IVertex)e.NewValue);
                if (!vertexControl.IsFocused)
                {
                    vertexControl.Focus();
                }
            }
        }

        public FrameworkElement? SelectedItem { get; set; }

        /// <summary>
        /// Represents a dynamic data collection of EdgeVisualizations that provides notifications when items get added, removed, or when the whole list is refreshed.
        /// http://msdn.microsoft.com/en-us/library/ms668604.aspx?queryresult=true
        /// </summary>
        public IEnumerable<EdgeControl> EdgeVisualizations
        {
            get
            {
                return Children.OfType<EdgeControl>();
            }
        }
        /// <summary>
        /// Represents a dynamic data collection of VertexVisualizations that provides notifications when items get added, removed, or when the whole list is refreshed.
        /// http://msdn.microsoft.com/en-us/library/ms668604.aspx?queryresult=true
        /// </summary>
        public IEnumerable<VertexControl> VertexVisualizations
        {
            get
            {
                return Children.OfType<VertexControl>();
            }
        }
        /// <summary>
        /// Represents a dynamic data collection of Frameworkelements which got focused
        /// </summary>
        public FrameworkElement? FocusedFrameworkElement
        {
            get
            {
                IEnumerable<FrameworkElement> FElementList = this.Children.OfType<FrameworkElement>().Where(f => f.IsFocused.Equals(true));
                if (FElementList != null && FElementList.Any())
                {
                    return FElementList.First();
                }
                else
                {
                    return null;
                }
            }
        }


    }
}
