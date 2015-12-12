using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
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
    }
}
