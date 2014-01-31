using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Get.Algorithms
{
    public static class Sort
    {
        public static IEnumerable<T> Selection_Sort<T>(this IEnumerable<T> A) where T : IComparable<T>
        {
            for (int j = 0; j < A.Count(); j++)
            {
                int minpos = j;
                for (int i = j + 1; j < A.Count(); i++)
                {
                    if (A.ToArray()[i].CompareTo(A.ToArray()[minpos]) == -1) // a<b equals to a.compare(b)==-1
                        minpos = i;
                }
                if (minpos > j)
                {
                    //vertauschen
                }
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
                int j = r-1;
                T tmp;
                do
                {
                    //5.CompareTo(6) = -1      First int is smaller.
                    //6.CompareTo(5) =  1      First int is larger.
                    //5.CompareTo(5) =  0      Ints are equal.
                    while ((A[i].CompareTo(x)==-1 || A.ToArray()[i].CompareTo(x)==0) && i < r) 
                    {
                        i = i + 1;
                    }
                    while (A[j].CompareTo(x)==1 && j > l)
                    {
                        j=j-1;
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

                Quick_Sort(A, l, p-1);
                Quick_Sort(A, p+1, r);
            }
            return A;
        }
        public static IEnumerable<T> Quick_Sort<T>(this IEnumerable<T> A) where T : IComparable<T>
        {
            return Quick_Sort<T>(A.ToList(), 0, A.Count()-1);
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
                    Debug.Write(v+", ");
                }
                else
                {
                    System.Console.Write(v+", ");
                }
            }

        }

    }
}
