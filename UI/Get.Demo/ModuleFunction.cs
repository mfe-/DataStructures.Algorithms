using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Demo
{
    [DataContract(Namespace = "http://schemas.get.com/Graph/ModuleFunction")]
    public class ModuleFunction : IModule
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
        public string MethodTyp { get; set; }
        [DataMember(Name = "Description", Order = 1, IsRequired = false)]
        public string Description { get; set; }

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
    }
}
