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

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Controls", "Get.Controls")]
namespace Get.Controls
{
    /// <summary>
    /// Interaction logic for ObjectDiagrammControl.xaml
    /// </summary>
    public partial class ObjectDiagrammControl : UserControl
    {
        public ObjectDiagrammControl()
        {
            InitializeComponent();
        }
    }
}
