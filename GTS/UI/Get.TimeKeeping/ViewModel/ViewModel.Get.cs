using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.Common.Cinch;
using Get.Common;
using System.Collections.ObjectModel;
using System.Data;

namespace Get.UI.TimeKeeping
{
    public class ViewModel_Get : ViewModelBase
    {
        protected getEntities _context = new getEntities();

        public ViewModel_Get()
        {
            _context.ContextOptions.LazyLoadingEnabled = true;
            try
            {
                _Projects = _context.g_project.ToObservableCollection();
                _ProjectAssistens = _context.g_projectassistent.ToObservableCollection();
                _TimeSheet = _context.g_project_timesheet.ToObservableCollection();
            }
            catch (EntityException e)
            {
                _Projects = new ObservableCollection<g_project>();
                _ProjectAssistens = new ObservableCollection<g_projectassistent>();
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
                else return _ProjectAssistens.Where(a => a.g_project.Equals(SelectedProject)).ToObservableCollection();
            }
            set
            {
                _ProjectAssistens = value;
                NotifyPropertyChanged(this.GetMemberName(x => x.ProjectAssistens));
            }
        }


        private g_projectassistent _SelectedProjectAssisten;
        public g_projectassistent SelectedProjectAssisten
        {
            get
            {
                return _SelectedProjectAssisten;
            }
            set
            {
                _SelectedProjectAssisten = value;
                NotifyPropertyChanged(this.GetMemberName(x => x.SelectedProjectAssisten));
                NotifyPropertyChanged(this.GetMemberName(x => x.TimeSheet));
            }
        }

        private ObservableCollection<g_project_timesheet> _TimeSheet;
        public ObservableCollection<g_project_timesheet> TimeSheet
        {
            get
            {
                if (SelectedProjectAssisten == null) return _TimeSheet;
                else
                {
                    var t = _TimeSheet.Where(a => a.projectassistent.Equals(SelectedProjectAssisten.id)).ToObservableCollection<g_project_timesheet>();
                    return t;
                }
            }
            set
            {
                _TimeSheet = value;
                NotifyPropertyChanged(this.GetMemberName(x => x.TimeSheet));
            }
        }

        protected override void OnDispose()
        {
            g_project_timesheet g = new g_project_timesheet();
            base.OnDispose();
            _context.Dispose();

        }
    }
}
