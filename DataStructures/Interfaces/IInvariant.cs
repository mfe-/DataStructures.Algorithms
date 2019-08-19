using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Invarainz: Typen bleiben gleich
    /// </summary>
    /// <remarks>
    /// <seealso cref="http://stackoverflow.com/questions/26276231/set-property-on-a-covariant-interface">set-property-on-a-covariant-interface</seealso>
    /// <seealso cref="https://msdn.microsoft.com/en-us/library/dd997386.aspx">Creating Variant Generic Interfaces</seealso></remarks>
    /// <typeparam name="T">The type which should be invariant</typeparam>
    public interface IInvariant<T> : ICovariant<T>, IContravariant<T> { }
}
