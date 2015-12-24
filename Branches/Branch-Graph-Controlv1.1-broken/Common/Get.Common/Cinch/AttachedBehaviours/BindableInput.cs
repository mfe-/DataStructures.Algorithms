using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Get.Common.Cinch
{
    /// <summary>
    /// Mouse Binding support
    /// </summary>
    public class MouseBinding : BindableInput
    {
        #region Overrides
        /// <summary>
        /// Gesture mapped to input
        /// </summary>
        [TypeConverter(typeof(MouseGestureConverter))]
        [ValueSerializer(typeof(MouseGestureValueSerializer))]
        public override InputGesture Gesture
        {
            get
            {
                return base.Gesture as MouseGesture;
            }
            set
            {
                if (value is MouseGesture)
                    base.Gesture = value;
            }
        }

        /// <summary>
        /// This method is used to match input bindings to remove them from the list.
        /// </summary>
        /// <param name="binding">Binding being matched</param>
        /// <returns>True if binding matches this object</returns>
        protected override bool IsInputBindingMatch(InputBinding binding)
        {
            var mb = binding as System.Windows.Input.MouseBinding;
            return (mb != null && mb.Gesture == binding.Gesture);
        }

        /// <summary>
        /// This method creates a WPF input binding from the Gesture in this object.
        /// </summary>
        /// <returns>WPF input binding.</returns>
        protected override InputBinding CreateInputBinding()
        {
            return new System.Windows.Input.MouseBinding(new CommandParameterRouter(this), (MouseGesture)Gesture);
        }
        #endregion
    }

    /// <summary>
    /// KeyBinding support
    /// </summary>
    public class KeyBinding : BindableInput
    {
        #region Data
        private Key _key = Key.None;
        private ModifierKeys _modKeys = ModifierKeys.None;
        #endregion

        #region Overrides
        /// <summary>
        /// Gesture mapped to input
        /// </summary>
        [TypeConverter(typeof(KeyGestureConverter))]
        [ValueSerializer(typeof(KeyGestureValueSerializer))]
        public override InputGesture Gesture
        {
            get
            {
                return base.Gesture as KeyGesture;
            }
            set
            {
                if (value is KeyGesture)
                    base.Gesture = value;
            }
        }

        /// <summary>
        /// This method is used to match input bindings to remove them from the list.
        /// </summary>
        /// <param name="binding">Binding being matched</param>
        /// <returns>True if binding matches this object</returns>
        protected override bool IsInputBindingMatch(InputBinding binding)
        {
            var kb = binding as System.Windows.Input.KeyBinding;
            return (kb != null && kb.Key == Key && kb.Modifiers == Modifiers);
        }

        /// <summary>
        /// This method creates a WPF input binding from the Gesture in this object.
        /// </summary>
        /// <returns>WPF input binding.</returns>
        protected override InputBinding CreateInputBinding()
        {
            if (Gesture == null)
                Gesture = new KeyGesture(_key, _modKeys);
            return new System.Windows.Input.KeyBinding(new CommandParameterRouter(this), (KeyGesture)Gesture);
        }
        #endregion

        #region Public Properties/Methods
        /// <summary>
        /// Modifier 
        /// </summary> 
        public ModifierKeys Modifiers
        {
            get
            {
                return null != Gesture ? ((KeyGesture)Gesture).Modifiers : _modKeys;
            }
            set 
            {
                _modKeys = value;
                if (Gesture != null)
                    Gesture = new KeyGesture(((KeyGesture) Gesture).Key, value);
            }
        }

        /// <summary>
        /// Key 
        /// </summary>
        public Key Key
        {
            get
            {
                return Gesture != null ? ((KeyGesture)Gesture).Key : _key;
            }

            set 
            {
                _key = value;
                if (Gesture != null)
                    Gesture = new KeyGesture(value, ((KeyGesture) Gesture).Modifiers);
            }
        }
        #endregion
    }

    ///<summary>
    /// This allows InputBinding based classes to have bindable 
    /// ICommand and CommandParameter elements.
    ///</summary>
    public abstract class BindableInput : Freezable
    {
        #region CommandParameterRouter Class
        /// <summary>
        /// This class wraps an existing ICommand implementation and 
        /// supplies the CommandParameter support.
        /// </summary>
        internal class CommandParameterRouter : ICommand
        {
            #region Data
            private readonly BindableInput _owner;
            #endregion

            #region Public Properties/Methods
            public CommandParameterRouter(BindableInput owner)
            {
                _owner = owner;
            }

            public event EventHandler CanExecuteChanged
            {
                add { _owner.Command.CanExecuteChanged += value; }
                remove { _owner.Command.CanExecuteChanged -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return _owner.Command.CanExecute(_owner.CommandParameter);
            }

            public void Execute(object parameter)
            {
                _owner.Command.Execute(_owner.CommandParameter);
            }
            #endregion
        }
        #endregion

        #region DPs

        #region Command
        /// <summary>
        /// ICommand implementation to bind to the input type.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = 
            DependencyProperty.Register("Command", typeof(ICommand), 
                typeof(BindableInput));

        /// <summary>
        /// Gets and sets the Command
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion

        #region CommandParameter
        /// <summary>
        /// Parameter for the ICommand
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = 
            DependencyProperty.Register("CommandParameter", typeof (object), 
                typeof (BindableInput));

        /// <summary>
        /// Gets and sets the CommandParameter
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        #endregion

        #region DataContext
        /// <summary>
        /// DataContext for any bindings applied to this CommandEvent
        /// </summary>
        public static readonly DependencyProperty DataContextProperty = 
            FrameworkElement.DataContextProperty.AddOwner(typeof(BindableInput), 
                new FrameworkPropertyMetadata(null));
        #endregion

        #endregion

        #region Virtual Methods
        /// <summary>
        /// Gesture mapped to input
        /// </summary>
        public virtual InputGesture Gesture { get; set; }
        #endregion

        #region Internal Methods
        internal void AddToInputBindings(object target)
        {
            var uie = target as UIElement;
            if (uie != null)
                uie.InputBindings.Add(CreateInputBinding());
        }

        internal void RemoveFromInputBindings(object target)
        {
            var uie = target as UIElement;
            if (uie != null)
            {
                for (int i = 0; i < uie.InputBindings.Count; i++)
                {
                    if (IsInputBindingMatch(uie.InputBindings[i]))
                    {
                        uie.InputBindings.RemoveAt(i);
                        break;
                    }
                }
            }

        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// This method is used to match input bindings to remove them from the list.
        /// </summary>
        /// <param name="binding">Binding being matched</param>
        /// <returns>True if binding matches this object</returns>
        protected abstract bool IsInputBindingMatch(InputBinding binding);

        /// <summary>
        /// This method creates a WPF input binding from the Gesture in this object.
        /// </summary>
        /// <returns>WPF input binding.</returns>
        protected abstract InputBinding CreateInputBinding();
        #endregion

        #region Overrides
        /// <summary>
        /// Required override - not implemented.
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// Collection of command to event mappings
    /// </summary>
    public class InputBindingCollection : FreezableCollection<BindableInput>
    {
        #region Data
        private object _target;
        private readonly List<BindableInput> _currentList = new List<BindableInput>();
        #endregion

        #region Ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public InputBindingCollection()
        {
            ((INotifyCollectionChanged)this).CollectionChanged += OnCollectionChanged;
        }
        #endregion

        #region Internal/Private Methods
        /// <summary>
        /// Wire up events to the target
        /// </summary>
        /// <param name="target"></param>
        internal void AddBindings(object target)
        {
            _target = target;
        }

        /// <summary>
        /// Unwire all target events
        /// </summary>
        /// <param name="target"></param>
        internal void RemoveBindings(object target)
        {
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
                        OnItemAdded((BindableInput)item);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                        OnItemRemoved((BindableInput)item);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (var item in e.OldItems)
                        OnItemRemoved((BindableInput)item);
                    foreach (var item in e.NewItems)
                        OnItemAdded((BindableInput)item);
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _currentList.ForEach(i => i.RemoveFromInputBindings(_target));
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
        private void OnItemAdded(BindableInput item)
        {
            if (item != null && _target != null)
            {
                _currentList.Add(item);
                item.AddToInputBindings(_target);
            }
        }

        /// <summary>
        /// An item has been removed from the event list.
        /// </summary>
        /// <param name="item"></param>
        private void OnItemRemoved(BindableInput item)
        {
            if (item != null && _target != null)
            {
                _currentList.Remove(item);
                item.RemoveFromInputBindings(_target);
            }
        }
        #endregion
    }

    /// <summary>
    /// This class replaces the traditional WPF InputBinding collection with a bindable
    /// collection that is usable in the MVVM pattern.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    /// <Cinch:InputBinder.Bindings>
    ///    <Cinch:KeyBinding Command="{Binding OnF3}" Key="F3" Modifiers="ALT" />
    ///    <Cinch:MouseBinding Command="{Binding OnWheelClick}" Gesture="Control+WheelClick" />
    /// </Cinch:InputBinder.Bindings>
    /// 
    /// ]]>
    /// </example>
    public static class InputBinder
    {
        #region DPs

        #region InputBindingCollection
        // Make it internal so WPF ignores the property and always uses the public getter/setter.  This is per
        // John Gossman blog post - 07/2008.
        internal static readonly DependencyProperty BindingsProperty = DependencyProperty.RegisterAttached("InternalBindings",
                            typeof(InputBindingCollection), typeof(InputBinder),
                            new UIPropertyMetadata(null, OnBindingsChanged));

        /// <summary>
        /// Retrieves the mapping collection
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static InputBindingCollection InternalGetBindingCollection(DependencyObject obj)
        {
            var bindings = obj.GetValue(BindingsProperty) as InputBindingCollection;
            if (bindings == null)
            {
                bindings = new InputBindingCollection();
                SetBindings(obj, bindings);
            }
            return bindings;
        }

        /// <summary>
        /// This retrieves the input binding collection
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <returns>Binding collection</returns>
        public static IList GetBindings(DependencyObject obj)
        {
            return InternalGetBindingCollection(obj);
        }

        /// <summary>
        /// This sets the mapping collection.
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <param name="value">Mapping collection</param>
        public static void SetBindings(DependencyObject obj, InputBindingCollection value)
        {
            obj.SetValue(BindingsProperty, value);
        }

        /// <summary>
        /// This changes the event mapping
        /// </summary>
        /// <param name="target"></param>
        /// <param name="e"></param>
        private static void OnBindingsChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                InputBindingCollection cec = e.OldValue as InputBindingCollection;
                if (cec != null)
                    cec.RemoveBindings(target);
            }
            if (e.NewValue != null)
            {
                InputBindingCollection cec = e.NewValue as InputBindingCollection;
                if (cec != null)
                    cec.AddBindings(target);
            }
        }
        #endregion

        #endregion
    }
}
