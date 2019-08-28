using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataStructures.Demo
{
    [DataContract(Namespace = "http://schemas.get.com/Graph/ModuleFunction")]
    public class ModuleFunction : IModule, INotifyPropertyChanged
    {
        public ModuleFunction()
        {
            MethodParameters = new List<MethodParameter>();
        }
        public bool Condition(object param)
        {
            throw new NotImplementedException();
        }
        public object methodResult = "";

        [DataMember(Name = "MethodParameters", Order = 5, IsRequired = false)]
        public IEnumerable<MethodParameter> MethodParameters { get; set; }

        [DataMember(Name = "MethodDeclaringType", Order = 4, IsRequired = false)]
        public string MethodDeclaringType { get; set; }

        [DataMember(Name = "AssemblyFullName", Order = 3, IsRequired = false)]
        public string AssemblyFullName { get; set; }

        [DataMember(Name = "MethodTyp", Order = 2, IsRequired = false)]
        private string _MethodNameTyp;
        public string MethodNameTyp
        {
            get { return _MethodNameTyp; }
            set { SetProperty(ref _MethodNameTyp, value, nameof(MethodNameTyp)); }
        }

        [DataMember(Name = "Description", Order = 1, IsRequired = false)]
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value, nameof(Description)); }
        }


        public object Run(params object[] p)
        {
            var t = Assembly.GetExecutingAssembly().GetTypes();



            methodResult = t;
            //find proper method via reflection from methodToRun
            //var t = Assembly.GetExecutingAssembly().GetTypes();
            //prepare paramList

                //execute method

            //save result into methodResult
            return methodResult;
            //return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }
            storage = value;

            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertiesChanged(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(propertyName);
            }
        }
    }
}
