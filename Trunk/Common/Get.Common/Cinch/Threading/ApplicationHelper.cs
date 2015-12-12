using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Security.Permissions;

namespace Get.Common.Cinch
{
    /// <summary>
    /// This class provides static helper methods for 
    /// working with the Dispatcher in WPF
    /// The following MSDN page is quite useful :
    /// http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatcher.pushframe.aspx
    /// </summary>
    public static class ApplicationHelper
    {
        #region DoEvents
        /// <summary>
        /// Forces the WPF message pump to process all enqueued messages
        /// that are above the input parameter DispatcherPriority.
        /// </summary>
        /// <param name="priority">The DispatcherPriority to use
        /// as the lowest level of messages to get processed</param>
        [SecurityPermissionAttribute(SecurityAction.Demand,
            Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents(DispatcherPriority priority)
        {
            DispatcherFrame frame = new DispatcherFrame();
            DispatcherOperation dispatcherOperation = 
                Dispatcher.CurrentDispatcher.BeginInvoke(priority, 
                    new DispatcherOperationCallback(ExitFrameOperation), frame);
            
            Dispatcher.PushFrame(frame);

            if (dispatcherOperation.Status != DispatcherOperationStatus.Completed)
            {
                dispatcherOperation.Abort();
            }
        }


        /// <summary>
        /// Forces the WPF message pump to process all enqueued messages
        /// that are DispatcherPriority.Background or above
        /// </summary>
        [SecurityPermissionAttribute(SecurityAction.Demand,
            Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            DoEvents(DispatcherPriority.Background);
        }


        /// <summary>
        /// Stops the dispatcher from continuing
        /// </summary>
        private static object ExitFrameOperation(object obj)
        {
            ((DispatcherFrame)obj).Continue = false;
            return null;
        }
        #endregion
    }
}
