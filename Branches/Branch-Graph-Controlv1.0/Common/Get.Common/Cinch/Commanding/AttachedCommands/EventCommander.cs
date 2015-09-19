using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using System.Reflection;
using System.Diagnostics;

/// <summary>
/// Provides a collection of classes that can be used
/// to attached Command to WPF elements using RoutedEvents
/// </summary>
namespace Get.Common.Cinch
{
    #region EventParameters Class
    /// <summary>
    /// This is passed to the ICommand handler for the event
    /// </summary>
    public class EventParameters
    {
        #region Public Properties
        /// <summary>
        /// The sender of the handled event
        /// </summary>
        public object Sender { get; set; }
        /// <summary>
        /// The passed EventArgs for the event.
        /// </summary>
        public EventArgs EventArgs { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal EventParameters(object sender, EventArgs e)
        {
            Sender = sender;
            EventArgs = e;
        }
        #endregion
    }
    #endregion

    #region CommandEvent Class
    /// <summary>
    /// This represents a single event to command mapping.  
    /// It derives from Freezable in order to inherit context and support 
    /// element name bindings per Mike Hillberg blog post.
    /// </summary>
    public class CommandEvent : Freezable
    {
        #region DPs
        #region Command DP
        /// <summary>
        /// Command Property Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = 
            DependencyProperty.Register("Command", typeof (ICommand), 
            typeof (CommandEvent), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Command property. 
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion

        #region Event DP
        /// <summary>
        /// Event Dependency Property
        /// </summary>
        public static readonly DependencyProperty EventProperty =
            DependencyProperty.Register("Event", typeof(string), typeof(CommandEvent),
            new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the Event property.
        /// </summary>
        public string Event
        {
            get { return (string)GetValue(EventProperty); }
            set { SetValue(EventProperty, value); }
        }
        #endregion

        #region Argument DP
        /// <summary>
        /// Event Dependency Property
        /// </summary>
        public static readonly DependencyProperty ArgumentProperty =
            DependencyProperty.Register("Argument", typeof(string), typeof(CommandEvent),
            new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the Event property.
        /// </summary>
        public string Argument
        {
            get { return (string)GetValue(ArgumentProperty); }
            set { SetValue(ArgumentProperty, value); }
        }
        #endregion

        #region DataContext DP
        /// <summary>
        /// DataContext for any bindings applied to this CommandEvent
        /// </summary>
        public static readonly DependencyProperty DataContextProperty = 
            FrameworkElement.DataContextProperty.AddOwner(typeof(CommandEvent), 
            new FrameworkPropertyMetadata(null));
        #endregion
        #endregion

        #region Private / Internal Methods
        /// <summary>
        /// Wires up an event to the target
        /// </summary>
        /// <param name="target"></param>
        internal void Subscribe(object target)
        {
            if (target != null)
            {
                BindingOperations.SetBinding(
                    this, FrameworkElement.DataContextProperty,
                        new Binding("DataContext") {Source = target});

                EventInfo ei = target.GetType().GetEvent(Event, 
                    BindingFlags.Public | BindingFlags.Instance);
                if (ei != null)
                {
                    ei.RemoveEventHandler(target, GetEventMethod(ei));
                    ei.AddEventHandler(target, GetEventMethod(ei));
                }
            }
        }

        /// <summary>
        /// Unwires target event
        /// </summary>
        /// <param name="target"></param>
        internal void Unsubscribe(object target)
        {
            if (target != null)
            {
                EventInfo ei = target.GetType().GetEvent(Event, 
                    BindingFlags.Public | BindingFlags.Instance);
                if (ei != null)
                    ei.RemoveEventHandler(target, GetEventMethod(ei));
            }
        }

        private Delegate _method;
        private Delegate GetEventMethod(EventInfo ei)
        {
            if (ei == null)
                throw new ArgumentNullException("ei");
            if (ei.EventHandlerType == null)
                throw new ArgumentException("EventHandlerType is null");
            if (_method == null)
                _method = Delegate.CreateDelegate(
                    ei.EventHandlerType, this,
                    GetType().GetMethod("OnEventRaised",
                    BindingFlags.NonPublic | BindingFlags.Instance));

            return _method;
        }

        /// <summary>
        /// This is invoked by the event - it invokes the command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventRaised(object sender, EventArgs e)
        {
            if (Command != null)
            {
                var ep = new EventParameters(sender, e);
                if (Command.CanExecute(ep))
                {
                    if(Argument == null)
                        Command.Execute(ep);
                    else
                        Command.Execute(Argument);
                }
            }
#if DEBUG
            else
            {
                Debug.WriteLine(string.Format(
                    "Missing Command on event handler, {0}: Sender={1}, EventArgs={2}", 
                    Event, sender, e));
            }
#endif
        }
        #endregion

        #region Overrides
        /// <summary>
        /// When implemented in a derived class, creates a new instance of the <see cref="T:System.Windows.Freezable"/> derived class. 
        /// </summary>
        /// <returns>
        /// The new instance.
        /// </returns>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    #endregion

    #region CommandEventCollection Class
    /// <summary>
    /// Collection of command to event mappings
    /// </summary>
    public class CommandEventCollection : FreezableCollection<CommandEvent>
    {
        #region Data
        private object _target;
        private readonly List<CommandEvent> _currentList = new List<CommandEvent>();
        #endregion

        #region Ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public CommandEventCollection()
        {
            ((INotifyCollectionChanged)this).CollectionChanged += OnCollectionChanged;
        }
        #endregion

        #region Private/Internal Methods
        /// <summary>
        /// Wire up events to the target
        /// </summary>
        /// <param name="target"></param>
        internal void Subscribe(object target)
        {
            _target = target;
            foreach(var item in this)
                item.Subscribe(target);
        }

        /// <summary>
        /// Unwire all target events
        /// </summary>
        /// <param name="target"></param>
        internal void Unsubscribe(object target)
        {
            foreach (var item in this)
                item.Unsubscribe(target);
            _target = null;
        }

        /// <summary>
        /// This handles the collection change event - it then subscribes and unsubscribes events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                        OnItemAdded((CommandEvent)item);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                        OnItemRemoved((CommandEvent)item);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (var item in e.OldItems)
                        OnItemRemoved((CommandEvent)item);
                    foreach (var item in e.NewItems)
                        OnItemAdded((CommandEvent)item);
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _currentList.ForEach(i => i.Unsubscribe(_target));
                    _currentList.Clear();
                    foreach (var item in this)
                        OnItemAdded(item);
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// A new item has been added to the event list
        /// </summary>
        /// <param name="item"></param>
        private void OnItemAdded(CommandEvent item)
        {
            if (item != null && _target != null)
            {
                _currentList.Add(item);
                item.Subscribe(_target);                
            }
        }

        /// <summary>
        /// An item has been removed from the event list.
        /// </summary>
        /// <param name="item"></param>
        private void OnItemRemoved(CommandEvent item)
        {
            if (item != null && _target != null)
            {
                _currentList.Remove(item);
                item.Unsubscribe(_target);
            }
        }
        #endregion
    }
    #endregion

    #region EventCommander Class
    /// <summary>
    /// This class manages a collection of command to event mappings.  
    /// It is used to wire up View events to a ViewModel ICommand implementation.  
    /// Note that if it is lifetime events (Loaded, Activated, Closing, Closed, etc.)
    /// then you should use the LifetimeEvents behavior instead.  
    /// This is for other input events to be tied to the 
    /// ViewModel without codebehind.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    /// <Grid Background="WhiteSmoke">
    ///   <Behaviors:EventCommander.Mappings>
    ///      <Behaviors:CommandEvent 
    ///          Command="{Binding MouseEnterCommand}" 
    ///          Event="MouseEnter" />
    ///      <Behaviors:CommandEvent 
    ///          Command="{Binding MouseLeaveCommand}" 
    ///          Event="MouseLeave" />
    ///   </Behaviors:EventCommander.Mappings>
    /// </Grid>
    /// ]]>
    /// </example>
    public static class EventCommander
    {
        #region InternalMappings DP
        // Make it internal so WPF ignores the property and always uses the 
        //public getter/setter.  This is per John Gossman blog post - 07/2008.
        internal static readonly DependencyProperty MappingsProperty = 
            DependencyProperty.RegisterAttached("InternalMappings", 
                            typeof(CommandEventCollection), typeof(EventCommander),
                            new UIPropertyMetadata(null, OnMappingsChanged));

        /// <summary>
        /// Retrieves the mapping collection
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static CommandEventCollection InternalGetMappingCollection(
            DependencyObject obj)
        {
            var map = obj.GetValue(MappingsProperty) as CommandEventCollection;
            if (map == null)
            {
                map = new CommandEventCollection();
                SetMappings(obj, map);
            }
            return map;
        }

        /// <summary>
        /// This retrieves the mapping collection
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <returns>Mapping collection</returns>
        public static IList GetMappings(DependencyObject obj)
        {
            return InternalGetMappingCollection(obj);
        }

        /// <summary>
        /// This sets the mapping collection.
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <param name="value">Mapping collection</param>
        public static void SetMappings(DependencyObject obj, 
            CommandEventCollection value)
        {
            obj.SetValue(MappingsProperty, value);
        }

        /// <summary>
        /// This changes the event mapping
        /// </summary>
        /// <param name="target"></param>
        /// <param name="e"></param>
        private static void OnMappingsChanged(DependencyObject target, 
            DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                CommandEventCollection cec = e.OldValue as CommandEventCollection;
                if (cec != null)
                    cec.Unsubscribe(target);
            }
            if (e.NewValue != null)
            {
                CommandEventCollection cec = e.NewValue as CommandEventCollection;
                if (cec != null)
                    cec.Subscribe(target);
            }
        }
        #endregion
    }
    #endregion
}
