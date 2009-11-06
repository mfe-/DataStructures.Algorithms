using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Get.Common
{
    public static class Const
    {
        /// <summary>
        /// Gibt das RoamingDir vom aktuell angemeldeten Benutzer zurück
        /// </summary>
        public static string RoamingDir
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    Path.DirectorySeparatorChar.ToString() +
                    (Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false).First() as AssemblyCompanyAttribute).Company
                    + Path.DirectorySeparatorChar.ToString();
            }
        }
        /// <summary>
        /// Gibt den Assemblytitel zurück
        /// </summary>
        /// <param name="pAssembly">Assembly aus der Informationen gelesen werden sollen.</param>
        /// <returns>Den Titel der Assembly</returns>
        public static string AssemblyTitle(Assembly pAssembly)
        {
            return (pAssembly.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false).First() as AssemblyTitleAttribute).Title;
        }
        /// <summary>
        /// Gibt den Assembly Productnamen zurück
        /// </summary>
        /// <param name="pAssembly">Assembly aus der Informationen gelesen werden sollen.</param>
        /// <returns>Den Titel der Assembly</returns>
        public static string AssemblyProduct(Assembly pAssembly)
        {
            return (pAssembly.GetCustomAttributes(typeof(System.Reflection.AssemblyProductAttribute), false).First() as AssemblyProductAttribute).Product;
        }


        private static readonly string _EnableLongPathString = @"\\?\";
        /// <summary>
        /// Gibt die Zeichen zurück um lange Pfade bei der win-api verwenden zu können
        /// http://blogs.msdn.com/bclteam/archive/2007/02/13/long-paths-in-net-part-1-of-3-kim-hamilton.aspx
        /// </summary>
        public static string EnableLongPathString
        {
            get
            {
                return _EnableLongPathString;
            }
        }
    }
}
