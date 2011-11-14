using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Graph", "Get.UI")]
namespace Get.UI
{
    public class MoveAbelItem : Thumb, INotifyPropertyChanged
    {
        protected Control _item;
        protected Point _Position;

        public MoveAbelItem()
        {
            //Occurs one or more times as the mouse changes position when a Thumb control has logical focus and mouse capture. 
            Loaded += new RoutedEventHandler(MoveAbelItem_Loaded);
        }

        protected virtual void MoveAbelItem_Loaded(object sender, RoutedEventArgs e)
        {
            Position = getPositionInCanvas();

            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
            base.LayoutUpdated += new EventHandler(MoveAbelItem_LayoutUpdated);
        }
        protected virtual Point getPositionInCanvas()
        {
            if (Item != null) return new Point(Canvas.GetLeft(Item), Canvas.GetTop(Item));
            else return new Point();
        }
        protected virtual void MoveAbelItem_LayoutUpdated(object sender, EventArgs e)
        {
            Position = getPositionInCanvas();
            //get center position of this Connector relative to the DesignerCanvas
            //this.Position = this.TransformToAncestor(item).Transform
            //     (new Point(this.Width / 2, this.Height / 2));
            ////http://www.codeproject.com/KB/WPF/WPFDiagramDesigner_Part3.aspx

        }

        protected virtual void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Item != null)
            {
                double _Left = Canvas.GetLeft(Item);
                double _Top = Canvas.GetTop(Item);

                Canvas.SetLeft(Item, _Left + e.HorizontalChange);
                Canvas.SetTop(Item, _Top + e.VerticalChange);

            }
        }



        public Control Item
        {
            get
            {
                if (_item == null) _item = this.DataContext as Control;
                return _item;
            }
        }


        public virtual Point Position
        {
            get { return _Position; }
            set
            {
                if (_Position != value)
                {
                    _Position = value;
                    NotifyPropertyChanged("Position");
                    Debug.WriteLine(Position);
                }
            }
        }





        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify using String property name
        /// </summary>
        protected void NotifyPropertyChanged(String propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
#if !SILVERLIGHT
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;


            }
#endif
        }
    }
}
