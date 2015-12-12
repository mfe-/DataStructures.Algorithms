using System;
using System.Reflection;

namespace Get.Common.Cinch
{
    /// <summary>
    /// This class is an implementation detail of the 
    /// MessageToActionsMap class.
    /// </summary>
    internal class WeakAction : WeakReference
    {
        #region Data
        readonly MethodInfo method;
        readonly Type delegateType;
        #endregion

        #region Internal Methods
        /// <summary>
        /// Constructs a new WeakAction
        /// </summary>
        /// <param name="target">The sender</param>
        /// <param name="method">The method to call on sender</param>
        /// <param name="parameterType">The parameter type if using generics</param>
        internal WeakAction(object target, MethodInfo method, Type parameterType)
            : base(target)
        {
            this.method = method;

			if (parameterType == null)
				this.delegateType = typeof(Action);
			else
				this.delegateType = typeof(Action<>).MakeGenericType(parameterType);
        }

        /// <summary>
        /// Creates callback delegate
        /// </summary>
        /// <returns>Callback delegate</returns>
		internal Delegate CreateAction()
		{
			object target = base.Target;
			if (target != null)
			{		
				// Rehydrate into a real Action
				// object, so that the method
				// can be invoked on the target.
				return Delegate.CreateDelegate(
							this.delegateType,
							base.Target,
							method);
			}
			else
			{
				return null;
			}
        }
        #endregion
    }
}