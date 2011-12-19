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
        protected getEntities _context = new getEntities();

        public ViewModel_Get()
        {
            _context.ContextOptions.LazyLoadingEnabled = true;
            _Projects = _context.g_project.ToObservableCollection();
            _ProjectAssistens = _context.g_projectassistent.ToObservableCollection();
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
                else return _ProjectAssistens.Where(a => a.g_project.Equals(SelectedProject)).ToObservableCollection();        
            }
            set
            {
                _ProjectAssistens = value;
                NotifyPropertyChanged(this.GetMemberName(x => x.ProjectAssistens));
            }
        }



        private ObservableCollection<g_project_timesheet> _TimeSheet;
        public ObservableCollection<g_project_timesheet> TimeSheet
        {
            get
            {
                return _TimeSheet;
            }
            set
            {
                _TimeSheet = value;
                NotifyPropertyChanged(this.GetMemberName(x => x.TimeSheet));
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _context.Dispose();

        }
    }
}
