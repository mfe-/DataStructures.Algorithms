using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Get.Common.Cinch
{
    /// <summary>
    /// This class provides a method to allow mutiple entries to be added
    /// to an ObservableCollection without the CollectionChanged event being fired.
    /// As this class also inherits from DispatcherNotifiedObservableCollection, it
    /// also supports the Dispatcher thread marshalling for added items. 
    /// 
    /// This class does not take support any thread sycnhronization of
    /// adding items using multiple threads, that level of thread synchronization
    /// is left to the user. This class simply marshalls the CollectionChanged
    /// call to the correct Dispatcher thread
    /// 
    /// This class was taken and subsequently modified from
    /// http://peteohanlon.wordpress.com/2008/10/22/bulk-loading-in-observablecollection/
    /// </summary>
    /// <typeparam name="T">Type this collection holds</typeparam>
    public class AddRangeObservableCollection<T> :
        DispatcherNotifiedObservableCollection<T>
    {
        #region Data
        private bool _suppressNotification = false;
        #endregion

        #region Ctors

        public AddRangeObservableCollection()
            : base()
        {
        }

        public AddRangeObservableCollection(List<T> list)
            : base(list)
        {
        }

        public AddRangeObservableCollection(IEnumerable<T> collection) 
            : base(collection)
        {

        }
        #endregion

        #region Overrides
        /// <summary>
        /// Only raise the OnCollectionChanged event if there 
        /// is currently no suppressed notification
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds a range of items to the Collection, without firing the
        /// CollectionChanged event
        /// </summary>
        /// <param name="list">The items to add</param>
        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _suppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }
            _suppressNotification = false;
            OnCollectionChanged(new
                NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion

    }
}
