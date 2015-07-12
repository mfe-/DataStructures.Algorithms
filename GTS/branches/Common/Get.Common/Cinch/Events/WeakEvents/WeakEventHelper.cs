/// All code in this source file is based on the article located at
/// http://diditwith.net/PermaLink,guid,aacdb8ae-7baa-4423-a953-c18c1c7940ab.aspx 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Get.Common.Cinch
{

    #region Types

    public delegate void UnregisterCallback<E>(EventHandler<E> eventHandler)
      where E : EventArgs;
    
    public interface IWeakEventHandler<E> where E : EventArgs
    {
        EventHandler<E> Handler { get; }
    }
    #endregion

    /// <summary>
    /// Provides methods for creating WeakEvent handlers
    /// </summary>
    /// <typeparam name="T">The type of the event source</typeparam>
    /// <typeparam name="E">The EventArgs</typeparam>
    public class WeakEventHandler<T, E> : IWeakEventHandler<E>
        where T : class
        where E : EventArgs
    {
        #region Data
        private delegate void OpenEventHandler(T @this, object sender, E e);
        private WeakReference m_TargetRef;
        private OpenEventHandler m_OpenHandler;
        private EventHandler<E> m_Handler;
        private UnregisterCallback<E> m_Unregister;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructs a new WeakEventHandler
        /// </summary>
        /// <param name="eventHandler">The Event handler</param>
        /// <param name="unregister">Unregister delegate</param>
        public WeakEventHandler(EventHandler<E> eventHandler, UnregisterCallback<E> unregister)
        {
            m_TargetRef = new WeakReference(eventHandler.Target);
            m_OpenHandler = (OpenEventHandler)Delegate.CreateDelegate(typeof(OpenEventHandler),
              null, eventHandler.Method);
            m_Handler = Invoke;
            m_Unregister = unregister;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Invokes the event handler if the source is still alive
        /// </summary>
        public void Invoke(object sender, E e)
        {
            T target = (T)m_TargetRef.Target;

            if (target != null)
                m_OpenHandler.Invoke(target, sender, e);
            else if (m_Unregister != null)
            {
                m_Unregister(m_Handler);
                m_Unregister = null;
            }
        }

        public EventHandler<E> Handler
        {
            get { return m_Handler; }
        }

        public static implicit operator EventHandler<E>(WeakEventHandler<T, E> weh)
        {
            return weh.m_Handler;
        }
        #endregion
    }
    

    /// <summary>
    /// Provides extension method for EventHandler&lt;E&gt;
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    ///    //SO DECLARE LISTENERS LIKE
    ///    workspace.CloseWorkSpace +=
    ///        new EventHandler<EventArgs>(OnCloseWorkSpace).
    ///           MakeWeak(eh => workspace.CloseWorkSpace -= eh);
    ///           
    ///    private void OnCloseWorkSpace(object sender, EventArgs e)
    ///    {
    ///
    ///    }
    ///    
    ///    //OR YOU COULD CREATE ACTUAL EVENTS LIKE
    ///    public class EventProvider
    ///    {
    ///         private EventHandler<EventArgs> closeWorkSpace;
    ///         public event EventHandler<EventArgs> CloseWorkSpace
    ///         {
    ///             add
    ///             {
    ///                 closeWorkSpace += value.MakeWeak(eh => closeWorkSpace -= eh);
    ///             }
    ///             remove
    ///             {
    ///             }
    ///         }
    ///    }
    /// ]]>
    /// </example>
    public static class EventHandlerUtils
    {
        #region EventHandler<E> extensions
        /// <summary>
        /// Sxtesion method for EventHandler<E>
        /// </summary>
        /// <typeparam name="E">The type</typeparam>
        /// <param name="eventHandler">The EventHandler</param>
        /// <param name="unregister">EventHandler unregister delegate</param>
        /// <returns>An EventHandler</returns>
        public static EventHandler<E> MakeWeak<E>(this EventHandler<E> eventHandler,
            UnregisterCallback<E> unregister) where E : EventArgs
        {
            if (eventHandler == null)
                throw new ArgumentNullException("eventHandler");

            if (eventHandler.Method.IsStatic || eventHandler.Target == null)
                throw new ArgumentException("Only instance methods are supported.", "eventHandler");

            Type wehType = typeof(WeakEventHandler<,>).MakeGenericType(
                eventHandler.Method.DeclaringType, typeof(E));

            ConstructorInfo wehConstructor =
                wehType.GetConstructor(new Type[] { typeof(EventHandler<E>),
                    typeof(UnregisterCallback<E>) });

            IWeakEventHandler<E> weh = (IWeakEventHandler<E>)wehConstructor.Invoke(
              new object[] { eventHandler, unregister });

            return weh.Handler;
        }
        #endregion
    }
}
