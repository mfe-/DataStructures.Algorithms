using Microsoft.Win32;
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

        public ICommand _SaveAssemblyCommand;
        public ICommand SaveAssemblyCommand => _SaveAssemblyCommand ?? (_SaveAssemblyCommand = new DelegateCommand(OnSaveAssemblyCommand));

        protected void OnPickAssemblyCommand()
        {
            //assemblyladen
            //Data.Value.
            if(ModuleFunction == null)
            {
                ModuleFunction = new ModuleFunction();
                //Test
                ModuleFunction.Description = "Hallo Test";

            }
            else
            {
                ModuleFunction.Run();
            }
        }
         
        protected void OnSaveAssemblyCommand()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Title = "Save the assembly";
            saveFileDialog1.FileName = ModuleFunction.Description;
            saveFileDialog1.ShowDialog();

            var savinMethod = ModuleFunction.methodToRun;
            
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                fs.Close();
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
