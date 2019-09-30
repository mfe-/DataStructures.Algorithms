using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using StateMachineEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace DataStructures.Demo
{
    public class StateModuleWindowViewModel : BindableBase
    {
        public StateModuleWindowViewModel()
        {
            MethodInfos = new ObservableCollection<MethodInfo>();
        }
        private ICommand _PickAssemblyCommand;
        public ICommand PickAssemblyCommand => _PickAssemblyCommand ?? (_PickAssemblyCommand = new DelegateCommand(OnPickAssemblyCommand));

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
            set
            {
                SetProperty(ref _SelectedMethodInfos, value, nameof(SelectedMethodInfos));
                ParameterInfos = SelectedMethodInfos?.GetParameters().Select(a => new MethodParameter()
                {
                    Name = a.Name,
                    ParameterType = a.ParameterType?.FullName,
                    ParameterValue = "",
                    Position = a.Position
                }).ToList();
                if (ModuleFunction != null && SelectedMethodInfos != null)
                {
                    //new method selected
                    if (SelectedMethodInfos.ToString() != ModuleFunction.MethodNameTyp)
                    {
                        ModuleFunction.MethodNameTyp = SelectedMethodInfos.ToString();
                        ModuleFunction.MethodDeclaringType = SelectedMethodInfos.DeclaringType.FullName;
                        ModuleFunction.MethodParameters = ParameterInfos;
                    }
                    else
                    {
                        ParameterInfos = ModuleFunction.MethodParameters.ToList();
                    }
                }
            }
        }

        private List<MethodParameter> _ParameterInfos;
        public List<MethodParameter> ParameterInfos
        {
            get { return _ParameterInfos; }
            set { SetProperty(ref _ParameterInfos, value, nameof(ParameterInfos)); }
        }

        private String _FilterMethodName;
        public String FilterMethodName
        {
            get { return _FilterMethodName; }
            set
            {
                SetProperty(ref _FilterMethodName, value, nameof(FilterMethodName));
                FilterMethodInfos = MethodInfos.Where(a => a.Name.ToLower().StartsWith(FilterMethodName.ToLower()) || a.Name.Contains(FilterMethodName));
            }
        }
        public Assembly Assembly { get; set; }
        protected void OnPickAssemblyCommand()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                bool? result = openFileDialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    Assembly = Assembly.LoadFrom(openFileDialog.FileName);
                    SetupSelectableMethods();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void SetupSelectableMethods()
        {
            List<MethodInfo> mList = new List<MethodInfo>();
            foreach (var t in Assembly.GetTypes().ToList())
            {
                if (t.IsPublic)
                {
                    var m = t.GetMethods();
                    if (t != null && t.IsPublic && t.Name != nameof(MethodInfo.Equals) && t.Name != nameof(MethodInfo.ToString))
                    {
                        mList.AddRange(m);
                    }
                }
            }
            MethodInfos = new ObservableCollection<MethodInfo>(mList);
            FilterMethodInfos = new ObservableCollection<MethodInfo>(MethodInfos);

            ModuleFunction.AssemblyFullName = Assembly.FullName;
        }

        //command mit speichern -> 

        //todo list mite methoden

        public StateModule ModuleFunction
        {
            get
            {
                if ((Vertex as Vertex<IState>)?.Value as StateModule != null)
                {
                    return (Vertex as Vertex<IState>)?.Value as StateModule;
                }
                if ((Vertex as IVertex<StateModule>)?.Value as StateModule != null)
                {
                    return (Vertex as IVertex<StateModule>)?.Value as StateModule;
                }
                return null;
            }
            set
            {
                if ((Vertex as Vertex<IState>) != null)
                    (Vertex as Vertex<IState>).Value = value;
                if ((Vertex as Vertex<StateModule>) != null)
                {
                    (Vertex as Vertex<StateModule>).Value = value;
                }
                RaisePropertyChanged(nameof(ModuleFunction));
            }
        }

        private IVertex _Vertex;
        public IVertex Vertex
        {
            get { return _Vertex; }
            set
            {
                SetProperty(ref _Vertex, value, nameof(Vertex));
                if (ModuleFunction == null)
                {
                    ModuleFunction = new StateModule();
                }
                else
                {
                    Assembly = ModuleFunction.LoadAssembly();
                    if (Assembly != null)
                    {
                        SetupSelectableMethods();
                        if (!String.IsNullOrEmpty(ModuleFunction.MethodNameTyp))
                        {
                            SelectedMethodInfos = ModuleFunction.LoadMethodTyp();
                        }
                    }
                }
            }
        }
    }
}
