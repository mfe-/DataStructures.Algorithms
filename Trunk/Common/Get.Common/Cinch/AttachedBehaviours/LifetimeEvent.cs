using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

/// <summary>
/// This allows a Windows lifecycle events to call ICommand(s)
/// within a ViewModel. This allows the ViewModel to know something
/// about the Views lifecycle without the need for a strong link
/// to the actual View
/// </summary>
namespace Get.Common.Cinch
{
    /// <summary>
    /// This class is used to attach the Window lifetime events to ICommand implementations.
    /// It allows a ViewModel to hook into the lifetime of the view (when necessary) 
    /// through simple XAML tags.  Supported events are Loaded, Unloaded, Activated, Deactivated 
    /// and Closing/Closed.  For the Closing/Closed event, the CanExecute handler is invoked
    /// in response to the Closing event - if it returns true, then the Closed event is 
    /// allowed and the Execute handler is called in response.
    /// </summary>
    /// <example>
    /// <![CDATA[  <Window Cinch:LifetimeEvent.Close="{Binding CloseCommand}" />  ]]>
    /// </example>
    public static class LifetimeEvent
    {
        #region Loaded
        /// <summary>
        /// Dependency property which holds the ICommand for the Loaded event
        /// </summary>
        public static readonly DependencyProperty LoadedProperty =
            DependencyProperty.RegisterAttached("Loaded", 
                typeof(ICommand), typeof(LifetimeEvent),
                    new UIPropertyMetadata(null, OnLoadedEventInfoChanged));

        /// <summary>
        /// Attached Property getter to retrieve the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <returns>ICommand</returns>
        public static ICommand GetLoaded(DependencyObject source)
        {
            return (ICommand)source.GetValue(LoadedProperty);
        }

        /// <summary>
        /// Attached Property setter to change the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="command">ICommand</param>
        public static void SetLoaded(DependencyObject source, ICommand command)
        {
            source.SetValue(LoadedProperty, command);
        }

