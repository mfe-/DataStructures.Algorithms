using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    //Beim Definieren von neuen Untertypen kann man bestehnde Typen verändern

    /// <summary>
    /// Kovarianz: UnterTyp-Beziehung: Paramtertyp variiert MIT dem Klassentyp (ko -mit)
    /// </summary>
    /// <remarks>https://msdn.microsoft.com/en-us/library/dd997386.aspx</remarks>
    /// <typeparam name="T">The type which should be Covariant</typeparam>
    public interface ICovariant<out T> { }
    /// <summary>
    /// Kontravarianz: Obertyp-Beziehung: Parametertyp variiert GEGEN den Klassentypen 
    /// </summary>
    /// <remarks>https://msdn.microsoft.com/en-us/library/dd997386.aspx</remarks>
    /// <typeparam name="T">The type which should be contravariant</typeparam>
    public interface IContravariant<in T> { }
    /// <summary>
    /// Invarainz: Typen bleiben gleich
    /// </summary>
    /// <remarks>https://msdn.microsoft.com/en-us/library/dd997386.aspx</remarks>
    /// <typeparam name="T">The type which should be invariant</typeparam>
    public interface IInvariant<T> : ICovariant<T>, IContravariant<T> { }
}
