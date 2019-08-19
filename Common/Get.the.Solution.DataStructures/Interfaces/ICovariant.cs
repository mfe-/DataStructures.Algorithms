using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Kovarianz: UnterTyp-Beziehung: Paramtertyp variiert MIT dem Klassentyp (ko -mit)
    /// </summary>
    /// <remarks>https://msdn.microsoft.com/en-us/library/dd997386.aspx</remarks>
    /// <typeparam name="T">The type which should be Covariant</typeparam>
    public interface ICovariant<out T> { }
}
