using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.DataStructure;

namespace Get.Algorithms
{
    public static class Search
    {
        public static IEnumerable<INode<T>> InOrder<T>(this INode<T> p, IList<INode<T>> list)
        {
            if (p != null)
            {
                InOrder<T>(p.Left, list);
                list.Add(p);
                InOrder<T>(p.Right, list);
            }
            return list;
        }
        public static IEnumerable<INode<T>> InOrder<T>(this INode<T> p)
        {
            return InOrder(p, new List<INode<T>>());
        }
    }
}
