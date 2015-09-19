using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Get.Common.Cinch
{
    /// <summary>
    /// Provides a singleton instance which holds a
    /// IUnityContainer object.
    /// </summary>
    public sealed class UnitySingleton
    {
        #region Data
        static readonly UnitySingleton instance = new UnitySingleton();
        static IUnityContainer unityContainer;
        static readonly object syncLock = new object();
        #endregion

        #region Ctor
        static UnitySingleton()
        { 
            
        }

        private UnitySingleton()
        {
         
        }
        #endregion

        #region Public Properties
        public static UnitySingleton Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Gets the unity container that was attached 
        /// when the UnitySingleton was Initialized.
        /// </summary>
        /// <value>The global unity container.</value>
        public IUnityContainer Container
        {
            get
            {
                if (unityContainer == null)
                {
                    lock (syncLock)
                    {
                        if (unityContainer == null)
                        {
                            unityContainer = new UnityContainer();
                        }
                    }
                }
                return unityContainer;
            }
        }
        #endregion
    }
}
