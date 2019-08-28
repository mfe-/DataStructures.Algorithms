using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Demo
{
    public interface IModule
    {
        string AssemblyFullName { get; set; }
        string MethodTyp { get; set; }
        string MethodDeclaringType { get; set; }
        IEnumerable<MethodParameter> MethodParameters { get; set; }
        object Run(params object[] p);
        bool Condition(object param);
        string Description { get; set; }
    }
}
