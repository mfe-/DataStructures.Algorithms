using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Base", "Get.UI.Base")]
namespace Get.UI.Base
{
    /// <summary>
    /// 
    /// http://www.codeproject.com/KB/WPF/WPFDiagramDesigner_Part2.aspx?display=Print
    /// </summary>
    public class Toolbox : ItemsControl
    {
        private Size defaultItemSize = new Size(65, 65);
        public Size DefaultItemSize
        {
            get { return this.defaultItemSize; }
            set { this.defaultItemSize = value; }
        }
        /// <summary>
        /// Creates or identifies the element that is used to display the given item. 
        /// http://msdn.microsoft.com/en-us/library/microsoft.surface.presentation.controls.elementmenuitem.getcontainerforitemoverride.aspx
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ToolboxItem();
        }
        /// <summary>
        /// Gets a Boolean value that indicates if an item is (or is eligible to be) its own SurfaceListBoxItem object. 
        /// http://msdn.microsoft.com/en-us/library/microsoft.surface.presentation.controls.surfacelistbox.isitemitsowncontaineroverride.aspx
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ToolboxItem);
        }
    }

    public class ToolboxItem : ContentControl
    {
        private Point? dragStartPoint = null;

        static ToolboxItem()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolboxItem),
                   new FrameworkPropertyMetadata(typeof(ToolboxItem)));
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            this.dragStartPoint = new Point?(e.GetPosition(this));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                this.dragStartPoint = null;
            }
            if (this.dragStartPoint.HasValue)
            {
                Point position = e.GetPosition(this);
                if ((SystemParameters.MinimumHorizontalDragDistance <=
                     Math.Abs((double)(position.X - this.dragStartPoint.Value.X))) ||
                     (SystemParameters.MinimumVerticalDragDistance <=
                     Math.Abs((double)(position.Y - this.dragStartPoint.Value.Y))))
                {
                    string xamlString = XamlWriter.Save(this.Content);
                    DataObject dataObject = new DataObject("DESIGNER_ITEM", xamlString);

                    if (dataObject != null)
                    {
                        DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
                    }
                }
                e.Handled = true;
            }
        }
    }
}