        /// <summary>
        /// This is the property changed handler for the Loaded property.
        /// </summary>
        /// <param name="sender">Dependency Object</param>
        /// <param name="e">EventArgs</param>
        private static void OnLoadedEventInfoChanged(DependencyObject sender, 
            DependencyPropertyChangedEventArgs e)
        {
            var win = sender as Window;
            if (win != null)
            {
                win.Loaded -= OnWindowLoaded;
                if (e.NewValue != null)
                {
                    win.Loaded += OnWindowLoaded;
                    // Workaround: depending on the properties of the element, it's possible the Loaded event was already raised
                    // This happens when the View is created before the ViewModel is applied to the DataContext.  In this
                    // case, raise the Loaded event as soon as we know about it.
                    if (win.IsLoaded)
                        OnWindowLoaded(sender, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// This is the handler for the Loaded event to raise the command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWindowLoaded(object sender, EventArgs e)
        {
            var dpo = (DependencyObject)sender;
            ICommand loadedCommand = GetLoaded(dpo);
            if (loadedCommand != null)
                loadedCommand.Execute(GetCommandParameter(dpo));
        }
        #endregion

        #region Activated
        /// <summary>
        /// Dependency property which holds the ICommand for the Activated event
        /// </summary>
        public static readonly DependencyProperty ActivatedProperty =
            DependencyProperty.RegisterAttached("Activated", 
            typeof(ICommand), typeof(LifetimeEvent),
                new UIPropertyMetadata(null, OnActivatedEventInfoChanged));

        /// <summary>
        /// Attached Property getter to retrieve the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <returns>ICommand</returns>
        public static ICommand GetActivated(DependencyObject source)
        {
            return (ICommand)source.GetValue(ActivatedProperty);
        }

        /// <summary>
        /// Attached Property setter to change the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="command">ICommand</param>
        public static void SetActivated(DependencyObject source, ICommand command)
        {
            source.SetValue(ActivatedProperty, command);
        }

        /// <summary>
        /// This is the property changed handler for the Activated property.
        /// </summary>
        /// <param name="sender">Dependency Object</param>
        /// <param name="e">EventArgs</param>
        private static void OnActivatedEventInfoChanged(DependencyObject sender, 
            DependencyPropertyChangedEventArgs e)
        {
            var win = sender as Window;
            if (win != null)
            {
                win.Activated -= OnWindowActivated;
                if (e.NewValue != null)
                    win.Activated += OnWindowActivated;
            }
        }

        /// <summary>
        /// This is the handler for the Activated event to raise the command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWindowActivated(object sender, EventArgs e)
        {
            var dpo = (DependencyObject)sender;
            ICommand activatedCommand = GetActivated(dpo);
            if (activatedCommand != null)
                activatedCommand.Execute(GetCommandParameter(dpo));
        }
        #endregion

        #region Deactivated
        /// <summary>
        /// Dependency property which holds the ICommand for the Deactivated event
        /// </summary>
        public static readonly DependencyProperty DeactivatedProperty =
            DependencyProperty.RegisterAttached("Deactivated", 
                typeof(ICommand), typeof(LifetimeEvent),
                    new UIPropertyMetadata(null, OnDeactivatedEventInfoChanged));

        /// <summary>
        /// Attached Property getter to retrieve the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <returns>ICommand</returns>
        public static ICommand GetDeactivated(DependencyObject source)
        {
            return (ICommand)source.GetValue(DeactivatedProperty);
        }

        /// <summary>
        /// Attached Property setter to change the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="command">ICommand</param>
        public static void SetDeactivated(DependencyObject source, ICommand command)
        {
            source.SetValue(DeactivatedProperty, command);
        }

        /// <summary>
        /// This is the property changed handler for the Deactivated property.
        /// </summary>
        /// <param name="sender">Dependency Object</param>
        /// <param name="e">EventArgs</param>
        private static void OnDeactivatedEventInfoChanged(DependencyObject sender, 
            DependencyPropertyChangedEventArgs e)
        {
            var win = sender as Window;
            if (win != null)
            {
                win.Deactivated -= OnWindowDeactivated;
                if (e.NewValue != null)
                    win.Deactivated += OnWindowDeactivated;
            }
        }

        /// <summary>
        /// This is the handler for the Deactivated event to raise the command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWindowDeactivated(object sender, EventArgs e)
        {
            var dpo = (DependencyObject)sender;
            ICommand deactivatedCommand = GetDeactivated(dpo);
            if (deactivatedCommand != null)
                deactivatedCommand.Execute(GetCommandParameter(dpo));
        }
        #endregion

        #region Close
        /// <summary>
        /// Dependency property which holds the ICommand for the Close event
        /// </summary>
        public static readonly DependencyProperty CloseProperty =
            DependencyProperty.RegisterAttached("Close", 
                typeof(ICommand), typeof(LifetimeEvent),
                    new UIPropertyMetadata(null, OnCloseEventInfoChanged));

        /// <summary>
        /// Attached Property getter to retrieve the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <returns>ICommand</returns>
        public static ICommand GetClose(DependencyObject source)
        {
            return (ICommand)source.GetValue(CloseProperty);
        }

        /// <summary>
        /// Attached Property setter to change the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="command">ICommand</param>
        public static void SetClose(DependencyObject source, ICommand command)
        {
            source.SetValue(CloseProperty, command);
        }

        /// <summary>
        /// This is the property changed handler for the Close property.
        /// </summary>
        /// <param name="sender">Dependency Object</param>
        /// <param name="e">EventArgs</param>
        private static void OnCloseEventInfoChanged(DependencyObject sender, 
            DependencyPropertyChangedEventArgs e)
        {
            var win = sender as Window;
            if (win != null)
            {
                win.Closing -= OnWindowClosing;
                win.Closed -= OnWindowClosed;

                if (e.NewValue != null)
                {
                    win.Closing += OnWindowClosing;
                    win.Closed += OnWindowClosed;
                }
            }
        }

        /// <summary>
        /// This method is invoked when the Window.Closing event is raised.  
        /// It checks with the ICommand.CanExecute handler
        /// and cancels the event if the handler returns false.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWindowClosing(object sender, CancelEventArgs e)
        {
            var dpo = (DependencyObject)sender;
            ICommand ic = GetClose(dpo);
            if (ic != null)
                e.Cancel = !ic.CanExecute(GetCommandParameter(dpo));
        }

        /// <summary>
        /// This method is invoked when the Window.Closed event is raised.  
        /// It executes the ICommand.Execute handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnWindowClosed(object sender, EventArgs e)
        {
            var dpo = (DependencyObject)sender;
            ICommand ic = GetClose(dpo);
            if (ic != null)
                ic.Execute(GetCommandParameter(dpo));
        }
        #endregion

        #region Unloaded
        /// <summary>
        /// Dependency property which holds the ICommand for the Unloaded event
        /// </summary>
        public static readonly DependencyProperty UnloadedProperty =
            DependencyProperty.RegisterAttached("Unloaded",
                typeof(ICommand), typeof(LifetimeEvent),
                    new UIPropertyMetadata(null, OnUnloadedEventInfoChanged));

        /// <summary>
        /// Attached Property getter to retrieve the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <returns>ICommand</returns>
        public static ICommand GetUnloaded(DependencyObject source)
        {
            return (ICommand)source.GetValue(UnloadedProperty);
        }

        /// <summary>
        /// Attached Property setter to change the ICommand
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="command">ICommand</param>
        public static void SetUnloaded(DependencyObject source, ICommand command)
        {
            source.SetValue(UnloadedProperty, command);
        }

        /// <summary>
        /// This is the property changed handler for the Unloaded property.
        /// </summary>
        /// <param name="sender">Dependency Object</param>
        /// <param name="e">EventArgs</param>
        private static void OnUnloadedEventInfoChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var win = sender as Window;

            if (win != null)
            {
                win.Unloaded -= OnWindowUnloaded;
                if (e.NewValue != null)
                    win.Unloaded += OnWindowUnloaded;
            }
        }

        /// <summary>
        /// This is the handler for the Unloaded event to raise the command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWindowUnloaded(object sender, EventArgs e)
        {
            var dpo = (DependencyObject)sender;
            ICommand deactivatedCommand = GetUnloaded(dpo);
            if (deactivatedCommand != null)
                deactivatedCommand.Execute(GetCommandParameter(dpo));
        }
        #endregion

        #region CommandParameter
        /// <summary>
        /// Parameter for the ICommand
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = 
            DependencyProperty.RegisterAttached(
            "CommandParameter", typeof(object), typeof(LifetimeEvent),
                new UIPropertyMetadata(null));

        /// <summary>
        /// This retrieves the CommandParameter used for the command.
        /// </summary>
        /// <param name="source">Dependency object</param>
        /// <returns>Command Parameter passed to ICommand</returns>
        public static object GetCommandParameter(DependencyObject source)
        {
            return source.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// This changes the CommandParameter used with this command.
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="value">New Value</param>
        public static void SetCommandParameter(DependencyObject source, object value)
        {
            source.SetValue(CommandParameterProperty, value);
        }
        #endregion
    }
}
