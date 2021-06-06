﻿using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Input;
using System.Linq;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Graph", "Get.UI")]
namespace DataStructures.UI
{
    public class EdgeControl : Control
    {
        static EdgeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EdgeControl), new FrameworkPropertyMetadata(typeof(EdgeControl)));
            StrokeThicknessProperty = System.Windows.Shapes.Shape.StrokeThicknessProperty.AddOwner(typeof(EdgeControl), new PropertyMetadata((double)1));
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

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.Focus();
        }
        /// <summary>
        /// Occurs before the KeyDown event when a key is pressed while focus is on this control.
        /// When the control is focused you can enter a number to change the weight of the edge
        /// </summary>
        /// <param name="e">Event Data</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            //http://stackoverflow.com/questions/8310777/convert-keydown-keys-to-one-string-c-sharp
            if (e != null)
            {
                if (e.Key.Equals(Key.Back))
                {
                    if (Edge.Weighted.ToString(CultureInfo.InvariantCulture).Length != 1)
                    {
                        String temp = Edge.Weighted.ToString(CultureInfo.InvariantCulture);
                        temp = temp.Remove(temp.Length - 1);
                        int result = 0;
                        if (Int32.TryParse(temp, out result))
                        {
                            Edge.Weighted = result;
                        }
                    }
                    else
                    {
                        Edge.Weighted = 0;
                    }

                }
                else
                {
                    if (e.Key >= Key.D0 && e.Key <= Key.D9
                        || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    {
                        // Number keys pressed so need to so special processing
                        // also check if shift pressed
                        String temp = Edge.Weighted.ToString(CultureInfo.InvariantCulture);
                        temp += e.Key.ToString()[1].ToString(CultureInfo.InvariantCulture);

                        int result = 0;
                        if (Int32.TryParse(temp, out result))
                        {
                            Edge.Weighted = result;
                        }
                    }

                }
            }
            //directed edges exists the other way around too. Set same weight
            var revertedEdge = Edge.V?.Edges?.FirstOrDefault(a => a.V.Equals(Edge.U));
            if (revertedEdge != null)
            {
                revertedEdge.Weighted = Edge.Weighted;
            }
            base.OnPreviewKeyDown(e);
        }
        /// <summary>
        /// Participates in rendering operations that are directed by the layout system. Adds the weighted as text in the middle of the edge
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Point p = new Point((PositionV.X + PositionU.X) / 2 + 4, (PositionV.Y + PositionU.Y) / 2);

            drawingContext?.DrawText(new FormattedText(Edge != null ? Edge.Weighted.ToString(CultureInfo.InvariantCulture) : "",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(this.FontFamily.ToString()),
                this.FontSize, this.Foreground, VisualTreeHelper.GetDpi(this).PixelsPerDip), p);

        }
        public DataStructures.IEdge Edge
        {
            get { return (DataStructures.IEdge)GetValue(EdgeProperty); }
            set { SetValue(EdgeProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Edge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EdgeProperty =
            DependencyProperty.Register("Edge", typeof(DataStructures.IEdge), typeof(EdgeControl), new UIPropertyMetadata(null, OnEdgeChanged));

        private static void OnEdgeChanged(DependencyObject pDependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if ((e.NewValue is IEdge) && (pDependencyObject != null && pDependencyObject.GetType().Equals(typeof(EdgeControl))))
            {
                IEdge edge = (IEdge)e.NewValue;
                //jetzt alle EdgeVisualization durchsuchen und schauen in welchem Edge unser Edge drin ist
                EdgeControl edgeVisualization = (EdgeControl)pDependencyObject;

                if (edge is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)edge).PropertyChanged += (sender, ePropertyChangedEventArgs) =>
                    {
                        //Rerender Weighted - OnRender()
                        edgeVisualization.InvalidateVisual();
                    };
                }


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
            DependencyProperty.Register("PositionU", typeof(Point), typeof(EdgeControl), new UIPropertyMetadata(new Point()));



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
            DependencyProperty.Register("PositionV", typeof(Point), typeof(EdgeControl), new UIPropertyMetadata(null));



        public bool Directed
        {
            get { return (bool)GetValue(DirectedProperty); }
            set { SetValue(DirectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Directed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectedProperty =
            DependencyProperty.Register("Directed", typeof(bool), typeof(EdgeControl), new UIPropertyMetadata(false));


        public static readonly ShiftConverter ShiftConverter = new ShiftConverter();

    }
    public class ShiftConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return new Point[] { };
            if (!values.GetType().Equals(typeof(System.Object[]))) return values;
            if (!values.Length.Equals(2)) return new Point[] { };
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
