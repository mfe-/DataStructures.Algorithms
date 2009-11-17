using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Get.Common.Methods
{
    public static class Modul
    {
        public static object GetModul(string pFileName, Type pTypeInterface)
        {
            //Assembly laden
            Assembly assembly = Assembly.LoadFrom(pFileName);
            // http://msdn.microsoft.com/de-de/library/t0cs7xez.aspx
            // Assembly Eigenschaften checken

            foreach (Type type in assembly.GetTypes())
                if (type.IsPublic) // Ruft einen Wert ab, der angibt, ob der Type als öffentlich deklariert ist. 
                    if (!type.IsAbstract)  //nur Assemblys verwenden die nicht Abstrakt sind
                    {
                        // Sucht die Schnittstelle mit dem angegebenen Namen. 
                        Type typeInterface = type.GetInterface(pTypeInterface.ToString(), true);

                        //Make sure the interface we want to use actually exists
                        if (typeInterface != null)
                        {
                            try
                            {
                                object activedInstance = Activator.CreateInstance(type);
                                return activedInstance;
                            }
                            catch (Exception exception)
                            {
                                System.Diagnostics.Debug.WriteLine(exception);
                            }
                        }

                        typeInterface = null;
                    }
            assembly = null;


            return null;
        }
    }
}
