using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Demo
{
    public class ModuleFunction : IModule
    {
        public bool Condition(object param)
        {
            throw new NotImplementedException();
        }
        public string methodToRun = "";
        public List<object> paramList = new List<object>();
        public object methodResult = "";

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
