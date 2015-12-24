using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Get.Common.Cinch
{
    /// <summary>
    /// This class forces focus to set on the specified UIElement 
    /// </summary>
    public class FocusHelper
    {
        #region Public Methods
        /// <summary>
        /// Set focus to UIElement
        /// </summary>
        /// <param name="element">The element to set focus on</param>
        public static void Focus(UIElement element)
        {
            //Focus in a callback to run on another thread, ensuring the main 
            //UI thread is initialized by the time focus is set
            ThreadPool.QueueUserWorkItem(delegate(Object theElement)
            {
                UIElement elem = (UIElement)theElement;
                elem.Dispatcher.Invoke(DispatcherPriority.Normal,
                    (Action)delegate()
                    {
                        elem.Focus();
                        Keyboard.Focus(elem);
                    });
            }, element);
        }
        #endregion
    }
}