using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Metadata;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Graph", "Get.UI")]
namespace DataStructures.UI
{
    public class EdgeControl : Control
    {
        static EdgeControl()
        {
            //    DefaultStyleKeyProperty.OverrideMetadata(typeof(EdgeControl), new FrameworkPropertyMetadata(typeof(EdgeControl)));
            //    StrokeThicknessProperty = System.Windows.Shapes.Shape.StrokeThicknessProperty.AddOwner(typeof(EdgeControl), new PropertyMetadata((double)1));
        }

        public EdgeControl()
            : base()
        {
        }
        protected override Size MeasureOverride(Size constraint)
        {
            //does not work
            //add some extra space for better selecting the item (IsMouseover, OnClick usw)
            return base.MeasureOverride(new Size(constraint.Width + 5, constraint.Height + 10));
        }

        //protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);
        //    this.Focus();
        //}
        ///// <summary>
        ///// Occurs before the KeyDown event when a key is pressed while focus is on this control.
        ///// When the control is focused you can enter a number to change the weight of the edge
        ///// </summary>
        ///// <param name="e">Event Data</param>
        //protected override void OnPreviewKeyDown(KeyEventArgs e)
        //{
        //    //http://stackoverflow.com/questions/8310777/convert-keydown-keys-to-one-string-c-sharp
        //    if (e.Key.Equals(Key.Back))
        //    {
        //        if (Edge.Weighted.ToString().Length != 1)
        //        {
        //            String temp = Edge.Weighted.ToString();
        //            temp = temp.Remove(temp.Length - 1);
        //            int result = 0;
        //            if (Int32.TryParse(temp, out result))
        //            {
        //                Edge.Weighted = result;
        //            }
        //        }
        //        else
        //        {
        //            Edge.Weighted = 0;
        //        }

        //    }
        //    else
        //    {
        //        if (e.Key >= Key.D0 && e.Key <= Key.D9 
        //            || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        //        {
        //            // Number keys pressed so need to so special processing
        //            // also check if shift pressed
        //            String temp = Edge.Weighted.ToString();
        //            temp += e.Key.ToString()[1].ToString();

        //            int result = 0;
        //            if (Int32.TryParse(temp, out result))
        //            {
        //                Edge.Weighted = result;

        //            }
        //        }

        //    }
        //    //directed edges exists the other way around too. Set same weight
        //    var revertedEdge = Edge.GetOppositeEdge();
        //    if (revertedEdge != null)
        //    {
        //        revertedEdge.Weighted = Edge.Weighted;
        //    }
        //    base.OnPreviewKeyDown(e);

        //}
        /// <summary>
        /// Participates in rendering operations that are directed by the layout system. Adds the weighted as text in the middle of the edge
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    base.OnRender(drawingContext);

        //    Point p = new Point((PositionV.X + PositionU.X) / 2 + 4, (PositionV.Y + PositionU.Y) / 2);

        //    drawingContext.DrawText(new FormattedText(Edge != null ? Edge.Weighted.ToString() : "",
        //        CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(this.FontFamily.ToString()),
        //        this.FontSize, this.Foreground), p);

        //}

        private IEdge _Edge = null;
        public IEdge Edge
        {
            get { return _Edge; }
            set { SetAndRaise(EdgeProperty, ref _Edge, value); }
        }

        // Using a DependencyProperty as the backing store for Directed.  This enables animation, styling, binding, etc...
        public static readonly DirectProperty<EdgeControl, IEdge> EdgeProperty =
            AvaloniaProperty.RegisterDirect<EdgeControl, IEdge>(nameof(Edge), o => o.Edge, OnEdgeChanged, defaultBindingMode: Avalonia.Data.BindingMode.Default);

