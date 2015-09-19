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
using System.Windows.Controls;

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
    /// <summary>  
    /// This class contains a few useful extenders for the ListBox  
    /// ListBox – Automatically scroll CurrentItem into View
    /// http://michlg.wordpress.com/2010/01/16/listbox-automatically-scroll-currentitem-into-view/
    /// </summary>  
    public class ListBoxExtenders : DependencyObject
    {

        #region Properties

        public static readonly DependencyProperty AutoScrollToCurrentItemProperty = DependencyProperty.RegisterAttached("AutoScrollToCurrentItem", typeof(bool), typeof(ListBoxExtenders), new UIPropertyMetadata(default(bool), OnAutoScrollToCurrentItemChanged));

        /// <summary>  
        /// Returns the value of the AutoScrollToCurrentItemProperty  
        /// </summary>  
        /// <param name="obj">The dependency-object whichs value should be returned</param>  
        /// <returns>The value of the given property</returns>  
        public static bool GetAutoScrollToCurrentItem(DependencyObject obj)
        {

            return (bool)obj.GetValue(AutoScrollToCurrentItemProperty);

        }
        /// <summary>  
        /// Sets the value of the AutoScrollToCurrentItemProperty  
        /// </summary>  
        /// <param name="obj">The dependency-object whichs value should be set</param>  
        /// <param name="value">The value which should be assigned to the AutoScrollToCurrentItemProperty</param>  
        public static void SetAutoScrollToCurrentItem(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToCurrentItemProperty, value);
        }

        #endregion

        #region Events

        /// <summary>  
        /// This method will be called when the AutoScrollToCurrentItem  
        /// property was changed  
        /// </summary>  
        /// <param name="s">The sender (the ListBox)</param>  
        /// <param name="e">Some additional information</param>  
        public static void OnAutoScrollToCurrentItemChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {

            var listBox = s as ListBox;

            if (listBox != null)
            {

                var listBoxItems = listBox.Items;

                if (listBoxItems != null)
                {

                    var newValue = (bool)e.NewValue;

                    var autoScrollToCurrentItemWorker = new EventHandler((s1, e2) => OnAutoScrollToCurrentItem(listBox, listBox.Items.CurrentPosition));

                    if (newValue)

                        listBoxItems.CurrentChanged += autoScrollToCurrentItemWorker;

                    else

                        listBoxItems.CurrentChanged -= autoScrollToCurrentItemWorker;

                }

            }

        }

        /// <summary>  
        /// This method will be called when the ListBox should  
        /// be scrolled to the given index  
        /// </summary>  
        /// <param name="listBox">The ListBox which should be scrolled</param>  
        /// <param name="index">The index of the item to which it should be scrolled</param>  
        public static void OnAutoScrollToCurrentItem(ListBox listBox, int index)
        {

            if (listBox != null && listBox.Items != null && listBox.Items.Count > index && index >= 0)

                listBox.ScrollIntoView(listBox.Items[index]);

        }

        #endregion

    }

}
