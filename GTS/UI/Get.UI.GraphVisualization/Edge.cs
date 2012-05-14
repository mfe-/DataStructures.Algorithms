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
                                                             
            Point p = new Point((PositionV.X + PositionU.X)/2+4, (PositionV.Y + PositionU.Y)/2);

            drawingContext.DrawText(new FormattedText(Edge!=null ? Edge.Weighted.ToString() : "", 
                System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(this.FontFamily.ToString()),
                this.FontSize, this.Foreground), p);

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
            DependencyProperty.Register("PositionU", typeof(Point), typeof(EdgeVisualization), new UIPropertyMetadata(null));

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


    }
}
