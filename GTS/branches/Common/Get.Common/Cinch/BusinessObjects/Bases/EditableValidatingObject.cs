using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;

namespace Get.Common.Cinch
{
    /// <summary>
    /// Provides a IDataErrorInfo validating object that is also
    /// editable by implementing the IEditableObject interface
    /// </summary>
    public abstract partial class EditableValidatingObject :
        ValidatingObject, IEditableObject
    {
        #region Data
        /// <summary>
        /// This stores the current "copy" of the object. 
        /// If it is non-null, then we are in the middle of an 
        /// editable operation.
        /// </summary>
        protected Dictionary<string, object> _savedState;
        #endregion

        #region Public/Protected Methods
        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            OnBeginEdit();
            _savedState = GetFieldValues();
        }

        /// <summary>
        /// Interception point for derived logic to do work when beginning edit.
        /// </summary>
        protected virtual void OnBeginEdit()
        {
        }

        /// <summary>
        /// Discards changes since the last 
        /// <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            OnCancelEdit();
            RestoreFieldValues(_savedState);
            _savedState = null;
        }

        /// <summary>
        /// This is called in response CancelEdit and provides an interception point.
        /// </summary>
        protected virtual void OnCancelEdit()
        {
        }

        /// <summary>
        /// Pushes changes since the last 
        /// <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> 
        /// or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> 
        /// call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            OnEndEdit();
            _savedState = null;
        }

        /// <summary>
        /// This is called in response EndEdit and provides an interception point.
        /// </summary>
        protected virtual void OnEndEdit()
        {
        }

        /// <summary>
        /// This is used to clone the object.  
        /// Override the method to provide a more efficient clone.  
        /// The default implementation simply reflects across 
        /// the object copying every field.
        /// </summary>
        /// <returns>Clone of current object</returns>
        protected virtual Dictionary<string, object> GetFieldValues()
        {
            return GetType().GetFields(BindingFlags.Public | 
                BindingFlags.NonPublic | BindingFlags.Instance).Select(
                fi => new { Key = fi.Name, Value = fi.GetValue(this) })
                    .ToDictionary(k => k.Key, k => k.Value);
        }

        /// <summary>
        /// This restores the state of the current object from the passed clone object.
        /// </summary>
        /// <param name="fieldValues">Object to restore state from</param>
        protected virtual void RestoreFieldValues(Dictionary<string, object> fieldValues)
        {
            foreach (FieldInfo fi in GetType().GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object value;
                if (fieldValues.TryGetValue(fi.Name, out value))
                    fi.SetValue(this, value);
                else
                {   
                    Debug.WriteLine("Failed to restore field " + 
                        fi.Name + " from cloned values, field not found in Dictionary.");
                }
            }
        }
        #endregion
    }
}
