using System;
using System.Collections.Generic;
using System.Reflection;

namespace Get.Common.Cinch
{
	/// <summary>
	/// Provides loosely-coupled messaging between
	/// various colleagues.  All references to objects
	/// are stored weakly, to prevent memory leaks.
	/// </summary>
	public class Mediator
    {
        #region Data
        readonly MessageToActionsMap invocationList = new MessageToActionsMap();
        #endregion

        #region Public Methods
        /// <summary>
        /// Registers a new object with the mediator which will allow
        /// mediator callbacks to occur
        /// </summary>
        /// <param name="target"></param>
        public void Register(object target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//Inspect the attributes on all methods and check if there 
            //are RegisterMediatorMessageAttribute
			foreach (var methodInfo in target.GetType().GetMethods(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
				foreach (MediatorMessageSinkAttribute attribute in 
                        methodInfo.GetCustomAttributes(
                        typeof(MediatorMessageSinkAttribute), true))

					if (methodInfo.GetParameters().Length == 1)
						invocationList.AddAction(attribute.Message, target, 
                            methodInfo, attribute.ParameterType);
					else
						throw new InvalidOperationException(
                            "The registered method should only have 1" +
                            "parameter since the Mediator has only 1 argument to pass");
		}
        
        /// <summary>
        /// Register particular callback delegate for Mediator message callbacks
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback"></param>
		public void Register(string message, Delegate callback)
		{
			if (callback.Target == null)
				throw new InvalidOperationException("Delegate cannot be static");

			ParameterInfo[] parameters = callback.Method.GetParameters();

			// JAS - Changed this logic to allow for 0 or 1 parameter.
			if (parameters != null && parameters.Length > 1)
				throw new InvalidOperationException(
                    "The registered delegate should only have 0 or 1 parameter" +
                    "since the Mediator has up to 1 argument to pass");

			Type parameterType = (parameters == null || parameters.Length == 0) ? null : 
                parameters[0].ParameterType;

			invocationList.AddAction(message, callback.Target, 
                callback.Method, parameterType);
		}
        /// <summary>
        /// Notify collegues use weak callback for those that
        /// registered for message
        /// </summary>
        /// <typeparam name="T">The type for the message</typeparam>
        /// <param name="message">The message to lookup weak callbacks
        /// for</param>
        /// <param name="parameter">The state parameter</param>
		public void NotifyColleagues<T>(string message, T parameter)
		{
			var actions = invocationList.GetActions(message);

			if (actions != null)
				actions.ForEach(action => action.DynamicInvoke(parameter));
		}

        /// <summary>
        /// Notify collegues use weak callback for those that
        /// registered for message
        /// </summary>
        /// <typeparam name="T">The type for the message</typeparam>
        /// <param name="message">The message to lookup weak callbacks
        /// for</param>
		public void NotifyColleagues<T>(string message)
		{
			var actions = invocationList.GetActions(message);

			if (actions != null)
				actions.ForEach(action => action.DynamicInvoke());
        }
        #endregion
    }
}