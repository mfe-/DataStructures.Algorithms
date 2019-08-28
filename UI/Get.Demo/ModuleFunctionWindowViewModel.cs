using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace DataStructures.Demo
{
    public class ModuleFunctionWindowViewModel : BindableBase
    {
        public ModuleFunctionWindowViewModel()
        {
            MethodInfos = new ObservableCollection<MethodInfo>();
        }
        private ICommand _PickAssemblyCommand;
        public ICommand PickAssemblyCommand => _PickAssemblyCommand ?? (_PickAssemblyCommand = new DelegateCommand(OnPickAssemblyCommand));

        public ICommand _SaveAssemblyCommand;
        public ICommand SaveAssemblyCommand => _SaveAssemblyCommand ?? (_SaveAssemblyCommand = new DelegateCommand(OnSaveAssemblyCommand));

        private ObservableCollection<MethodInfo> _MethodInfos;
        public ObservableCollection<MethodInfo> MethodInfos
        {
            get { return _MethodInfos; }
            set { SetProperty(ref _MethodInfos, value, nameof(MethodInfos)); }
        }
        private IEnumerable<MethodInfo> _FilterMethodInfos;
        public IEnumerable<MethodInfo> FilterMethodInfos
        {
            get { return _FilterMethodInfos; }
            set { SetProperty(ref _FilterMethodInfos, value, nameof(FilterMethodInfos)); }
        }
        private MethodInfo _SelectedMethodInfos;
        public MethodInfo SelectedMethodInfos
        {
            get { return _SelectedMethodInfos; }
            set { SetProperty(ref _SelectedMethodInfos, value, nameof(SelectedMethodInfos)); }
        }
        private String _FilterMethodName;
        public String FilterMethodName
        {
            get { return _FilterMethodName; }
            set
            {
                SetProperty(ref _FilterMethodName, value, nameof(FilterMethodName));
                FilterMethodInfos = MethodInfos.Where(a => a.Name.StartsWith(FilterMethodName));
            }
        }
        public Assembly Assembly { get; set; }
        protected void OnPickAssemblyCommand()
        {
            if (ModuleFunction == null)
            {
                ModuleFunction = new ModuleFunction();
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                Assembly = Assembly.LoadFrom(openFileDialog.FileName);
                List<MethodInfo> mList = new List<MethodInfo>();
                foreach (var t in Assembly.GetTypes().ToList())
                {
                    var m = t.GetMethods();
                    if (t != null ||
                        t.Name != nameof(MethodInfo.Equals) || t.Name != nameof(MethodInfo.ToString))
                    {
                        mList.AddRange(m);
                    }
                }
                MethodInfos = new ObservableCollection<MethodInfo>(mList);
                FilterMethodInfos = new ObservableCollection<MethodInfo>(MethodInfos);
            }
        }

        protected void OnSaveAssemblyCommand()
        {
            ModuleFunction.MethodTyp = SelectedMethodInfos.ToString();
            ModuleFunction.MethodDeclaringType = SelectedMethodInfos.DeclaringType.FullName;
            ModuleFunction.AssemblyFullName = Assembly.FullName;
        }

        //command mit speichern -> 

        //todo list mite methoden

        public ModuleFunction ModuleFunction
        {
            get
            {
                return (Vertex as Vertex<ModuleFunction>)?.Value as ModuleFunction;
            }
            set
            {
                if ((Vertex as Vertex<ModuleFunction>) != null)
                    (Vertex as Vertex<ModuleFunction>).Value = value;
            }
        }

        private IVertex _Vertex;
        public IVertex Vertex
        {
            get { return _Vertex; }
            set { SetProperty(ref _Vertex, value, nameof(Vertex)); }
        }
    }
}
