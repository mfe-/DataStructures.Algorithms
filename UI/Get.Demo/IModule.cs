using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Demo
{
    public interface IModule
    {
        object Run(params object[] p);
        bool Condition(object param);
    }
}
