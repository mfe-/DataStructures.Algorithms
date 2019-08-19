using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="D">The Type of the Data which should be used in the DataStructure</typeparam>
    public interface IData<out D> : ICovariant<D>
    {
        D Value { get; }
    }
}
