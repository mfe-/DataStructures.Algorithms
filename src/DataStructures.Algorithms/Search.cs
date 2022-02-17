using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Algorithms
{
    /// <summary>
    /// Contains search extensions like <see cref="BinarySearch{T}(IEnumerable{T}, T)"/>
    /// </summary>
    public static class Search
    {
        /// <summary>
        /// InOrder stack based implementation - Root is placed in the middle of the list. Elements will be returned ascending
        /// </summary>
        /// <typeparam name="TNode">A Treenode which supports traversing Parent Left and Right</typeparam>
        /// <param name="p">The Node to start</param>
        /// <returns>ascending elements</returns>
        public static IEnumerable<INodeParent<TNode>> InOrder<TNode>(this INodeParent<TNode> p) 
            where TNode : class, INodeParent<TNode>, INode<TNode>
        {
            Stack<INodeParent<TNode>> s = new Stack<INodeParent<TNode>>();
            INodeParent<TNode>? current = p;

            // traverse the tree  
            while (current != null || s.Count > 0)
            {
                // Reach the left most Node of the  curr Node 
                while (current != null)
                {
                    // place pointer to a tree node on the stack before traversing  
                    // the node's left subtree
                    s.Push(current);
                    current = current.V;
                }
                current = s.Pop();
                yield return current;
                // we have visited the node and its left subtree. 
                // Now, it's right subtree's turn
                current = current.U;
            }
        }
        /// <summary>
        /// PreOrder recursiv implementation - The first element is the root node
        /// </summary>
        /// <typeparam name="TNode">A Treenode which supports traversing Parent Left and Right</typeparam>
        /// <param name="p">The Node to start</param>
        /// <returns></returns>
        public static IEnumerable<INodeParent<TNode>> PreOrder<TNode>(this INodeParent<TNode> p) 
            where TNode : class, INodeParent<TNode>, INode<TNode>
        {
            return PreOrder(p, new List<INodeParent<TNode>>());
        }
        internal static IEnumerable<INodeParent<TNode>> PreOrder<TNode>(this INodeParent<TNode>? p, IList<INodeParent<TNode>> list) 
            where TNode : class, INodeParent<TNode>, INode<TNode>
        {
            if (p != null)
            {
                list.Add(p);
                PreOrder(p.V, list);
                PreOrder(p.U, list);
            }
            return list;
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
