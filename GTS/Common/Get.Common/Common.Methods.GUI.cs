using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml", "Get.Common")]
namespace Get.Common
{
    public sealed static class GUI
    {
        private static FrameworkElement GetFrameworkElementParent(FrameworkElement element, string name)
        {
            FrameworkElement parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
            if (parent != null)
            {
                if (parent.Name == name)
                {
                    return parent;
                }
                return GetFrameworkElementParent(parent, name);
            }
            return null;
        }
    }
}
