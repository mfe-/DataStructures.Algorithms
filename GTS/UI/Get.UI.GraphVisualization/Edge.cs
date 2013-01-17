using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using Get.UI;
using System.Diagnostics;
using System.Windows;
using Get.Model.Graph;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Windows.Data;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Graph", "Get.UI")]
namespace Get.UI
{
    public class EdgeVisualization : Control
    {
        static EdgeVisualization()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EdgeVisualization), new FrameworkPropertyMetadata(typeof(EdgeVisualization)));
            StrokeThicknessProperty = System.Windows.Shapes.Line.StrokeThicknessProperty.AddOwner(typeof(EdgeVisualization), new PropertyMetadata((double)1));
        }

        public EdgeVisualization()
            : base()
        {
            //todo: ondrawing überschreiben und TextBlock hinzufügen?
        }
        /// <summary>
        /// Participates in rendering operations that are directed by the layout system. Adds the weighted as text in the middle of the edge
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Point p = new Point((PositionV.X + PositionU.X) / 2 + 4, (PositionV.Y + PositionU.Y) / 2);

            drawingContext.DrawText(new FormattedText(Edge != null ? Edge.Weighted.ToString() : "",
                System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(this.FontFamily.ToString()),
                this.FontSize, this.Foreground), p);

 EdgeVisualization edgev = this;

            double dx = (edgev.PositionV.X ) - edgev.PositionU.X;
            double dy = (edgev.PositionV.Y ) - edgev.PositionU.Y;
            double alpha = Math.Atan2(dy, dx);
            double b = Math.Sqrt((dx * dx) + (dy * dy));

            double r = 20;

            double c = (b - r) * Math.Sin(alpha);
            double d = (b - r) * Math.Cos(alpha);

            double dxx = edgev.PositionU.X + d;
            double dyy = edgev.PositionU.Y + c;

            drawingContext.DrawEllipse(BorderBrush, new Pen(this.Foreground, this.StrokeThickness), new Point(dxx, dyy), 2, 2);

        }
        public Get.Model.Graph.Edge Edge
        {
            get { return (Get.Model.Graph.Edge)GetValue(EdgeProperty); }
            set { SetValue(EdgeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Edge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EdgeProperty =
            DependencyProperty.Register("Edge", typeof(Get.Model.Graph.Edge), typeof(EdgeVisualization), new UIPropertyMetadata(null, OnEdgeChanged));

        private static void OnEdgeChanged(DependencyObject pDependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if ((e.NewValue != null && e.NewValue.GetType().Equals(typeof(Edge))) && (pDependencyObject != null && pDependencyObject.GetType().Equals(typeof(EdgeVisualization))))
            {
                Edge edge = e.NewValue as Edge;
                //jetzt alle EdgeVisualization durchsuchen und schauen in welchem Edge unser Edge drin ist
                EdgeVisualization edgeVisualization = pDependencyObject as EdgeVisualization;

            }
        }

        public static readonly DependencyProperty StrokeThicknessProperty;

        /// <summary>
        /// Gets or sets the width of the shape outline. 
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        /// <summary>
        /// Gets or sets the value of the position from the correspondending U Vertex control
        /// </summary>
        public Point PositionU
        {
            get { return (Point)GetValue(PositionUProperty); }
            set { SetValue(PositionUProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PositionU.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionUProperty =
            DependencyProperty.Register("PositionU", typeof(Point), typeof(EdgeVisualization), new UIPropertyMetadata(new Point()));



        /// <summary>
        /// Gets or sets the value of the position from the correspondending V Vertex control
        /// </summary>
        public Point PositionV
        {
            get { return (Point)GetValue(PositionVProperty); }
            set { SetValue(PositionVProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PositionV.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionVProperty =
            DependencyProperty.Register("PositionV", typeof(Point), typeof(EdgeVisualization), new UIPropertyMetadata(null));



        public bool Directed
        {
            get { return (bool)GetValue(DirectedProperty); }
            set { SetValue(DirectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Directed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectedProperty =
            DependencyProperty.Register("Directed", typeof(bool), typeof(EdgeVisualization), new UIPropertyMetadata(false));


        public static ShiftConverter ShiftConverter = new ShiftConverter();

    }
    public sealed class ShiftConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">EdgeVisualization</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">move pixel</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (!value.GetType().Equals(typeof(Point))) return value;
            if (value == null) return value;
            Point p = (Point)value;
            
            //EdgeVisualization edgev = value as EdgeVisualization;

            //double dx = edgev.PositionV.X - edgev.PositionU.X;
            //double dy = edgev.PositionV.Y - edgev.PositionU.Y;
            //double alpha = Math.Atan2(dy, dx);
            //double b = Math.Sqrt((dx * dx) + (dy * dy));

            //double r = 20;

            //double c = (b - r) * Math.Sin(alpha);
            //double d = (b - r) * Math.Cos(alpha);

            //double dxx = edgev.PositionU.X + d;
            //double dyy = edgev.PositionU.Y + c;

            return new Point(p.X+20, p.Y+20);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
