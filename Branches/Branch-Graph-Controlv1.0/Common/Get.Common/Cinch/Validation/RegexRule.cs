using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// A simple RegEx rule may be used to validate objects
/// that inherit from <see cref="Cinch.ValidatingObject">
/// Cinch.ValidatingObject</see>
/// </summary>
namespace Get.Common.Cinch
{
    /// <summary>
    /// A class to define a RegEx rule, using a delegate for validation.
    /// </summary>
    public class RegexRule : Rule
    {
        #region Data
        private string regex;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructor.
        /// </summary>
        public RegexRule(string propertyName, string description, string regex)
            : base(propertyName, description)
        {
            this.regex = regex;
        }
        #endregion

        #region Overrides
        public override bool ValidateRule(Object domainObject)
        {
            PropertyInfo pi = domainObject.GetType().GetProperty(this.PropertyName);
            String value = pi.GetValue(domainObject, null) as String;
            if (!String.IsNullOrEmpty(value))
            {
                Match m = Regex.Match(value, regex);
                return !m.Success;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
