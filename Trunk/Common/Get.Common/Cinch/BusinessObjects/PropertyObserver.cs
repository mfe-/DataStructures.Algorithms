using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;

namespace Get.Common.Cinch
{
    /// <summary>
    /// Monitors the PropertyChanged event of an object that implements INotifyPropertyChanged,
    /// and executes callback methods (i.e. handlers) registered for properties of that object.
    /// 
    /// http://joshsmithonwpf.wordpress.com/2009/07/11/one-way-to-avoid-messy-propertychanged-event-handling/
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    ///    PropertyObserver<NumberViewModel> observer;
    ///    
    ///    public NumberChangeLogViewModel()
    ///    {
    ///        this.Number = new NumberViewModel();
    ///        this.ChangeLog = new ObservableCollection<string>();
    ///
    ///        observer =
    ///            new PropertyObserver<NumberViewModel>(this.Number)
    ///               .RegisterHandler(n => n.Value, n => Log("Value: " + n.Value))
    ///               .RegisterHandler(n => n.IsNegative, this.AppendIsNegative)
    ///               .RegisterHandler(n => n.IsEven, this.AppendIsEven);
    ///    }
    ///         
    ///    void AppendIsEven(NumberViewModel number)
    ///    {
    ///        if (number.IsEven)
    ///            this.Log("\tNumber is now even");
    ///        else
    ///            this.Log("\tNumber is now odd");
    ///    }
    ///
    /// void Log(string message)
    ///    {
    ///        this.ChangeLog.Add(message);
    ///        this.OnLogged();
    ///    }
    /// 
    /// ]]>
    /// </example>
    /// <typeparam name="TPropertySource">The type of object to monitor for property changes.</typeparam>
    public class PropertyObserver<TPropertySource> : IWeakEventListener
        where TPropertySource : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of PropertyObserver, which
        /// observes the 'propertySource' object for property changes.
        /// </summary>
        /// <param name="propertySource">The object to monitor for property changes.</param>
        public PropertyObserver(TPropertySource propertySource)
        {
            if (propertySource == null)
                throw new ArgumentNullException("propertySource");

            _propertySourceRef = new WeakReference(propertySource);
            _propertyNameToHandlerMap = new Dictionary<string, Action<TPropertySource>>();
        }

        #endregion // Constructor

        #region Public Methods

        #region RegisterHandler

        /// <summary>
        /// Registers a callback to be invoked when the PropertyChanged event has been raised for the specified property.
        /// </summary>
        /// <param name="expression">A lambda expression like 'n => n.PropertyName'.</param>
        /// <param name="handler">The callback to invoke when the property has changed.</param>
        /// <returns>The object on which this method was invoked, to allow for multiple invocations chained together.</returns>
        public PropertyObserver<TPropertySource> RegisterHandler(
            Expression<Func<TPropertySource, object>> expression,
            Action<TPropertySource> handler)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            string propertyName = this.GetPropertyName(expression);
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("'expression' did not provide a property name.");

            if (handler == null)
                throw new ArgumentNullException("handler");

            TPropertySource propertySource = this.GetPropertySource();
            if (propertySource != null)
            {
                _propertyNameToHandlerMap[propertyName] = handler;
                PropertyChangedEventManager.AddListener(propertySource, this, propertyName);
            }

            return this;
        }

        #endregion // RegisterHandler

        #region UnregisterHandler

        /// <summary>
        /// Removes the callback associated with the specified property.
        /// </summary>
        /// <param name="propertyName">A lambda expression like 'n => n.PropertyName'.</param>
        /// <returns>The object on which this method was invoked, to allow for multiple invocations chained together.</returns>
        public PropertyObserver<TPropertySource> UnregisterHandler(Expression<Func<TPropertySource, object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            string propertyName = this.GetPropertyName(expression);
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("'expression' did not provide a property name.");

            TPropertySource propertySource = this.GetPropertySource();
            if (propertySource != null)
            {
                if (_propertyNameToHandlerMap.ContainsKey(propertyName))
                {
                    _propertyNameToHandlerMap.Remove(propertyName);
                    PropertyChangedEventManager.RemoveListener(propertySource, this, propertyName);
                }
            }

            return this;
        }

        #endregion // UnregisterHandler

        #endregion // Public Methods

        #region Private Helpers

        #region GetPropertyName

        string GetPropertyName(Expression<Func<TPropertySource, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            Debug.Assert(memberExpression != null, "Please provide a lambda expression like 'n => n.PropertyName'");

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                return propertyInfo.Name;
            }

            return null;
        }

        #endregion // GetPropertyName

        #region GetPropertySource

        TPropertySource GetPropertySource()
        {
            try
            {
                return (TPropertySource)_propertySourceRef.Target;
            }
            catch
            {
                return default(TPropertySource);
            }
        }

        #endregion // GetPropertySource

        #endregion // Private Helpers

        #region Fields

        readonly Dictionary<string, Action<TPropertySource>> _propertyNameToHandlerMap;
        readonly WeakReference _propertySourceRef;

        #endregion // Fields

        #region IWeakEventListener Members

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(PropertyChangedEventManager))
            {
                string propertyName = ((PropertyChangedEventArgs)e).PropertyName;
                TPropertySource propertySource = (TPropertySource)sender;

                if (String.IsNullOrEmpty(propertyName))
                {
                    // When the property name is empty, all properties are considered to be invalidated.
                    // Iterate over a copy of the list of handlers, in case a handler is registered by a callback.
                    foreach (Action<TPropertySource> handler in _propertyNameToHandlerMap.Values.ToArray())
                        handler(propertySource);

                    return true;
                }
                else
                {
                    Action<TPropertySource> handler;
                    if (_propertyNameToHandlerMap.TryGetValue(propertyName, out handler))
                    {
                        handler(propertySource);
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}