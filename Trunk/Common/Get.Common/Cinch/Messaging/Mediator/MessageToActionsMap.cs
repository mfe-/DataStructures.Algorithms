using System;
using System.Collections.Generic;
using System.Reflection;

namespace Get.Common.Cinch
{
    /// <summary>
    /// This class is an implementation detail of the Mediator class.
    /// </summary>
    internal class MessageToActionsMap
    {
        #region Data
        //store a hash where the key is the message and the value 
        //is the list of Actions to call
		readonly Dictionary<string, List<WeakAction>> map = 
            new Dictionary<string, List<WeakAction>>();
        #endregion

        #region Internal Methods
        /// <summary>
        /// Creates a weak callback and adds it to a local list of 
        /// weak callbacks
        /// </summary>
        /// <param name="message">The mediator message</param>
        /// <param name="target">The sender</param>
        /// <param name="method">The method to call on sender</param>
        /// <param name="actionType">The parameter type if using generics</param>
        internal void AddAction(string message, object target, 
            MethodInfo method, Type actionType)		
        {
            if (message == null)
                throw new ArgumentNullException("message");

			if (method == null)
                throw new ArgumentNullException("method");

            lock (map)//lock on the dictionary 
            {
                if (!map.ContainsKey(message))
                    map[message] = new List<WeakAction>();

                map[message].Add(new WeakAction(target, method, actionType));
            }
        }
        /// <summary>
        /// Gets all weak callbacks for a given Mediator message
        /// </summary>
        /// <param name="message">Mediator message</param>
        /// <returns>All weak callbacks for a given Mediator message</returns>
        internal List<Delegate> GetActions(string message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            List<Delegate> actions;
            lock (map)
            {
                if (!map.ContainsKey(message))
                    return null;

                List<WeakAction> weakActions = map[message];
                actions = new List<Delegate>(weakActions.Count);
                for (int i = weakActions.Count - 1; i > -1; --i)
                {
                    WeakAction weakAction = weakActions[i];
                    if (!weakAction.IsAlive)
                        weakActions.RemoveAt(i);
                    else
                        actions.Add(weakAction.CreateAction());
                }

                //delete the list from the hash if it is now empty
                if (weakActions.Count == 0)
                    map.Remove(message);
            }
            return actions;
        }
        #endregion
    }
}