using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Markup;
using System.Windows.Input;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml", "Get.Common")]
namespace Get.Common
{
    public static class HyperlinkUtility
    {
        public static bool GetAddHyperLink(DependencyObject obj)
        {
            return (bool)obj.GetValue(AddHyperLinkProperty);
        }

        public static void SetAddHyperLink(DependencyObject obj, bool value)
        {
            obj.SetValue(AddHyperLinkProperty, value);
        }

        // Using a DependencyProperty as the backing store for AddHyperLink.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddHyperLinkProperty =
            DependencyProperty.RegisterAttached("AddHyperLink", typeof(bool), typeof(HyperlinkUtility), new UIPropertyMetadata(false, OnAddHyperLinkCahnged));

        private static void OnAddHyperLinkCahnged(DependencyObject pDependencyObject, DependencyPropertyChangedEventArgs pe)
        {
            FrameworkElement frameworkElement = pDependencyObject as FrameworkElement;
            frameworkElement.AddHandler(Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(Hyperlink_RequestNavigateEvent));

            frameworkElement.MouseMove += (sender, e) =>
            {
                if ((sender as FrameworkElement).IsMouseOver)
                    frameworkElement.Cursor = Cursors.Hand;
            };

        }
        private static void Hyperlink_RequestNavigateEvent(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());

            e.Handled = true;
        }


    }

}
