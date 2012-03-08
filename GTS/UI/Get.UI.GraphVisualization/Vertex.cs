using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Diagnostics;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Graph", "Get.UI")]
namespace Get.UI
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Get.UI.GraphVisualization.Vertex"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Get.UI.GraphVisualization.Vertex;assembly=Get.UI.GraphVisualization.Vertex"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///    <local:VertexVisualization>
    ///       <graph:Vertex Weighted="2"></graph:Vertex>
    ///    </local:VertexVisualization>
    ///
    /// </summary>
    [ContentProperty("Vertex")]
    [TemplatePartAttribute(Name = "PART_Border", Type = typeof(Border))]
    public class VertexVisualization : Control, INotifyPropertyChanged
    {
        protected Border Border { get; set; }
        public VertexVisualization()
        {
            
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template != null)
            {
                //Border = this.Template.FindName("PART_Border", this) as Border;
                //if (Parent != null)
                //{
                //    UIElement uIElement = Parent as UIElement;
                //    uIElement.GotFocus += (sender, eargs) =>
                //    {
                //        //Set Focus to our control when DesignerItem is IsSelected so that the GotFocus Event will be raised
                //        Focus();

                //    };
                //}
                
            }

        }
        static VertexVisualization()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata(typeof(VertexVisualization)));

            WidthProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata((double)40));
            HeightProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata((double)40));

            BorderThicknessProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata(new Thickness(1)));

            BorderBrushProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata(Brushes.Black));
            BackgroundProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata(Brushes.White));
        }

        public Get.Model.Graph.Vertex Vertex
        {
            get { return (Get.Model.Graph.Vertex)GetValue(VertexProperty); }
            set { SetValue(VertexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vertex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VertexProperty =
            DependencyProperty.Register("Vertex", typeof(Get.Model.Graph.Vertex), typeof(VertexVisualization), new UIPropertyMetadata());

        private Point _Position;
        public virtual Point Position
        {
            get { return _Position; }
            set
            {
                if (_Position != value)
                {
                    _Position = value;
                    NotifyPropertyChanged("Position");
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
