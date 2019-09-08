using System.Collections.Generic;

namespace StateMachineEngine
{
    public interface IState
    {
        /// <summary>
        /// Get or sets the assembly FullName which should be used to look up for the executing method
        /// </summary>
        string AssemblyFullName { get; set; }
        /// <summary>
        /// Get or sets the Type which contains the method
        /// </summary>
        string MethodDeclaringType { get; set; }
        /// <summary>
        /// Get or sets the method which should be executed
        /// </summary>
        string MethodNameTyp { get; set; }
        /// <summary>
        /// Get or sets the parameter which should be passed when executing the method
        /// </summary>
        IEnumerable<MethodParameter> MethodParameters { get; set; }
        /// <summary>
        /// Executes the method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        object Run(params object[] p);
        bool Condition(object param);
        string Description { get; set; }
    }
}
