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
    public class EdgeVisualization : Control, INotifyPropertyChanged
    {
        public EdgeVisualization()
        {
            // fired when layout changes
            base.LayoutUpdated += new EventHandler(EdgeVisualization_LayoutUpdated);
        }

        protected virtual void EdgeVisualization_LayoutUpdated(object sender, EventArgs e)
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
                var g = edgeVisualization.GetUIParentCore();

                var owner = FindVisualParent<UIElementCollection>(pDependencyObject); 


            }
        }
        /// <summary> 
        /// Finds a parent of a given item on the visual tree. 
        /// </summary> 
        /// <typeparam name="T">The type of the queried item.</typeparam> 
        /// <param name="child">A direct or indirect child of the queried item.</param> 
        /// <returns>The first parent item that matches the submitted type parameter.  
        /// If not matching item can be found, a null reference is being returned.</returns> 
        public static T FindVisualParent<T>(DependencyObject child)
          where T : DependencyObject
        {
            // get parent item 
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // we’ve reached the end of the tree 
            if (parentObject == null) return null;

            // check if the parent matches the type we’re looking for 
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                // use recursion to proceed with next level 
                return FindVisualParent<T>(parentObject);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
