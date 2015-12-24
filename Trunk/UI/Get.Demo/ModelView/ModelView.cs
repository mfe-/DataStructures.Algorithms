using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
//using Get.Common;
//using Get.Model.Graph;

namespace Get.Demo
{
    public class GraphModelView : INotifyPropertyChanged
    {
        public GraphModelView()
        {

        }





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
