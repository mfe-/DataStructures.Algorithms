using System;
using System.Collections.Generic;


/// <summary>
/// Provides a mechanism to be add and locate a specific services
/// </summary>
namespace Get.Common.Cinch
{
    /// <summary>
    /// This class acts as a resolver for typed services 
    /// (interfaces and implementations).
    /// </summary>
    /// <example>
    /// To register a service use Add:
    /// <![CDATA[
    /// serviceResolver.Add(typeof(IService), new Service());
    /// 
    /// To retrieve a service use Resolve:
    /// 
    /// IService svc = serviceResolver<IService>.Resolve();
    /// ]]>
    /// </example>
    public class ServiceProvider : IServiceProvider
    {
        #region Data
        private readonly Dictionary<Type, object> _services = 
            new Dictionary<Type, object>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all services from the resolver list 
        /// </summary>
        public void Clear()
        {
            if (_services != null && _services.Count > 0)
                _services.Clear();
        }

        /// <summary>
        /// Adds a new service to the resolver list
        /// </summary>
        /// <param name="type">Service Type (typically an interface)</param>
        /// <param name="value">Object that implements service</param>
        public void Add(Type type, object value)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (value == null)
                throw new ArgumentNullException("value");

            lock (_services)
            {
                // Replacing existing service
                if (_services.ContainsKey(type))
                    _services[type] = value;
                // Adding new service
                else
                    _services.Add(type, value);
            }
        }

        /// <summary>
        /// Remove a service
        /// </summary>
        /// <param name="type">Type to remove</param>
        public void Remove(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            lock (_services)
            {
                _services.Remove(type);
            }
        }

        /// <summary>
        /// This resolves a service type and returns the implementation. Note that this
        /// assumes the key used to register the object is of the appropriate type or
        /// this method will throw an InvalidCastException!
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Implementation</returns>
        public T Resolve<T>()
        {
            return (T)GetService(typeof(T));
        }

        /// <summary>
        /// Implementation of IServiceProvider
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <returns>Object implementing service</returns>
        public object GetService(Type serviceType)
        {
            lock (_services)
            {
                object value;
                return _services.TryGetValue(serviceType, out value) ? value : null;
            }
        }
        #endregion
    }
}
