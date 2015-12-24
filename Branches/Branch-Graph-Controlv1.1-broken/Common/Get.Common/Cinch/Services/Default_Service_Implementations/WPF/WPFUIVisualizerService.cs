using System;
using System.Collections.Generic;
using System.Windows;

namespace Get.Common.Cinch
{
    /// <summary>
    /// This class implements the IUIVisualizerService for WPF purposes.
    /// This implementation HAD TO be in the Main project, as
    /// it needs to know about Popup windows that is not known about in 
    /// the Cinch project.
    /// </summary>
    public class WPFUIVisualizerService : IUIVisualizerService
    {
        #region Data
        private readonly Dictionary<string, Type> _registeredWindows;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public WPFUIVisualizerService()
        {
            _registeredWindows = new Dictionary<string, Type>();


        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Registers a collection of entries
        /// </summary>
        /// <param name="startupData"></param>
        public void Register(Dictionary<string, Type> startupData)
        {
            foreach (var entry in startupData)
                Register(entry.Key, entry.Value);
        }

        /// <summary>
        /// Registers a type through a key.
        /// </summary>
        /// <param name="key">Key for the UI dialog</param>
        /// <param name="winType">Type which implements dialog</param>
        public void Register(string key, Type winType)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (winType == null)
                throw new ArgumentNullException("winType");
            if (!typeof(Window).IsAssignableFrom(winType))
                throw new ArgumentException("winType must be of type Window");

            lock (_registeredWindows)
            {
                _registeredWindows.Add(key, winType);
            }
        }

        /// <summary>
        /// This unregisters a type and removes it from the mapping
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>True/False success</returns>
        public bool Unregister(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            lock (_registeredWindows)
            {
                return _registeredWindows.Remove(key);
            }
        }

        /// <summary>
        /// This method displays a modaless dialog associated with the given key.
        /// </summary>
        /// <param name="key">Key previously registered with the UI controller.</param>
        /// <param name="state">Object state to associate with the dialog</param>
        /// <param name="setOwner">Set the owner of the window</param>
        /// <param name="completedProc">Callback used when UI closes (may be null)</param>
        /// <returns>True/False if UI is displayed</returns>
        public bool Show(string key, object state, bool setOwner,
            EventHandler<UICompletedEventArgs> completedProc)
        {
            Window win = CreateWindow(key, state, setOwner, completedProc, false);
            if (win != null)
            {
                win.Show();
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method displays a modal dialog associated with the given key.
        /// </summary>
        /// <param name="key">Key previously registered with the UI controller.</param>
        /// <param name="state">Object state to associate with the dialog</param>
        /// <returns>True/False if UI is displayed.</returns>
        public bool? ShowDialog(string key, object state)
        {
            Window win = CreateWindow(key, state, true, null, true);
            if (win != null)
                return win.ShowDialog();

            return false;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This creates the WPF window from a key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="dataContext">DataContext (state) object</param>
        /// <param name="setOwner">True/False to set ownership to MainWindow</param>
        /// <param name="completedProc">Callback</param>
        /// <param name="isModal">True if this is a ShowDialog request</param>
        /// <returns>Success code</returns>
        private Window CreateWindow(string key, object dataContext, bool setOwner,
            EventHandler<UICompletedEventArgs> completedProc, bool isModal)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            Type winType;
            lock (_registeredWindows)
            {
                if (!_registeredWindows.TryGetValue(key, out winType))
                    return null;
            }

            var win = (Window)Activator.CreateInstance(winType);
            win.DataContext = dataContext;
            if (setOwner)
                win.Owner = Application.Current.MainWindow;

            if (dataContext != null)
            {
                var bvm = dataContext as ViewModelBase;
                if (bvm != null)
                {
                    if (isModal)
                    {
                        bvm.CloseRequest += ((s, e) =>
                        {
                            try
                            {
                                win.DialogResult = e.Result;
                            }
                            catch (InvalidOperationException)
                            {
                                win.Close();
                            }
                        });
                    }
                    else
                    {
                        bvm.CloseRequest += ((s, e) => win.Close());
                    }
                    bvm.ActivateRequest += ((s, e) => win.Activate());
                }
            }

            if (completedProc != null)
            {
                win.Closed +=
                    (s, e) =>
                        completedProc
                        (this, new UICompletedEventArgs
                        {
                            State = dataContext,
                            Result = (isModal) ? win.DialogResult : null
                        }
                        );

            }

            return win;
        }
        #endregion
    }
}
