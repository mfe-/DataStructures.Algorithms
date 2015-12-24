using System;

namespace Get.Common.Cinch
{
    /// <summary>
    /// An event handler wrapper used to create weak-reference event handlers, 
    /// so that event subscribers can be garbage collected without the event publisher 
    /// interfering. 
    /// </summary>
    /// <typeparam name="TEventArgs">The type of event arguments used in the event handler.
    /// </typeparam>
    /// <example>
    /// <![CDATA[
    /// 
    ///        You might have an event that is available like ncc.CollectionChanged
    /// 
    ///        //SO DECLARE LISTENERS LIKE
    ///        private EventHandler<NotifyCollectionChangedEventArgs> 
    ///         collectionChangeHandler;
    ///        private WeakEventProxy<NotifyCollectionChangedEventArgs> 
    ///         weakCollectionChangeListener;
    ///
    ///        //THEN WIRE IT UP LIKE
    ///        if (weakCollectionChangeListener == null)
    ///        {
    ///          collectionChangeHandler = OnCollectionChanged;
    ///          weakCollectionChangeListener = 
    ///             new WeakEventProxy<NotifyCollectionChangedEventArgs>(
    ///                 collectionChangeHandler);
    ///        }
    ///        ncc.CollectionChanged += weakCollectionChangeListener.Handler;
    ///        
    /// 
    ///        private void OnCollectionChanged(object sender, 
    ///         NotifyCollectionChangedEventArgs e)
    ///        {
    ///
    ///        }
    /// ]]>
    /// </example>
    public class WeakEventProxy<TEventArgs> : IDisposable
        where TEventArgs : EventArgs
    {
        #region Data
        private WeakReference callbackReference;
        private readonly object syncRoot = new object();
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventProxy&lt;TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public WeakEventProxy(EventHandler<TEventArgs> callback)
        {
            callbackReference = new WeakReference(callback, true);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used as the event handler which should be subscribed to source collections.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Handler(object sender, TEventArgs e)
        {
            //acquire callback, if any
            EventHandler<TEventArgs> callback;
            lock (syncRoot)
            {
                callback = callbackReference == null ? null : callbackReference.Target as EventHandler<TEventArgs>;
            }

            if (callback != null)
            {
                callback(sender, e);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            lock (syncRoot)
            {
                GC.SuppressFinalize(this);

                if (callbackReference != null)
                {
                    //test for null in case the reference was already cleared
                    callbackReference.Target = null;
                }

                callbackReference = null;
            }
        }
        #endregion
    }
}