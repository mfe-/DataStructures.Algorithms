using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Get.Common.Cinch
{
    /// <summary>
    /// This class provides an ObservableCollection which supports the 
    /// Dispatcher thread marshalling for added items. 
    /// 
    /// This class does not take support any thread sycnhronization of
    /// adding items using multiple threads, that level of thread synchronization
    /// is left to the user. This class simply marshalls the CollectionChanged
    /// call to the correct Dispatcher thread
    /// </summary>
    /// <typeparam name="T">Type this collection holds</typeparam>
    public class DispatcherNotifiedObservableCollection<T> : ObservableCollection<T>
    {
        #region Ctors

        public DispatcherNotifiedObservableCollection()
            : base()
        {
        }

        public DispatcherNotifiedObservableCollection(List<T> list)
            : base(list)
        {
        }

        public DispatcherNotifiedObservableCollection(IEnumerable<T> collection) 
            : base(collection)
        {

        }
        #endregion

        #region Overrides
        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, 
        /// or the entire list is refreshed.
        /// </summary>
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Raises the <see cref="E:System.Collections.ObjectModel.
        /// ObservableCollection`1.CollectionChanged"/> 
        /// event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var eh = CollectionChanged;
            if (eh != null)
            {
                Dispatcher dispatcher = 
                    (from NotifyCollectionChangedEventHandler nh in eh.GetInvocationList()
                     let dpo = nh.Target as DispatcherObject
                     where dpo != null
                     select dpo.Dispatcher).FirstOrDefault();

                if (dispatcher != null && dispatcher.CheckAccess() == false)
                {
                    dispatcher.Invoke(DispatcherPriority.DataBind, 
                        (Action)(() => OnCollectionChanged(e)));
                }
                else
                {
                    foreach (NotifyCollectionChangedEventHandler nh 
                            in eh.GetInvocationList())
                        nh.Invoke(this, e);
                }
            }
        }
        #endregion
    }
}
