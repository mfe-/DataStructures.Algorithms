using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A simple delegate rule may be used to validate objects
/// that inherit from <see cref="Cinch.ValidatingObject">
/// Cinch.ValidatingObject</see>
/// </summary>
namespace Get.Common.Cinch
{
    /// <summary>
    ///  A class to define a simple rule, using a delegate for validation.
    /// </summary>
    /// <remarks>
    /// Recommended usage:
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
    /// </remarks>
    public class SimpleRule : Rule
    {
        #region Data
        private Func<Object,Boolean> ruleDelegate;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propertyName">The name of the property this rule validates for. This may be blank.</param>
        /// <param name="brokenDescription">A description message to show if the rule has been broken.</param>
        /// <param name="ruleDelegate">A delegate that takes an Object parameter and returns a boolean value, used to validate the rule.</param>
        public SimpleRule(string propertyName, string brokenDescription, Func<Object, Boolean> ruleDelegate) :
            base(propertyName, brokenDescription)
        {
            this.RuleDelegate = ruleDelegate;
        }
        #endregion

        #region Public Methods/Properties
        /// <summary>
        /// Gets or sets the delegate used to validate this rule.
        /// </summary>
        protected virtual Func<Object, Boolean> RuleDelegate
        {
            get { return ruleDelegate; }
            set { ruleDelegate = value; }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Validates that the rule has not been broken.
        /// </summary>
        /// <param name="domainObject">The domain object being validated.</param>
        /// <returns>True if the rule has not been broken, or false if it has.</returns>
        public override bool ValidateRule(Object domainObject)
        {
            return RuleDelegate(domainObject);
        }
        #endregion
    }
}