        private static void OnEdgeChanged(EdgeControl pDependencyObject, IEdge e)
        {
            if ((e != null) && (pDependencyObject != null))
            {
                //register event only on a diffrent value
                if(pDependencyObject.Edge != e)
                {
                    IEdge edge = e;
                    //jetzt alle EdgeVisualization durchsuchen und schauen in welchem Edge unser Edge drin ist
                    EdgeControl edgeVisualization = pDependencyObject as EdgeControl;

                    edge.PropertyChanged += (sender, ePropertyChangedEventArgs) =>
                    {

                        //Rerender Weighted - OnRender()
                        edgeVisualization.InvalidateVisual();
                    };
                }
            }
            //set property
            pDependencyObject.Edge = e;
        }


        //public static readonly DependencyProperty StrokeThicknessProperty;

        ///// <summary>
        ///// Gets or sets the width of the shape outline. 
        ///// </summary>
        //[TypeConverter(typeof(LengthConverter))]
        //public double StrokeThickness
        //{
        //    get { return (double)GetValue(StrokeThicknessProperty); }
        //    set { SetValue(StrokeThicknessProperty, value); }
        //}

        /// <summary>
        /// Gets or sets the value of the position from the correspondending U Vertex control
        /// </summary>
        private Point _PositionU = new Point();
        public Point PositionU
        {
            get { return _PositionV; }
            set { SetAndRaise(PositionUProperty, ref _PositionU, value); }
        }

        // Using a DependencyProperty as the backing store for Directed.  This enables animation, styling, binding, etc...
        public static readonly DirectProperty<EdgeControl, Point> PositionUProperty =
            AvaloniaProperty.RegisterDirect<EdgeControl, Point>(nameof(PositionU), o => o.PositionU, (o, v) => o.PositionU = v, defaultBindingMode: Avalonia.Data.BindingMode.Default);

        /// <summary>
        /// Gets or sets the value of the position from the correspondending V Vertex control
        /// </summary>
        private Point _PositionV = new Point();
        public Point PositionV
        {
            get { return _PositionV; }
            set { SetAndRaise(PositionVProperty, ref _PositionV, value); }
        }

        // Using a DependencyProperty as the backing store for Directed.  This enables animation, styling, binding, etc...
        public static readonly DirectProperty<EdgeControl, Point> PositionVProperty =
            AvaloniaProperty.RegisterDirect<EdgeControl, Point>(nameof(PositionV), o => o.PositionV, (o, v) => o.PositionV = v, defaultBindingMode: Avalonia.Data.BindingMode.Default);


        private bool _Directed = false;
        public bool Directed
        {
            get { return _Directed; }
            set { SetAndRaise(DirectedProperty, ref _Directed, value); }
        }

        //// Using a DependencyProperty as the backing store for Directed.  This enables animation, styling, binding, etc...
        public static readonly DirectProperty<EdgeControl, bool> DirectedProperty =
            AvaloniaProperty.RegisterDirect<EdgeControl, bool>(nameof(Directed), o => o.Directed, (o, v) => o.Directed = v, defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        public static ShiftConverter ShiftConverter = new ShiftConverter();

    }
    public class ShiftConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return new Point[] { };
            if (!values.GetType().Equals(typeof(IList<object>))) return values;
            if (!values.Count().Equals(2)) return new Point[] { };
            if (!values[0].GetType().Equals(typeof(Point))) return new Point[] { };
            if (!values[1].GetType().Equals(typeof(Point))) return new Point[] { };

            double r = 20;

            if ((parameter == null)) r = 20;
            if ((parameter != null) && !(Double.TryParse(parameter.ToString(), out r))) r = 20;


            Point pu = (Point)values[0];
            Point pv = (Point)values[1];

            //EdgeVisualization edgev = value as EdgeVisualization;

            double dx = pv.X - pu.X;
            double dy = pv.Y - pu.Y;
            double alpha = Math.Atan2(dy, dx);
            double b = Math.Sqrt((dx * dx) + (dy * dy));

            double c = (b - r) * Math.Sin(alpha);
            double d = (b - r) * Math.Cos(alpha);

            double dxx = pu.X + d;
            double dyy = pu.Y + c;

            return new Point(dxx, dyy);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { value };
        }
    }
}
