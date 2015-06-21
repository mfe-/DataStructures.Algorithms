using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.Algorithms
{
    public static class Search
    {
        public static IEnumerable<Node<T>> InOrder<T>(this Node<T> p, IList<Node<T>> list)
        {
            if (p != null)
            {
                InOrder<T>(p.GetLeft(), list);
                list.Add(p);
                InOrder<T>(p.GetRight(), list);
            }
            return list;
        }
        public static IEnumerable<Node<T>> InOrder<T>(this Node<T> p)
        {
            return InOrder(p, new List<Node<T>>());
        }
    }
}
