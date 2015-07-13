using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Get.the.Solution.Algorithms
{
    using Get.the.Solution.DataStructure;
    public static class Search
    {
        public static IEnumerable<INode<T>> InOrder<T>(this INode<T> p, IList<INode<T>> list)
        {
            if (p != null)
            {
                InOrder(p.Left, list);
                list.Add(p);
                InOrder(p.Right, list);
            }
            return list;
        }
        public static IEnumerable<INode<T>> InOrder<T>(this INode<T> p)
        {
            return InOrder(p, new List<INode<T>>());
        }

    }
}
