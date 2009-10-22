using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Get.Model.Core
{
    public class ObjectCore : IObjectCore, INotifyPropertyChanged
    {

        #region IObjectCore Members

        private string _ObjectName = string.Empty;
        public string ObjectName
        {
            get
            {
                return _ObjectName;
            }
            set
            {
                _ObjectName = value;
            }
        }
        private ObjectPropertieList _ObjectPropertieList = new ObjectPropertieList();
        public ObjectPropertieList ObjectPropertieList
        {
            get
            {
                return _ObjectPropertieList;
            }
            set
            {
                _ObjectPropertieList = value;
            }
        }
        private IObjectCore _DerivedObject = null;
        public IObjectCore DerivedObject
        {
            get
            {
                return _DerivedObject;
            }
            set
            {
                _DerivedObject = value;
            }
        }

        #endregion

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
    public interface IObjectCore
    {
        string ObjectName { get; set; }
        ObjectPropertieList ObjectPropertieList { get; set; }
        Get.Model.Core.IObjectCore DerivedObject { get; set; }
    }
    public interface IObjectCoreList : IEnumerable<IObjectCore>
    {

    }
    public class ObjectCoreList : ObservableCollection<ObjectCore>
    {

    }
}
