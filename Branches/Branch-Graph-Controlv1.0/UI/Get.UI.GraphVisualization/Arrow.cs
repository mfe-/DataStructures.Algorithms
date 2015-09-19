using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;

namespace Get.UI
{
    /// <summary>
    /// WPF Arrow and Custom Shapes
    /// http://www.codeproject.com/Articles/23116/WPF-Arrow-and-Custom-Shapes
    /// </summary>
    public sealed class Arrow : Shape
    {
        #region Dependency Properties



        public Point Point1
        {
            get { return (Point)GetValue(Point1Property); }
            set { SetValue(Point1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Point1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Point1Property =
            DependencyProperty.Register("Point1", typeof(Point), typeof(Arrow), new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));



        public Point Point2
        {
            get { return (Point)GetValue(Point2Property); }
            set { SetValue(Point2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Point2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Point2Property =
            DependencyProperty.Register("Point2", typeof(Point), typeof(Arrow), new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register("HeadWidth", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadHeightProperty = DependencyProperty.Register("HeadHeight", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region CLR Properties

        [TypeConverter(typeof(LengthConverter))]
        public double HeadWidth
        {
            get { return (double)base.GetValue(HeadWidthProperty); }
            set { base.SetValue(HeadWidthProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadHeight
        {
            get { return (double)base.GetValue(HeadHeightProperty); }
            set { base.SetValue(HeadHeightProperty, value); }
        }

        #endregion

        #region Overrides

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    InternalDrawArrowGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        #endregion

        #region Privates

        private void InternalDrawArrowGeometry(StreamGeometryContext context)
        {
            double theta = Math.Atan2(Point1.Y - Point2.Y, Point1.X - Point2.X);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            Point pt1 = Point1;
            Point pt2 = Point2;

            Point pt3 = new Point(
                Point2.X + (HeadWidth * cost - HeadHeight * sint),
                Point2.Y + (HeadWidth * sint + HeadHeight * cost));

            Point pt4 = new Point(
                Point2.X + (HeadWidth * cost + HeadHeight * sint),
                Point2.Y - (HeadHeight * cost - HeadWidth * sint));

            context.BeginFigure(pt1, true, false);
            context.LineTo(pt2, true, true);
            context.LineTo(pt3, true, true);
            context.LineTo(pt2, true, true);
            context.LineTo(pt4, true, true);
        }

        #endregion
    }
}
