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

namespace Get.UI
{
    public class EdgeVisualization : Control
    {
        static EdgeVisualization()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EdgeVisualization), new FrameworkPropertyMetadata(typeof(EdgeVisualization)));
        }

        public EdgeVisualization()
            : base()
        {

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

        public Point PositionU
        {
            get { return (Point)GetValue(PositionUProperty); }
            set { SetValue(PositionUProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PositionU.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionUProperty =
            DependencyProperty.Register("PositionU", typeof(Point), typeof(EdgeVisualization), new UIPropertyMetadata(null));



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
