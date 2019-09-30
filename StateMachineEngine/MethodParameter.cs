using System;

namespace StateMachineEngine
{
    public class MethodParameter
    {
        public String Name { get; set; }
        public String ParameterType { get; set; }
        public String ParameterValue { get; set; }
        public int Position { get; set; }
        /// <summary>
        /// Indicates that the rest value of the previous computed state should be used for this parameter
        /// </summary>
        public bool UsePreviousStateValue { get; set; }
    }
}
