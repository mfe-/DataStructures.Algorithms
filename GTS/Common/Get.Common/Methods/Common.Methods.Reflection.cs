using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Markup;


namespace Get.Common
{
    public static class Reflection
    {
        public class PropertySetterHelper
        {
            public PropertySetterHelper(object pInstance)
            {
                if (pInstance == null)
                    throw new ArgumentException("Cant be null pInstance");

                _Instance = pInstance;
            }

            public void SetProperty(string pPropertyName, object PropertyValue)
            {
                Type type = _Instance.GetType();

                PropertyInfo propertyInfo = type.GetProperties().ToList().Where(a => a.Name.Equals(pPropertyName)).First();
                if (propertyInfo == null) return;
                propertyInfo.SetValue(_Instance, PropertyValue, null);
            }

            private readonly object _Instance = null;

        }

    }
}
