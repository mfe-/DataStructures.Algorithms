using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

/// <summary>
/// Allows a hour glass cursor to be shown.
/// This is more than likely specific to WPF
/// </summary>
namespace Get.Common.Cinch
{
    /// <summary>
    /// This class implements a disposable WaitCursor to 
    /// show an hourglass while some long-running event occurs.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    /// using (new WaitCursor())
    /// {
    ///    .. Do work here ..
    /// }
    /// 
    /// ]]>
    /// </example>
    public class WaitCursor : IDisposable
    {
        #region Data
        private readonly Cursor oldCursor;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public WaitCursor()
        {
            oldCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the cursor to the default state.
        /// </summary>
        public void Dispose()
        {
            Mouse.OverrideCursor = oldCursor;
        }
        #endregion
    }
}
