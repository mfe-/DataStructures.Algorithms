using System;

namespace Get.Common.Cinch
{
    /// <summary>
    /// Allows ViewModels to mark up methods as
    /// Mediator callbacks
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MediatorMessageSinkAttribute : Attribute
    {
        #region Data
        public string Message { get; private set; }

        public Type ParameterType { get; set; }
        #endregion

        #region Ctor
        public MediatorMessageSinkAttribute(string message)
        {
            Message = message;
        }
        #endregion
    }
}