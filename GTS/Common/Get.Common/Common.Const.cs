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
