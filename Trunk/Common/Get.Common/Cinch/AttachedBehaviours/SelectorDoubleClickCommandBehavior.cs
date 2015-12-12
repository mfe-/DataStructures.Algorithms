using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Controls;

namespace Get.Common.Cinch
{
    /// <summary>
    /// Simple helper class that encapsulates the 
    /// parameter that is passed to the ICommand
    /// </summary>
    internal class DoubleClickData
    {
        #region Data
        public Object DoubleClickedItem { get; set; }
        public MouseButtonEventArgs DoubleClickedEventArgs { get; set; }
        #endregion

        #region Ctor
        public DoubleClickData(Object doubleClickedItem,
            MouseButtonEventArgs doubleClickedEventArgs)
        {
            this.DoubleClickedItem = doubleClickedItem;
            this.DoubleClickedEventArgs = doubleClickedEventArgs;
        }
        #endregion
    }

    /// <summary>
    /// Selector MouseDoubleClick calling ViewModel ICommand Behavior
    /// The selected item and the MouseButtonEventArgs are passed to
    /// the bound ICommand as DoubleClickData object
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    ///<ListView ItemsSource="{Binding People}" 
    ///  IsSynchronizedWithCurrentItem="True"
    ///  Cinch:SelectorDoubleClickCommandBehavior.HandleDoubleClick="true"
    ///  Cinch:SelectorDoubleClickCommandBehavior.TheCommandToRun=
    ///          "{Binding Path=DoItCommand}" >
    ///
    ///    <ListView.View>
    ///        <GridView>
    ///            <GridViewColumn Header="FirstName"
    ///                    DisplayMemberBinding="{Binding FirstName}"
    ///                    Width="80" />
    ///            <GridViewColumn Header="LastName" 
    ///                    DisplayMemberBinding="{Binding LastName}"  
    ///                    Width="80"/>
    ///        </GridView>
    ///    </ListView.View>
    ///</ListView>
    /// 
    /// ]]>
    /// </example>
    public static class SelectorDoubleClickCommandBehavior
    {
        #region Attached DPs
        #region HandleDoubleClick

        /// <summary>
        /// HandleDoubleClick Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty HandleDoubleClickProperty =
            DependencyProperty.RegisterAttached("HandleDoubleClick", typeof(bool),
            typeof(SelectorDoubleClickCommandBehavior),
                new FrameworkPropertyMetadata(false,
                    new PropertyChangedCallback(OnHandleDoubleClickChanged)));

        /// <summary>
        /// Gets the HandleDoubleClick property.  
        /// </summary>
        public static bool GetHandleDoubleClick(DependencyObject d)
        {
            return (bool)d.GetValue(HandleDoubleClickProperty);
        }

        /// <summary>
        /// Sets the HandleDoubleClick property. 
        /// </summary>
        public static void SetHandleDoubleClick(DependencyObject d, bool value)
        {
            d.SetValue(HandleDoubleClickProperty, value);
        }

        /// <summary>
        /// Hooks up a weak event against the source Selectors MouseDoubleClick
        /// if the Selector has asked for the HandleDoubleClick to be handled
        /// 
        /// If the source Selector has expressed an interest in not having its
        /// MouseDoubleClick handled the internal reference
        /// </summary>
        private static void OnHandleDoubleClickChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Selector selector = d as Selector;


            if (selector != null)
            {
                if ((bool)e.NewValue)
                {
                    selector.MouseDoubleClick -= OnMouseDoubleClick;
                    selector.MouseDoubleClick += OnMouseDoubleClick;
                }
            }
        }
        #endregion

        #region TheCommandToRun

        /// <summary>
        /// TheCommandToRun : The actual ICommand to run
        /// </summary>
        public static readonly DependencyProperty TheCommandToRunProperty =
            DependencyProperty.RegisterAttached("TheCommandToRun",
                typeof(ICommand),
                typeof(SelectorDoubleClickCommandBehavior),
                new FrameworkPropertyMetadata((ICommand)null));

        /// <summary>
        /// Gets the TheCommandToRun property.  
        /// </summary>
        public static ICommand GetTheCommandToRun(DependencyObject d)
        {
            return (ICommand)d.GetValue(TheCommandToRunProperty);
        }

        /// <summary>
        /// Sets the TheCommandToRun property.  
        /// </summary>
        public static void SetTheCommandToRun(DependencyObject d, ICommand value)
        {
            d.SetValue(TheCommandToRunProperty, value);
        }
        #endregion
        #endregion

        #region Private Methods


        /// <summary>
        /// Handle Selector.MouseDoubleClick but will only fire the associated
        /// ViewModel command if the MouseDoubleClick occurred over an actual
        /// ItemsControl item. This is nessecary as if we are using a ListView 
        /// we may have clicked the headers which are not items, so do not want the
        /// associated ViewModel command to be run
        /// </summary>
        private static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Get the ItemsControl and then get the item, and check there
            //is an actual item, as if we are using a ListView we may have clicked the
            //headers which are not items
            ItemsControl listView = sender as ItemsControl;
            DependencyObject originalSender = e.OriginalSource as DependencyObject;
            if (listView == null || originalSender == null) return;

            DependencyObject container =
                ItemsControl.ContainerFromElement
                (sender as ItemsControl, e.OriginalSource as DependencyObject);

            if (container == null ||
                container == DependencyProperty.UnsetValue) return;

            // found a container, now find the item.
            object activatedItem =
                listView.ItemContainerGenerator.ItemFromContainer(container);

            if (activatedItem != null)
            {
                ICommand command = (ICommand)(sender as DependencyObject).
                    GetValue(TheCommandToRunProperty);

                if (command != null)
                {
                    DoubleClickData param = new DoubleClickData(activatedItem, e);

                    if (command.CanExecute(null))
                        command.Execute(param);
                }
            }
        }
        #endregion
    }

}
