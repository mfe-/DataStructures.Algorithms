﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Get.the.Solution.Algorithms
{
    public static class Sort
    {
        /// <summary>
        /// von algodat übung 1 ss15 beispiel 8 ähnlich wie insertion sort. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A"></param>
        /// <returns></returns>
        public static IEnumerable<T> Sub_Sort<T>(this IList<T> A) where T : IComparable<T>
        {
            for (int j = 1; j < A.Count; j++)
            {
                A.Print();
                Sub_Sort(A, j - 1, A[j]);
            }
            return A;
        }
        private static IEnumerable<T> Sub_Sort<T>(this IList<T> A, int i, T k) where T : IComparable<T>
        {
            //assign k to A[i + 1] when greater else use A[i]
            A[i + 1] = k.CompareTo(A[i]) == 1 ? k : A[i];
            //A[i]>k
            if (A[i].CompareTo(k) == 1) //change compareTo==-1 and ? A[i] : k; for descending
            {
                if (i == 0)
                {
                    A[i] = k;
                }
                else
                {
                    A.Print();
                    Sub_Sort(A, i - 1, k);
                }
            }
            return A;
        }
        public static IEnumerable<T> Transpose_Sort<T>(this IList<T> A) where T : IComparable<T>
        {
            if (A.Count < 1)
            {
                return A;
            }
            for (int i = 0; i < A.Count - 1; i++)
            {
                Transpose_Sort(A, i);
            }
            return A;
        }
        private static IEnumerable<T> Transpose_Sort<T>(this IList<T> A, int i) where T : IComparable<T>
        {
            if (A[i].CompareTo(A[i + 1]) != -1) // a<b equals to a.compare(b)==-1
            {
                //transpose a[i] and a[j]
                T tmp = A[i];
                A[i] = A[i + 1];
                A[i + 1] = tmp;
                if (i > 0)
                {
                    Transpose_Sort(A, i - 1);
                }
            }
            return A;
        }
        /// <summary>
        /// Basically looksup for the proper element and moves to the left. All left elements behind the current element are sorted.  
        /// Less data moving
        /// A lot key comparision
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A"></param>
        /// <returns></returns>
        public static IEnumerable<T> Selection_Sort<T>(this IList<T> A) where T : IComparable<T>
        {
            for (int j = 0; j < A.Count(); j++)
            {
                //determine position of minimum key
                int minpos = j;
                for (int i = j + 1; i < A.Count(); i++)
                {
                    if (A[i].CompareTo(A[minpos]) == -1) // a<b equals to a.compare(b)==-1
                    {
                        minpos = i;
                    }
                }

                if (minpos > j)
                {
                    //transpose a[i] and a[j]
                    T tmp = A[minpos];
                    A[minpos] = A[j];
                    A[j] = tmp;
                }
                A.Print();
            }

            return A;
        }
        /// <summary>
        /// Basically compares all keys behind the current position and move to the correct place
        /// Less key comparision
        /// A lot data moving
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A"></param>
        /// <returns></returns>
        public static IEnumerable<T> Insertion_Sort<T>(this IList<T> A) where T : IComparable<T>
        {
            T key;
            int i;
            for (int j = 1; j < A.Count; j++)
            {
                key = A[j];
                //insert A[j] in sorted sequence A[0],...,A[j-1]
                i = j - 1;
                while (i >= 0 && A[i].CompareTo(key) == 1)
                {
                    //shift elements backward and calculate position of the new key
                    A[i + 1] = A[i];
                    i = i - 1;
                }

                A[i + 1] = key;
                Print(A);
            }
            return A;
        }
        private static IEnumerable<T> Quick_Sort<T>(this IList<T> A, int l, int r) where T : IComparable<T>
        {
            if (l < r)
            {
                T x = A[r];
                //partition
                int i = l;
                int j = r - 1;
                T tmp;
                do
                {
                    //5.CompareTo(6) = -1      First int is smaller.
                    //6.CompareTo(5) =  1      First int is larger.
                    //5.CompareTo(5) =  0      Ints are equal.
                    while ((A[i].CompareTo(x) == -1 || A.ToArray()[i].CompareTo(x) == 0) && i < r)
                    {
                        i = i + 1;
                    }
                    while (A[j].CompareTo(x) == 1 && j > l)
                    {
                        j = j - 1;
                    }

                    if (i < j)
                    {
                        //transpose a[i] and a[j]
                        tmp = A[i];
                        A[i] = A[j];
                        A[j] = tmp;
                        A.Print();
                    }
                }
                while (i < j);
                tmp = A[i];
                A[i] = A[r];
                A[r] = tmp;
                A.Print();

                int p = i;

                Quick_Sort(A, l, p - 1);
                Quick_Sort(A, p + 1, r);
            }
            return A;
        }
        public static IEnumerable<T> Quick_Sort<T>(this IEnumerable<T> A) where T : IComparable<T>
        {
            return Quick_Sort<T>(A.ToList(), 0, A.Count() - 1);
        }

        public static T Min<T>(params T[] values) where T : IComparable<T>
        {
            T min = values[0];
            foreach (var item in values.Skip(1))
            {
                if (item.CompareTo(min) < 0)
                    min = item;
            }
            return min;
        }
        public static void Print<T>(this IEnumerable<T> A)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine("");
            }
            else
            {
                System.Console.WriteLine("");
            }
            foreach (T v in A)
            {
                if (Debugger.IsAttached)
                {
                    Debug.Write(v + ", ");
                }
                else
                {
                    System.Console.Write(v + ", ");
                }
            }

        }

    }
}