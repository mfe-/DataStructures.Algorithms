using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Get.Common.Cinch
{
    /// <summary>
    /// A small helper class that has a method to help create
    /// PropertyChangedEventArgs when using the INotifyPropertyChanged
    /// interface
    /// </summary>
    public static class ObservableHelper
    {
        #region Public Methods
        /// <summary>
        /// Creates PropertyChangedEventArgs
        /// </summary>
        /// <param name="propertyExpression">Expression to make 
        /// PropertyChangedEventArgs out of</param>
        /// <returns>PropertyChangedEventArgs</returns>
        public static PropertyChangedEventArgs CreateArgs<T>(
            Expression<Func<T, Object>> propertyExpression)
        {
            return new PropertyChangedEventArgs(
                GetPropertyName<T>(propertyExpression));
        }

        /// <summary>
        /// Creates PropertyChangedEventArgs
        /// </summary>
        /// <param name="propertyExpression">Expression to make 
        /// PropertyChangedEventArgs out of</param>
        /// <returns>PropertyChangedEventArgs</returns>
        public static string GetPropertyName<T>(
            Expression<Func<T, Object>> propertyExpression)
        {
            var lambda = propertyExpression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;

            return propertyInfo.Name;
        }

        #endregion
    }
}
