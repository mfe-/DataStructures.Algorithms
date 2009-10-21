using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System.Windows.Media;
using Microsoft.Practices.Composite.Wpf.Commands;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml", "Get.Common")]
namespace Get.Common
{
    public static class CommandBehavior
    {
        #region TheCommandToRun

        /// <summary>
        /// TheCommandToRun : The actual DelegateCommand<object> to run
        /// </summary>
        public static readonly DependencyProperty TheCommandToRunProperty =
            DependencyProperty.RegisterAttached("TheCommandToRun",
                typeof(DelegateCommand<object>),
                typeof(CommandBehavior),
                new FrameworkPropertyMetadata((DelegateCommand<object>)null));

        /// <summary>
        /// Gets the TheCommandToRun property.  
        /// </summary>
        public static DelegateCommand<object> GetTheCommandToRun(DependencyObject d)
        {
            return (DelegateCommand<object>)d.GetValue(TheCommandToRunProperty);
        }

        /// <summary>
        /// Sets the TheCommandToRun property.  
        /// </summary>
        public static void SetTheCommandToRun(DependencyObject d, DelegateCommand<object> value)
        {
            d.SetValue(TheCommandToRunProperty, value);
        }
        #endregion

        #region RoutedEventName

        /// <summary>
        /// RoutedEventName : The event that should actually execute the
        /// DelegateCommand<object>
        /// </summary>
        public static readonly DependencyProperty RoutedEventNameProperty =
            DependencyProperty.RegisterAttached("RoutedEventName", typeof(String),
            typeof(CommandBehavior),
                new FrameworkPropertyMetadata((String)String.Empty,
                    new PropertyChangedCallback(OnRoutedEventNameChanged)));

        /// <summary>
        /// Gets the RoutedEventName property.  
        /// </summary>
        public static String GetRoutedEventName(DependencyObject d)
        {
            return (String)d.GetValue(RoutedEventNameProperty);
        }

        /// <summary>
        /// Sets the RoutedEventName property.  
        /// </summary>
        public static void SetRoutedEventName(DependencyObject d, String value)
        {
            d.SetValue(RoutedEventNameProperty, value);
        }

        /// <summary>
        /// Hooks up a Dynamically created EventHandler (by using the 
        /// <see cref="EventHooker">EventHooker</see> class) that when
        /// run will run the associated DelegateCommand<object>
        /// </summary>
        private static void OnRoutedEventNameChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            String routedEvent = (String)e.NewValue;

            //If the RoutedEvent string is not null, create a new
            //dynamically created EventHandler that when run will execute
            //the actual bound DelegateCommand<object> instance (usually in the ViewModel)
            if (!String.IsNullOrEmpty(routedEvent))
            {
                EventHooker eventHooker = new EventHooker();
                eventHooker.ObjectWithAttachedCommand = d;

                EventInfo eventInfo = d.GetType().GetEvent(routedEvent,
                    BindingFlags.Public | BindingFlags.Instance);

                //Hook up Dynamically created event handler
                if (eventInfo != null)
                {
                    eventInfo.AddEventHandler(d,
                        eventHooker.GetNewEventHandlerToRunCommand(eventInfo));
                }
            }
        }
        #endregion

        #region CommandParameter

        /// <summary>
        /// CommandParameter Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(CommandBehavior),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets the CommandParameter property.  
        /// </summary>
        public static object GetCommandParameter(DependencyObject d)
        {
            return (object)d.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// Sets the CommandParameter property. 
        /// </summary>
        public static void SetCommandParameter(DependencyObject d, object value)
        {
            d.SetValue(CommandParameterProperty, value);
        }

        #endregion

    }

    /// <summary>
    /// Contains the event that is hooked into the source RoutedEvent
    /// that was specified to run the DelegateCommand<object>
    /// </summary>
    sealed class EventHooker
    {
        #region Public Methods/Properties
        /// <summary>
        /// The DependencyObject, that holds a binding to the actual
        /// DelegateCommand<object> to execute
        /// </summary>
        public DependencyObject ObjectWithAttachedCommand { get; set; }

        /// <summary>
        /// Creates a Dynamic EventHandler that will be run the DelegateCommand<object>
        /// when the user specified RoutedEvent fires
        /// </summary>
        /// <param name="eventInfo">The specified RoutedEvent EventInfo</param>
        /// <returns>An Delegate that points to a new EventHandler
        /// that will be run the DelegateCommand<object></returns>
        public Delegate GetNewEventHandlerToRunCommand(EventInfo eventInfo)
        {
            Delegate del = null;

            if (eventInfo == null)
                throw new ArgumentNullException("eventInfo");

            if (eventInfo.EventHandlerType == null)
                throw new ArgumentException("EventHandlerType is null");

            if (del == null)
                del = Delegate.CreateDelegate(eventInfo.EventHandlerType, this,
                      GetType().GetMethod("OnEventRaised",
                        BindingFlags.NonPublic |
                        BindingFlags.Instance));

            return del;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Runs the DelegateCommand<object> when the requested RoutedEvent fires
        /// </summary>
        private void OnEventRaised(object sender, EventArgs e)
        {
            DelegateCommand<object> command = (DelegateCommand<object>)(sender as DependencyObject).GetValue(CommandBehavior.TheCommandToRunProperty);

            object param = (sender as DependencyObject).GetValue(CommandBehavior.CommandParameterProperty);

            if (command != null)
            {
                command.Execute(param);
            }
        }

        #endregion
    }

}
