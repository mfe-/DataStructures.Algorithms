using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Get.Common.Methods
{
    public static class Modul
    {
        /// <summary>
        /// Ladet die Module aus der übergebenen Assembly
        /// </summary>
        /// <param name="pFileName">Assembly die verwendet werden soll.</param>
        /// <param name="pTypeInterface">Welches Interface das Modul implementierrt hat.</param>
        /// <returns>Gibt ein Dictionary zurück. Als Key wird der Klassenname der aktivierten Instanz verwendet.</returns>
        public static Dictionary<string, object> GetModul(string pFileName, Type pTypeInterface)
        {
            //Assembly laden
            Assembly assembly = Assembly.LoadFrom(pFileName);
            // http://msdn.microsoft.com/de-de/library/t0cs7xez.aspx
            // Assembly Eigenschaften checken

            Dictionary<string, object> interfaceinstances = new Dictionary<string,object>();

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
                                if (activedInstance != null)
                                {
                                    interfaceinstances.Add(type.Name, activedInstance);
                                }
                            }
                            catch (Exception exception)
                            {
                                System.Diagnostics.Debug.WriteLine(exception);
                            }
                        }

                        typeInterface = null;
                    }
            assembly = null;


            return interfaceinstances;
        }
    }
}
