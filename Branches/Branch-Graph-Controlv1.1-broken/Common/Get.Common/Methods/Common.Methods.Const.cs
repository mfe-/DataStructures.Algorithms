using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml", "Get.Common")]
namespace Get.Common
{
    public static class Const
    {

        /// <summary>
        /// Erzeugt einen zufälligen String
        /// </summary>
        /// <param name="size">Wie lang der String sein soll.</param>
        /// <param name="lowerCase">Nur kleine Buchstaben aktivieren</param>
        /// <returns>Gibt einen zufällig generierten string zurück</returns>
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        /// <summary>
        /// Gibt den Pfad zurück in der die ausgeführe Exe liegt.
        /// http://www.mycsharp.de/wbb2/thread.php?threadid=54102
        /// </summary>
        public static string ApplicationDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString();
            }
        }
        /// <summary>
        /// Gibt den Pfad aller Benutzer zurück.
        /// </summary>
        public static string AllUserDir
        {
            get
            {
                return Environment.GetEnvironmentVariable("ALLUSERSPROFILE") + Path.DirectorySeparatorChar
                    + (Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false).First() as AssemblyCompanyAttribute).Company;
            }
        }
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
        /// <param name="pAssembly">Assembly aus der Informationen gelesen werden sollen. Z.B. System.Reflection.Assembly.GetExecutingAssembly()</param>
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
