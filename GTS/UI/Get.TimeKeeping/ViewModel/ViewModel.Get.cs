using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.Common.Cinch;
using Get.Common;
using System.Collections.ObjectModel;

namespace Get.UI.TimeKeeping
{
    public class ViewModel_Get : ViewModelBase
    {

        private ObservableCollection<g_project> _Projects;
        public ObservableCollection<g_project> Projects
        {
            get
            {
                return _Projects;
            }
            set
            {
                _Projects = value;
                NotifyPropertyChanged(this.GetMemberName(x => x.Projects));
            }
        }
    }
}
