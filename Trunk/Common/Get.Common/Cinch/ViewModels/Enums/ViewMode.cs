using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.Common.Cinch
{
    /// <summary>
    /// Used by the ViewModel to set the state of all
    /// the data of a particular views data to the correct state
    /// Basically what happens is that the ViewModel loops through 
    /// a source object (UI Model class is expected really) and attempts
    /// to set all Cinch.DataWrapper fields to have the correct 
    /// Cinch.DataWrapper.IsEditable 
    /// to the correct state based on the current ViewMode 
    /// </summary>
    public enum ViewMode { EditMode, AddMode, ViewOnlyMode };
}
