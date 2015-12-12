using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Kontravarianz: Obertyp-Beziehung: Parametertyp variiert GEGEN den Klassentypen 
    /// </summary>
    /// <remarks>https://msdn.microsoft.com/en-us/library/dd997386.aspx</remarks>
    /// <typeparam name="T">The type which should be contravariant</typeparam>
    public interface IContravariant<in T> { }
}
