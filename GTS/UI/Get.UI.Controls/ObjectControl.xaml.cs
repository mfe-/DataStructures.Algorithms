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
using System.ComponentModel;
using Get.Model.Core;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Controls", "Get.UI.Controls")]
namespace Get.UI.Controls
{


    //http://www.c-sharpcorner.com/uploadfile/dpatra/making-timeline-control-for-datagrid-in-wpf/
    //http://www.codeproject.com/Articles/72670/WPF-A-TimeLineControl
    //http://sachabarber.net/?p=716
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// http://msdn.microsoft.com/de-de/library/system.windows.markup.contentpropertyattribute.aspx
    /// </summary>
    [ContentProperty("ItemsSource")]
    [LocalizabilityAttribute(LocalizationCategory.None, Readability = Readability.Unreadable)]
    //[StyleTypedPropertyAttribute(Property = "ItemContainerStyle", StyleTargetType = typeof(FrameworkElement))]
    public partial class ObjectControl : UserControl
    {
        public ObjectControl()
        {
            InitializeComponent();
        }

        [BindableAttribute(true)]
        public ObjectCore ItemsSource
        {
            get { return (ObjectCore)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObjectCore), typeof(ObjectControl), new UIPropertyMetadata(null));


    }
}
