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
        public ViewModel_Get()
        {
            using (getEntities context = new getEntities())
            {
                context.ContextOptions.LazyLoadingEnabled = false;
                _Projects = context.g_project.ToObservableCollection();
                _ProjectAssistens = context.g_projectassistent.ToObservableCollection();

                // Load the orders for the customer explicitly.
                //if (!contact.SalesOrderHeaders.IsLoaded)
                //{
                //    contact.SalesOrderHeaders.Load();
                //}
            }


        }
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


        private g_project _SelectedProject;
        public g_project SelectedProject
        {
            get
            {
                return _SelectedProject;
            }
            set
            {
                _SelectedProject = value;
                NotifyPropertyChanged(this.GetMemberName(x => x.SelectedProject));
                NotifyPropertyChanged(this.GetMemberName(x => x.ProjectAssistens));
            }
        }

        private ObservableCollection<g_projectassistent> _ProjectAssistens;
        public ObservableCollection<g_projectassistent> ProjectAssistens
        {
            get
            {
                if (SelectedProject == null) return new ObservableCollection<g_projectassistent>();
                else
                {
                    var g =  _ProjectAssistens.Where(a => a.g_project.Equals(SelectedProject)).ToObservableCollection();
                    return g;
                }
            }
            set
            {
                _ProjectAssistens = value;
                NotifyPropertyChanged(this.GetMemberName(x => x.ProjectAssistens));
            }
        }
    }
}
