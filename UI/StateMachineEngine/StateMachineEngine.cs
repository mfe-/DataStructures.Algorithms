using DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineEngine
{
    public class StateMachineEngine
    {
        public IVertex<IState> Current { get; protected set; }
        public object Run(IVertex startVertex, IState state)
        {


            return null;
        }

    }
}
