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
        public bool Condition(object param)
        {
            throw new NotImplementedException();
        }
        public string methodToRun = "";
        public List<object> paramList = new List<object>();
        public object methodResult = "";

        [DataMember(Name = "MethodDeclaringType", Order = 4, IsRequired = true)]
        public string MethodDeclaringType { get; set; }

        [DataMember(Name = "AssemblyFullName", Order = 3, IsRequired = true)]
        public string AssemblyFullName { get; set; }
        [DataMember(Name = "MethodTyp", Order = 2, IsRequired = true)]
        public string MethodTyp { get; set; }
        [DataMember(Name = "Description", Order = 1, IsRequired = true)]
        public string Description { get; set; }

        public object Run(params object[] p)
        {
            var t = Assembly.GetExecutingAssembly().GetTypes();

            paramList.Add(t);

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
