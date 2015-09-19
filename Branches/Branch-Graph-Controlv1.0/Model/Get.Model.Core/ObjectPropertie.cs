using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Get.Model.Core
{
    public enum Modifiers
    {
        Public = 1,
        Private
    }
    public enum ObjectType
    {
        Type = 1,
        Propertie,
        Function,
        Event
    }
    public abstract class Object
    {
        /// <summary>
        /// Name vom Objekt
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Modifizierer vom Objekt (private, public)
        /// </summary>
        public Modifiers Modifiers { get; set; }
    }
    public sealed class Class : Object
    {

    }
    public class ObjectPropertie : IObjectPropertie, INotifyPropertyChanged
    {
        string _PropertieName = string.Empty;
        object _PropertieType = null;
        Modifiers _PropertieModifiers;

        public string PropertieName
        {
            get
            {
                return _PropertieName;
            }
            set
            {
                _PropertieName = value;
            }
        }

        public object PropertieType
        {
            get
            {
                return _PropertieType;
            }
            set
            {
                _PropertieType = value;
            }
        }

        public Modifiers PropertieModifiers
        {
            get
            {
                return _PropertieModifiers;
            }
            set
            {
                _PropertieModifiers = value;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        [DebuggerStepThrough]
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion


    }
    public interface IObjectPropertie 
    {
        string PropertieName { get; set; }
        object PropertieType { get; set; }
        Modifiers PropertieModifiers { get; set; }
    }
    public class ObjectPropertieList : ObservableCollection<IObjectPropertie>
    {

    }
}
