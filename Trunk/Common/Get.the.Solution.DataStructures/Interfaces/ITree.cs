using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Represents a tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [InheritedExport]
    public interface ITree<T> where T : IComparable
    {
        bool Empty {get;}
        int Length {get;}
        int Height { get; }
        T FindIndex(int k);
        int IndexOf(T t);
        void Add(T t);
        void Remove(T val);
        bool Exists(T val);

        void Clear();
        void CopyTo(T[] array, int index);

    }
}
