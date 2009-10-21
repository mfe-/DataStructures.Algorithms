using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Get.Common;

namespace Get.Demo
{
    public class ModelView : INotifyPropertyChanged
    {
        //public ModelView()
        //{
        //    ObjectCoreList = new ObjectCoreList();
        //    ObjectCoreList.Add(new ObjectCore() { ObjectName = "Kontakt" });
        //    ObjectCore = new ObjectCore() { ObjectName = "Kontakt" };
        //    ObjectCore objectCore = new ObjectCore() { ObjectName = "Base" };
        //    ObjectCore.DerivedObject = objectCore;
        //    ObjectCore.ObjectPropertieList.Add(new ObjectPropertie() { PropertieName = "Vorname" });
        //    ObjectCore.ObjectPropertieList.Add(new ObjectPropertie() { PropertieName = "Nachname" });
        //}
        //public ObjectCoreList ObjectCoreList { get; set; }

        //private string _PropertieModifiers;
        //public string PropertieModifiers
        //{
        //    get
        //    {
        //        return _PropertieModifiers;
        //    }
        //    set
        //    {
        //        _PropertieModifiers = value;
        //        System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
        //        s.Start();
        //        NotifyPropertyChanged(this.GetMemberName(x=>x.PropertieModifiers));
        //        s.Stop();

        //    }
        //}

        //private string _text;
        //public string text
        //{
        //    get
        //    {
        //        return _text;
        //    }
        //    set
        //    {
        //        _text = value;
        //        System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
        //        s.Start();
        //        NotifyPropertyChanged("text");
        //        s.Stop();

        //    }
        //}

        //public ObjectCore ObjectCore { get; set; }

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        #endregion

    }
}
