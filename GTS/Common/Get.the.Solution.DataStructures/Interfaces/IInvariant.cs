using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Invarainz: Typen bleiben gleich
    /// </summary>
    /// <remarks>https://msdn.microsoft.com/en-us/library/dd997386.aspx</remarks>
    /// <typeparam name="T">The type which should be invariant</typeparam>
    public interface IInvariant<T> : ICovariant<T>, IContravariant<T> { }
}
