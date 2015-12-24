using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Text.RegularExpressions;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Validations", "Get.Common.Validations")]
namespace Get.Common.Validations
{
    public static class ValidationsList
    {
        public static IsIntegerValidationRule IsIntegerValidationRule = new IsIntegerValidationRule();
        public static IsEmailAdressValidationRule IsEmailAdressValidationRule = new IsEmailAdressValidationRule();

    }
    public class IsIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string inputString = value as string;

            if (null != inputString)
            {
                int inputNumber;
                if (false == int.TryParse(inputString, out inputNumber))
                {
                    return new ValidationResult(false, "Please enter a valid number.");
                }
            }

            return ValidationResult.ValidResult;
        }
    }
    public class IsEmailAdressValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            string inputString = value as string;

            if (null != inputString)
            {
                if (false == re.IsMatch(inputString))
                {
                    return new ValidationResult(false, "Geben Sie eine Gültige Email-Adresse ein.");
                }
            }

            return ValidationResult.ValidResult;
        }
    }

}
