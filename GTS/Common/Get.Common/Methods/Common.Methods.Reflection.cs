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
        public class PropertySetterHelper<T> where T : class
        {
            private static Dictionary<string, PropertyInfo> _Props = new Dictionary<string, PropertyInfo>();

            private readonly T _Instance = null;

            static PropertySetterHelper()
            {
                foreach (var pi in typeof(T).GetProperties())
                {
                    _Props.Add(pi.Name, pi);
                }
            }

            public PropertySetterHelper(T pInstance)
            {
                if (pInstance == null) throw new ArgumentNullException("pInstance");
                _Instance = pInstance;
            }

            public void SetProperty(string pPropertyName, object PropertyValue)
            {
                _Props[pPropertyName].SetValue(_Instance, PropertyValue, null);
            }


        }

        public static MethodInfo GetMethodInfo<T>(String pMethodName)
        {
            Type type = typeof(T);
            MethodInfo methodInfo = type.GetMethods().Where(t => t.Name.Equals(pMethodName)).First();
            return methodInfo;
        }

    }
}
