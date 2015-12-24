using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Get.Common.Cinch
{
    /// <summary>
    /// The class all validating domain objects must inherit from. 
    /// Allows changes via a BeginEdit()/EndEdit() combination, and 
    /// provides rollbacks for cancels. This class also support validation
    /// rules that can be added using the AddRule()
    /// method, such as
    /// </summary>
    /// <example>
    /// <code>
    ///     //STEP 1:
    ///     //Declare a static field for the rule, so it is shared for all instance of the
    ///     //same type
    ///     private static SimpleRule quantityRule;
    ///     
    ///     //STEP 2:
    ///     //Set the rule up in the static constructor
    ///     static OrderModel()
    ///     {
    ///
    ///        quantityRule = new SimpleRule("DataValue", "Quantity can not be &lt; 0",
    ///                  (Object domainObject)=>
    ///                  {
    ///                      DataWrapper<Int32> obj = (DataWrapper<Int32>)domainObject;
    ///                      return obj.DataValue &lt;= 0;
    ///                  });
    ///     }
    ///     
    ///     //STEP 3:
    ///     //Add the rule in the instance constructor for the field you want to validate
    ///     public OrderModel()
    ///     {
    ///         Quantity = new DataWrapper<Int32>(this, quantityChangeArgs);
    ///         //declare the rules
    ///         quantity.AddRule(quantityRule);
    ///     }  
    /// </code>
    /// </example>
    [Serializable()]
    public class ValidatingObject : IDataErrorInfo, INotifyPropertyChanged, IParentablePropertyExposer
    {
        #region Data
        private List<Rule> rules = new List<Rule>();
        #endregion

        #region Public Methods/Properties

        /// <summary>
        /// Gets a value indicating whether or not this domain object is valid. 
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                return String.IsNullOrEmpty(this.Error);
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this domain object. 
        /// The default is an empty string ("").
        /// </summary>
        public virtual string Error
        {
            get
            {
                string result = this[string.Empty];
                if (result != null && result.Trim().Length == 0)
                {
                    result = null;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">The name of the property whose error 
        /// message to get.</param>
        /// <returns>The error message for the property. 
        /// The default is an empty string ("").</returns>
        public virtual string this[string propertyName]
        {
            get
            {
                string result = string.Empty;

                propertyName = CleanString(propertyName);

                foreach (Rule r in GetBrokenRules(propertyName))
                {
                    if (propertyName == string.Empty || r.PropertyName == propertyName)
                    {
                        result += r.Description;
                        result += Environment.NewLine;
                    }
                }
                result = result.Trim();
                if (result.Length == 0)
                {
                    result = null;
                }
                return result;
            }
        }

        /// <summary>
        /// Validates all rules on this domain object, returning a list of the broken rules.
        /// </summary>
        /// <returns>A read-only collection of rules that have been broken.</returns>
        public virtual ReadOnlyCollection<Rule> GetBrokenRules()
        {
            return GetBrokenRules(string.Empty);
        }

        /// <summary>
        /// Validates all rules on this domain object for a given property, 
        /// returning a list of the broken rules.
        /// </summary>
        /// <param name="property">The name of the property to check for. 
        /// If null or empty, all rules will be checked.</param>
        /// <returns>A read-only collection of rules that have been broken.</returns>
        public virtual ReadOnlyCollection<Rule> GetBrokenRules(string property)
        {
            property = CleanString(property);

            List<Rule> broken = new List<Rule>();

            foreach (Rule r in this.rules)
            {
                // Ensure we only validate a rule 
                if (r.PropertyName == property || property == string.Empty)
                {
                    bool isRuleBroken = r.ValidateRule(this);
                    Debug.WriteLine(DateTime.Now.ToLongTimeString() +
                        ": Validating the rule: '" + r.ToString() +
                        "' on object '" + this.ToString() + "'. Result = " +
                        ((isRuleBroken == false) ? "Valid" : "Broken"));

                    if (isRuleBroken)
                    {
                        broken.Add(r);
                    }
                }
            }

            return broken.AsReadOnly();
        }

 
        /// <summary>
        /// Adds a new rule to the list of rules
        /// </summary>
        /// <param name="newRule">The new rule</param>
        public void AddRule(Rule newRule)
        {
            this.rules.Add(newRule);
        }

        /// <summary>
        /// A helper method that raises the PropertyChanged event for a property.
        /// </summary>
        /// <param name="propertyNames">The names of the properties that changed.</param>
        protected virtual void NotifyChanged(params string[] propertyNames)
        {
            foreach (string name in propertyNames)
            {
                NotifyPropertyChanged(name);
            }
        }

        /// <summary>
        /// Cleans a string by ensuring it isn't null and trimming it.
        /// </summary>
        /// <param name="s">The string to clean.</param>
        protected string CleanString(string s)
        {
            return (s ?? string.Empty).Trim();
        }
        #endregion

        #region Debugging Aides

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify using pre-made PropertyChangedEventArgs
        /// </summary>
        /// <param name="args"></param>
        protected void NotifyPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Notify using String property name
        /// </summary>
        protected void NotifyPropertyChanged(String propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IParentablePropertyExposer
        /// <summary>
        /// Returns the list of delegates that are currently subscribed for the
        /// <see cref="System.ComponentModel.INotifyPropertyChanged">INotifyPropertyChanged</see>
        /// PropertyChanged event
        /// </summary>
        public Delegate[] GetINPCSubscribers()
        {
            return PropertyChanged == null ? null : PropertyChanged.GetInvocationList();
        }

        #endregion
    }
}
