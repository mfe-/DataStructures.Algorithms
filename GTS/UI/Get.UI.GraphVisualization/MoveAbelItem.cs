using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Get.UI
{
    public class MoveAbelItem : Thumb, INotifyPropertyChanged
    {
        protected Point _Position;

        public MoveAbelItem()
        {
            //Occurs one or more times as the mouse changes position when a Thumb control has logical focus and mouse capture. 
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        protected virtual void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _item = this.DataContext as Control;

            if (item != null)
            {
                double _Left = Canvas.GetLeft(item);
                double _Top = Canvas.GetTop(item);

                Canvas.SetLeft(item, _Left + e.HorizontalChange);
                Canvas.SetTop(item, _Top + e.VerticalChange);
                NotifyPropertyChanged("Position");
                
            }
        }
        protected Control _item;
        protected Control item
        {
            get
            {
                return _item;
            }
        }

        public virtual Point Position
        {
            get
            {
                if (item == null) return new Point();
                Point position = new Point(Canvas.GetLeft(item), Canvas.GetTop(item));
                Debug.WriteLine("x" + _Position.X + " " + "y " + _Position.Y);
                return position;
            }
            set
            {
                value = _Position;
                NotifyPropertyChanged("Position");
                Debug.WriteLine(Position);
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
