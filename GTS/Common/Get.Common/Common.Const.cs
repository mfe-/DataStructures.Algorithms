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
    }
}
