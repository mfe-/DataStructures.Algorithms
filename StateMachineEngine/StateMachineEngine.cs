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
        public IVertex<StateModule> Current { get; protected set; }
        public StateModule CurrentState { get; protected set; }
        protected StateModule LastVertex { get; set; }
        public async Task<object> Run(IVertex<StateModule> startVertex)
        {
            Current = startVertex as IVertex<StateModule>;
            CurrentState = Current.Value;
            object stateResult = null;
            while (Current != null && CurrentState != null)
            {
                stateResult = await RunState(CurrentState, stateResult);
                LastVertex = CurrentState;
                foreach (IEdge edge in Current.Edges)
                {
                    var vertex = edge.V as IVertex<StateModule>;
                    if (vertex != null && vertex.Value != null && vertex.Value.Condition(stateResult))
                    {
                        Current = vertex;
                        CurrentState = Current.Value;
                    }
                }
                //there is no edge which contains a state which can be used - exit loop
                if (LastVertex == CurrentState) CurrentState = null;
            }


            return null;
        }

        private async Task<object> RunState(StateModule stateModule, object previousComputedStateResult)
        {
            stateModule = Current.Value;
            //load assembly
            if (stateModule.Assembly == null)
            {
                Current?.Value?.LoadAssembly();
            }
            //load method
            MethodInfo methodInfo = stateModule.LoadMethodTyp();
            object classInstanceToInvokeMethod = null;
            if (methodInfo.IsStatic == false)
            {
                classInstanceToInvokeMethod = Activator.CreateInstance(methodInfo.DeclaringType, null);
            }
            if (classInstanceToInvokeMethod != null || methodInfo.IsStatic)
            {
                var methodParametersInfo = stateModule.GenerateMethodParameters().OrderBy(a => a.Position);
                List<object> param = new List<object>();
                foreach (ParameterInfo paramInfo in methodParametersInfo)
                {
                    //find the method parameter value from the current state 
                    MethodParameter methodParameter = stateModule.MethodParameters.FirstOrDefault(a => a.Position == paramInfo.Position);
                    string rawValue = methodParameter.ParameterValue;
                    //generate value
                    if (methodParameter.UsePreviousStateValue)
                    {
                        param.Add(previousComputedStateResult);
                    }
                    else
                    {
                        object val = InitDefaultValue(paramInfo.ParameterType, methodParameter.ParameterValue);
                        param.Add(val);
                    }
                }
                object resultMethod = null;
                if (methodInfo.IsStatic)
                {
                    resultMethod = methodInfo.Invoke(null, param.ToArray());
                }
                else
                {
                    resultMethod = methodInfo.Invoke(classInstanceToInvokeMethod, param.ToArray());
                }
                if (resultMethod is Task)
                {
                    Task resultMethodTask = (resultMethod as Task);
                    await resultMethodTask;

                    if (methodInfo.ReturnType.IsGenericType)
                    {
                        var resultProperty = methodInfo.ReturnType.GetProperty("Result");
                        var x = resultProperty.GetValue(resultMethodTask);
                        return x;
                    }


                }
                else
                {
                    return Task.FromResult(resultMethod);
                }
            }
            return null;
        }

        private object InitDefaultValue(Type memberType, object value)
        {
            if (memberType.IsValueType)
            {
                object x = memberType.InvokeMember(string.Empty, BindingFlags.CreateInstance, null, null, new object[0]);

                return x;
            }
            else
            {
                if (memberType == typeof(string))
                {
                    return $"{value}";
                }

                object x = Activator.CreateInstance(memberType, null);
                return x;
            }
        }


    }
}
