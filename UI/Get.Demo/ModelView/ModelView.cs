using System;
using System.ComponentModel;

namespace DataStructures.Demo
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
