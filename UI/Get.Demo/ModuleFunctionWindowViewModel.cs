using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            //ModuleFunction moduleFunction = new ModuleFunction();

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(ms))
            //    {
            //        var dataContractSerializerSettings = new DataContractSerializerSettings();
            //        dataContractSerializerSettings.PreserveObjectReferences = true;
            //        dataContractSerializerSettings.KnownTypes = new List<Type>() { typeof(Vertex<object>), typeof(Edge<object>) };
            //        DataContractSerializer serializer = new DataContractSerializer(moduleFunction.GetType(), dataContractSerializerSettings);
            //        serializer.WriteObject(writer, moduleFunction);
            //        writer.Flush();
            //        ms.Position = 0;
            //        XElement xElement = XElement.Load(ms);
            //    }
            //}

        }
        private ICommand _PickAssemblyCommand;
        public ICommand PickAssemblyCommand => _PickAssemblyCommand ?? (_PickAssemblyCommand = new DelegateCommand(OnPickAssemblyCommand));

        protected void OnPickAssemblyCommand()
        {
            //assemblyladen
            //Data.Value.
            if (ModuleFunction == null)
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
