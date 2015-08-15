using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Get.the.Solution.Algorithms
{
    using Get.the.Solution.DataStructure;
    public static class Search
    {
        public static IEnumerable<INode<T>> InOrder<T>(this INode<T> p)
        {
            return InOrder(p, new List<INode<T>>());
        }
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

        public static IEnumerable<INode<T>> PreOrder<T>(this INode<T> p)
        {
            return PreOrder(p, new List<INode<T>>());
        }
        public static IEnumerable<INode<T>> PreOrder<T>(this INode<T> p, IList<INode<T>> list)
        {
            if (p != null)
            {
                list.Add(p);
                PreOrder<T>(p.Left, list);
                PreOrder<T>(p.Right, list);
            }
            return list;
        }
        public static IEnumerable<INode<T>> PostOrder<T>(this INode<T> p)
        {
            return PostOrder(p, new List<INode<T>>());
        }

        public static IEnumerable<INode<T>> PostOrder<T>(this INode<T> p, IList<INode<T>> list)
        {
            if (p != null)
            {
                PostOrder<T>(p.Left, list);
                PostOrder<T>(p.Right, list);
                list.Add(p);
            }
            return list;
        }
        public static ITreeNode<T> CreatePostOrderList<T>(this ITreeNode<T> p)
        {
            ITreeNode<T> temp = CreatePostOrderList<T>(p, null);
            temp.Left = null;
            temp = temp.Parent;
            return temp;
        }
        private static ITreeNode<T> CreatePostOrderList<T>(this ITreeNode<T> p, ITreeNode<T> currentNode)
        {
            if (p != null)
            {
                currentNode = CreatePostOrderList<T>(p.Left, currentNode);
                currentNode = CreatePostOrderList<T>(p.Right, currentNode);
                //first item
                if (currentNode == null)
                {
                    currentNode = p;
                    //save first element
                    currentNode.Parent = p;
                }
                else
                {
                    currentNode.Left = p;
                    p.Parent = currentNode.Parent;
                    p.Right = currentNode;
                    currentNode = p;
                }
            }
            return currentNode;
        }


        public static T BinarySearch<T>(this IEnumerable<T> A, T key) where T : IComparable<T>
        {
            return BinarySearch(A.ToArray(), key, 0, A.Count() - 1);
        }
        private static T BinarySearch<T>(this IList<T> A, T key, int l, int r) where T : IComparable<T>
        {
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            int m;
            do
            {
                m = (int)System.Math.Round((double)(l + r) / 2, 0);
                //key < A[m]
                if (key.CompareTo(A[m]) == -1)
                {
                    r = m - 1;
                }
                else
                {
                    l = m + 1;
                }
            }
            while (key.CompareTo(A[m]) != 0 || l.CompareTo(r) == -1);
            if (key.CompareTo(A[m]) == 0)
            {
                return A[m];
            }
            else
            {
                throw new ArgumentException();
            }

        }
        /// <summary>
        /// do not use - bad runtime
        /// </summary>
        /// <param name="A"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int FibonacciSearch(this IList<int> A, int s)
        {
            int p = 0, q = 0, i = 1;
            int prep = 0, preq = 0, prei = 0;
            while ((q + p) < A.Count)
            {
                q = p;
                p = i;
                i = q + p;
                if ((q + p) < A.Count)
                {
                    prep = p;
                    preq = q;
                    prei = i;
                }
            }

            return FibonacciSearch(A, s, prei, prep, preq);
        }

        public static int FibonacciSearch(this IList<int> A, int s, int i, int p, int q)
        {
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            System.Diagnostics.Debug.WriteLine("s={0},i={1}p={2}q={3}", s, i, p, q);
            if (s < A[i - 1])
            {
                if (q == 0)
                {
                    return 0;
                }
                else
                {
                    return FibonacciSearch(A, s, i - q, q, p - q);
                }

            }
            else if (s > A[i - 1])
            {
                if (p == 1)
                {
                    return 0;
                }
                else
                    return FibonacciSearch(A, s, i + q, p - q, (2 * q) - p);
            }
            else
            {

                return i - 1;
            }
        }
    }

}
