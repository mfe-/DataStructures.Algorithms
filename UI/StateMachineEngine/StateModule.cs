using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.IO;

namespace StateMachineEngine
{
    [DataContract(Namespace = "http://schemas.get.com/Graph/ModuleFunction")]
    public class StateModule : IState, INotifyPropertyChanged
    {
        public StateModule()
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

        public IEnumerable<ParameterInfo> GenerateMethodParameters()
        {
            if (_MethodTyp == null) LoadMethodTyp();

            var parameters = _MethodTyp.GetParameters();

            //foreach (ParameterInfo parameterInfo in parameters)
            //{
            //    MethodParameter methodParameter = MethodParameters.FirstOrDefault(a => a.Position == parameterInfo.Position &&
            //    Type.GetType(a.ParameterType, false) == parameterInfo.ParameterType);
            //}
            return parameters;
        }

        [DataMember(Name = "MethodDeclaringType", Order = 4, IsRequired = false)]
        public string MethodDeclaringType { get; set; }

        [DataMember(Name = "AssemblyFullName", Order = 3, IsRequired = false)]
        public string AssemblyFullName { get; set; }

        protected MethodInfo _MethodTyp = null;
        [DataMember(Name = "MethodTyp", Order = 2, IsRequired = false)]
        private string _MethodNameTyp;
        public string MethodNameTyp
        {
            get { return _MethodNameTyp; }
            set { SetProperty(ref _MethodNameTyp, value, nameof(MethodNameTyp)); }
        }
        /// <summary>
        /// Looks up for the <seealso cref="MethodNameTyp"/> and sets the correct <seealso cref="System.MethodInfo"/>
        /// </summary>
        public MethodInfo LoadMethodTyp()
        {
            if (Assembly == null) LoadAssembly();
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
            if (!String.IsNullOrEmpty(MethodNameTyp))
            {
                _MethodTyp = mList.FirstOrDefault(a => a.ToString() == MethodNameTyp);
                return _MethodTyp;
            }
            else
            {
                throw new ArgumentException($"{nameof(MethodNameTyp)} not set!");
            }
        }

        [DataMember(Name = "Description", Order = 1, IsRequired = false)]
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value, nameof(Description)); }
        }

        protected Assembly Assembly { get; set; }
        public static IEnumerable<String> GetAssemblies()
        {
            string dir = Assembly.GetExecutingAssembly().Location;

            List<Assembly> allAssemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(dir);

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
                yield return dll;
        }
        /// <summary>
        /// Loads the proper Assembly by using <seealso cref="AssemblyFullName"/>
        /// </summary>
        /// <returns>The assembly which was loaded</returns>
        public Assembly LoadAssembly()
        {
            string foundAssemblyPath = GetAssemblies().
                        FirstOrDefault(a => a.Contains(",") && $"{AssemblyFullName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()}.dll" == new FileInfo(a).Name);
            if(!String.IsNullOrEmpty(foundAssemblyPath))
            {
                Assembly = Assembly.LoadFrom(foundAssemblyPath);
                if (Assembly.FullName != AssemblyFullName)
                {
                    throw new ArgumentException("Could not find proper Assembly!");
                }
            }
            return Assembly;
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
