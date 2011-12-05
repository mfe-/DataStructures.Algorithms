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
    public class VertexVisualization : Control
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
                Border = this.Template.FindName("PART_Border", this) as Border;
                if (Parent != null)
                {
                    UIElement uIElement = Parent as UIElement;
                    uIElement.GotFocus += (sender, eargs) =>
                    {
                        //Set Focus to our control when DesignerItem is IsSelected so that the GotFocus Event will be raised
                        Focus();

                    };
                }
            }
            this.GotFocus += new RoutedEventHandler(VertexVisualization_GotFocus);
        }

        void VertexVisualization_GotFocus(object sender, RoutedEventArgs e)
        {
            //EventTrigger g = (((this.Template as ControlTemplate).LoadContent() as Border).Triggers.First() as EventTrigger);
            //Storyboard s = (g.Actions.First() as BeginStoryboard).Storyboard;
            //ColorAnimation c = s.Children.First() as ColorAnimation;
            //this.BeginAnimation(BorderBrushProperty, c);
            //ColorAnimation colorAnimation = new ColorAnimation();
            //colorAnimation.Duration = new Duration(new TimeSpan(0, 0, 10));

            //colorAnimation.From = Colors.Red;
            //colorAnimation.To = Colors.Orange ;

            //this.BeginAnimation(Border.BorderBrushProperty, colorAnimation);

            //var g = this.GetAnimationBaseValue(BorderBrushProperty);

            //ColorAnimation ani = new ColorAnimation(Colors.Red, new Duration(new TimeSpan(0, 0, 0, 10, 0)));
            //ani.RepeatBehavior = RepeatBehavior.Forever;

            //Brush brush = Border.BorderBrush;

            //brush.BeginAnimation(SolidColorBrush.ColorProperty, ani);

            e.Handled = true;
        }
        static VertexVisualization()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata(typeof(VertexVisualization)));

            WidthProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata((double)40));
            HeightProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata((double)40));

            BorderThicknessProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata(new Thickness(2)));

            BorderBrushProperty.OverrideMetadata(typeof(VertexVisualization), new FrameworkPropertyMetadata(Brushes.Black));
        }

        public Get.Model.Graph.Vertex Vertex
        {
            get { return (Get.Model.Graph.Vertex)GetValue(VertexProperty); }
            set { SetValue(VertexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vertex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VertexProperty =
            DependencyProperty.Register("Vertex", typeof(Get.Model.Graph.Vertex), typeof(VertexVisualization), new UIPropertyMetadata());



    }
}
