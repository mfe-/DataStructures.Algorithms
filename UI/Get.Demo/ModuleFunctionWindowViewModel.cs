using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataStructures.Demo
{
    public class ModuleFunctionWindowViewModel : BindableBase
    {
        public ModuleFunctionWindowViewModel()
        {

        }
        private ICommand _PickAssemblyCommand;
        public ICommand PickAssemblyCommand => _PickAssemblyCommand ?? (_PickAssemblyCommand = new DelegateCommand(OnPickAssemblyCommand));

        protected void OnPickAssemblyCommand()
        {
            //assemblyladen
            //Data.Value.
            if(ModuleFunction == null)
            {
                ModuleFunction = new ModuleFunction();
            }

        }

        //command mit speichern -> 

        //todo list mite methoden

        public ModuleFunction ModuleFunction
        {
            get
            {
                return Vertex.Value as ModuleFunction;
            }
            set
            {
                Vertex.Value = value;
            }
        }

        private IVertex<IModule> _Vertex;
        public IVertex<IModule> Vertex
        {
            get { return _Vertex; }
            set { SetProperty(ref _Vertex, value, nameof(Vertex)); }
        }
    }
}
