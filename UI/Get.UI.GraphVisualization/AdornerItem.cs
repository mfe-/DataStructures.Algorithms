//using System.Windows;
//using System.Windows.Documents;
//using System.Windows.Media;

//namespace DataStructures.UI
//{
//    public class AdornerItem : Adorner
//    {
//        public AdornerItem(UIElement adornedElement)
//            : base(adornedElement)
//        {
//            this.Focusable = true;
//        }
//        // A common way to implement an adorner's rendering behavior is to override the OnRender
//        // method, which is called by the layout system as part of a rendering pass.
//        protected override void OnRender(DrawingContext drawingContext)
//        {
//            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

//            // Some arbitrary drawing implements.
//            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Black);
//            renderBrush.Opacity = 0.2;
//            Pen renderPen = new Pen(new SolidColorBrush(Colors.Gray), 1.5);
//            double renderRadius = 5.0;

//            // Draw a circle at each corner.
//            //drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
//            //drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
//            //drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
//            //drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);

//            drawingContext.DrawEllipse(renderBrush, renderPen, new Point(adornedElementRect.Width / 2, 0).Add(0, -6), renderRadius, renderRadius);
//            drawingContext.DrawEllipse(renderBrush, renderPen, new Point(adornedElementRect.Width, adornedElementRect.Height / 2).Add(6, 0), renderRadius, renderRadius);
//            drawingContext.DrawEllipse(renderBrush, renderPen, new Point(0, adornedElementRect.Width / 2).Add(-6, 0), renderRadius, renderRadius);
//            drawingContext.DrawEllipse(renderBrush, renderPen, new Point(adornedElementRect.Width / 2, adornedElementRect.Height).Add(0, 6), renderRadius, renderRadius);

//        }
//    }
//}
