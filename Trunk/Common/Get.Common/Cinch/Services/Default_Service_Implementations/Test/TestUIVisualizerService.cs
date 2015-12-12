using System;
using System.Collections.Generic;


namespace Get.Common.Cinch
{
    /// <summary>
    /// This class implements the IUIVisualizerService for Unit testing purposes.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    ///    TestUIVisualizerService testUIVisualizerService =
    ///      (TestUIVisualizerService)
    ///        ViewModelBase.ServiceProvider.Resolve<IUIVisualizerService>();
    ///        
    ///    //Queue up the response we expect for our given TestUIVisualizerService
    ///    //for a given ICommand/Method call within the test ViewModel
    ///    testUIVisualizerService.ShowDialogResultResponders.Enqueue
    ///     (() =>
    ///        {
    ///            return true;
    ///        }
    ///     );
    /// ]]>
    /// </example>
    public class TestUIVisualizerService : IUIVisualizerService
    {
        #region Data

        /// <summary>
        /// Queue of callback delegates for the Show methods expected
        /// for the item under test
        /// </summary>
        public Queue<Func<bool>> ShowResultResponders { get; set; }

        /// <summary>
        /// Queue of callback delegates for the ShowDialog methods expected
        /// for the item under test
        /// </summary>
        public Queue<Func<bool>> ShowDialogResultResponders { get; set; }

        #endregion

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public TestUIVisualizerService()
        {
              ShowResultResponders = new Queue<Func<bool>>();
              ShowDialogResultResponders = new Queue<Func<bool>>();
        }
        #endregion

        #region IUIVisualizerService Members

        /// <summary>
        /// Does nothing, as nothing required for testing
        /// </summary>
        /// <param name="key">Key for the UI dialog</param>
        /// <param name="winType">Type which implements dialog</param>
        public void Register(string key, Type winType)
        {
            //Nothing to do, as there will never be a UI
            //as we are testing the VMs
        }


        /// <summary>
        /// Does nothing, as nothing required for testing
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>True/False success</returns>
        public bool Unregister(string key)
        {
            //Nothing to do, as there will never be a UI
            //as we are testing the VMs, simple return true
            return true;
        }

        /// <summary>
        /// Returns the next Dequeue Show response expected. See the tests for 
        /// the Func callback expected values
        /// </summary>
        /// <param name="key">Key previously registered with the UI controller.</param>
        /// <param name="state">Object state to associate with the dialog</param>
        /// <param name="setOwner">Set the owner of the window</param>
        /// <param name="completedProc">Callback used when UI closes (may be null)</param>
        /// <returns>True/False if UI is displayed</returns>
        public bool Show(string key, object state, bool setOwner, EventHandler<UICompletedEventArgs> completedProc)
        {
            if (ShowResultResponders.Count == 0)
                throw new ApplicationException(
                    "TestUIVisualizerService Show method expects a Func<bool> callback \r\n" +
                    "delegate to be enqueued for each Show call");
            else
            {
                Func<bool> responder = ShowResultResponders.Dequeue();
                return responder();
            }
        }

        /// <summary>
        /// Returns the next Dequeue ShowDialog response expected. See the tests for 
        /// the Func callback expected values
        /// </summary>
        /// <param name="key">Key previously registered with the UI controller.</param>
        /// <param name="state">Object state to associate with the dialog</param>
        /// <returns>True/False if UI is displayed.</returns>
        public bool? ShowDialog(string key, object state)
        {
            if (ShowDialogResultResponders.Count == 0)
                throw new ApplicationException(
                    "TestUIVisualizerService ShowDialog method expects a Func<bool?> callback \r\n" +
                    "delegate to be enqueued for each Show call");
            else
            {
                Func<bool> responder = ShowDialogResultResponders.Dequeue();
                return responder() as bool?;
            }
        }

        #endregion
    }
}
