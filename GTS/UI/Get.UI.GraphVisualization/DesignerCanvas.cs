using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.IO;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Graph", "Get.UI")]
namespace Get.UI
{
    /// <summary>
    /// Adds some extra features to the Canvas like enabeling scrollbars
    /// http://www.codeproject.com/KB/WPF/WPFDiagramDesigner_Part2.aspx
    /// </summary>
    public class DesignerCanvas : Canvas
    {
        public static readonly RoutedEvent ChildAddedEvent;

        public delegate void ChildAddedEventHandler(object sender, RoutedChildAddedEventArgs e);

        public DesignerCanvas()
        {
            //set backgroundcolor to activate PreviewMouseDown event
            //http://social.msdn.microsoft.com/forums/en-US/wpf/thread/ad232a54-5ec2-4393-a281-f9f808f1e006/
            Background = Brushes.White;
        }
        static DesignerCanvas()
        {
            AllowDropProperty.OverrideMetadata(typeof(DesignerCanvas), new FrameworkPropertyMetadata(true));
            ChildAddedEvent = EventManager.RegisterRoutedEvent("ChildAdded", RoutingStrategy.Bubble, typeof(ChildAddedEventHandler), typeof(DesignerCanvas));
        }
        /// <summary>
        /// Deselects all selected Items because the user clicked on the canvas
        /// Invoked when an unhandled Mouse. MouseDown attached event reaches an element in its route that is derived 
        /// from this class.Implement this method to add class handling for this event. (Inherited from UIElement.)
        /// http://msdn.microsoft.com/de-de/library/system.windows.uielement.onmousedown.aspx
        /// </summary>
        /// <param name="e">The MouseButtonEventArgs that contains the event data.This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            DeselectAll();
        }
        /// <summary>
        /// Occurs when the mouse button is released during a drag-and-drop operation.
        /// Raise a ChildAddedEvent with the proper RoutedChildAddedEventArgs. You can add the "new" Control (DesignerItem) by
        /// registering the ChildAddedEvent and do Canvas.Children.Add(e.Child);
        /// </summary>
        /// <param name="e">A [System.Windows.DragEventArgs] that contains the event data.</param>
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            string xamlString = e.Data.GetData("DESIGNER_ITEM") as string;
            if (!String.IsNullOrEmpty(xamlString))
            {
                DesignerItem newItem = null;
                FrameworkElement content = XamlReader.Load(XmlReader.Create(new StringReader(xamlString))) as FrameworkElement;

                if (content != null)
                {
                    newItem = new DesignerItem();
                    newItem.Content = content;

                    Point position = e.GetPosition(this);
                    if (content.MinHeight != 0 && content.MinWidth != 0)
                    {
                        newItem.Width = content.MinWidth * 2; ;
                        newItem.Height = content.MinHeight * 2;
                    }
                    else
                    {
                        newItem.Width = 65;
                        newItem.Height = 65;
                    }
                    DesignerCanvas.SetLeft(newItem, Math.Max(0, position.X - newItem.Width / 2));
                    DesignerCanvas.SetTop(newItem, Math.Max(0, position.Y - newItem.Height / 2));

                    RoutedChildAddedEventArgs routedChildAddedEventArgs = new RoutedChildAddedEventArgs(ChildAddedEvent, newItem);
                    RaiseEvent(routedChildAddedEventArgs);

                    //this.Children.Add(newItem);

                    this.DeselectAll();
                    newItem.IsSelected = true;
                }

                e.Handled = false;
            }
        }
        public class RoutedChildAddedEventArgs : RoutedEventArgs
        {
            private readonly object _Child;
            public RoutedChildAddedEventArgs(object pChild)
                : base()
            {
                this._Child = pChild;
            }
            public RoutedChildAddedEventArgs(RoutedEvent routedEvent, object pChild)
                : base(routedEvent)
            {

                this._Child = pChild;
            }
            public RoutedChildAddedEventArgs(RoutedEvent routedEvent, object source, object pChild)
                : base(routedEvent, source)
            {

                this._Child = pChild;
            }
            public object Child { get { return this._Child; } }

        }
        /// <summary>
        /// Selected Items
        /// </summary>
        public IEnumerable<DesignerItem> SelectedItems
        {
            get
            {
                var selectedItems = from item in this.Children.OfType<DesignerItem>()
                                    where item.IsSelected == true
                                    select item;

                return selectedItems;
            }
        }
        /// <summary>
        /// Deselcts all Designer Items
        /// </summary>
        public void DeselectAll()
        {
            foreach (DesignerItem item in this.SelectedItems)
            {
                item.IsSelected = false;
            }
            //todo nur ein element darf immer focus haben!
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
        public event RoutedEventHandler ChildAdded
        {
            add
            {
                this.AddHandler(ChildAddedEvent, value);
            }
            remove
            {
                this.RemoveHandler(ChildAddedEvent, value);
            }
        }
    }
}
