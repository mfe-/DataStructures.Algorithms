using System;

/// <summary>
/// These events args are used to close the active popup window
/// that may have been opened using the WPF implementation of the
/// IUIVisualizerService
/// </summary>
namespace Get.Common.Cinch
{
    /// <summary>
    /// This is used to send result parameters to a CloseRequest
    /// </summary>
    public class CloseRequestEventArgs : EventArgs
    {
        #region Data
        ///<summary>
        /// Final result for ShowDialog
        ///</summary>
        public bool? Result { get; private set; }
        #endregion

        #region Ctor
        internal CloseRequestEventArgs(bool? result)
        {
            Result = result;
        }
        #endregion
    }
}
