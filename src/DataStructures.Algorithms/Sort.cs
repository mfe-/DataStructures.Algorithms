using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace DataStructures.Algorithms
{
    /// <summary>
    /// Contains sort extensions like <see cref="MergeSort{T}(T[])"/>, <see cref="Quick_Sort{T}(IList{T})"/>, <see cref="Insertion_Sort{T}(IList{T})"/>, ...
    /// </summary>
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
        /// Sorts a list ascending using insertion sort
        /// </summary>
        /// <remarks>
        /// Basically looksup for the proper element and moves to the left. All left elements behind the current element are sorted.  
        /// Use selection sort when moving data is cheap and comparison between keys is cheap.
        /// Stable: No
        /// </remarks>
        /// <typeparam name="T">Compareable type</typeparam>
        /// <param name="A">Collection to sort</param>
        /// <returns>Sorted list</returns>
        public static IEnumerable<T> Selection_Sort<T>(this IList<T> A) where T : IComparable<T>
        {
            for (int j = 0; j < A.Count; j++)
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
        /// Sorts a list ascending using insertion sort
        /// Less key comparision
        /// A lot data moving
        /// </summary>
        /// <remarks>
        /// Basically compares all keys behind the current position and move to the correct place
        /// Stable: Yes
        /// </remarks>
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

                if (Debugger.IsAttached)
                    Print(A);
            }
            return A;
        }
        /// <summary>
        /// Sorts a list ascending using merge sort
        /// </summary>
        /// <remarks>
        /// The merge sort divides the left part (l to m) of the collection and merges them.
        /// After that the right part (m+1 to r) of the collection will be devided and merged. 
        /// Requires additional theta(n) space.
        /// Stable: Yes
        /// </remarks>
        /// <typeparam name="T">Compareable data</typeparam>
        /// <param name="data">data collection to sort</param>
        /// <returns>The sorted liset</returns>
        public static IEnumerable<T> MergeSort<T>(this T[] data) where T : IComparable
        {
            return MergeSort(data, 0, data.Length - 1);
        }
        private static IEnumerable<T> MergeSort<T>(this T[] data, int l, int r) where T : IComparable
        {
            if (l < r)
            {
                int m = (l + r) / 2;
                MergeSort(data, l, m);
                MergeSort(data, m + 1, r);

                Merge(data, l, m, r);
            }
            return data;
        }
        private static IEnumerable<T> Merge<T>(this T[] data, int l, int m, int r) where T : IComparable
        {
            T[] temp = new T[data.Length];

            int i = l, h = l, j = m + 1;

            while (h <= m && j <= r)
            {
                //5.CompareTo(6) = -1      First int is smaller.
                //6.CompareTo(5) =  1      First int is larger.
                //5.CompareTo(5) =  0      Ints are equal.
                if (data[h].CompareTo(data[j]) == -1 || data[h].CompareTo(data[j]) == 0)
                {
                    temp[i] = data[h];
                    h++;
                }
                else
                {
                    temp[i] = data[j];
                    j++;
                }

                i++;
            }

            if (h > m)
                for (int k = j; k <= r; k++)
                    temp[i++] = data[k];
            else
                for (int k = h; k <= m; k++)
                    temp[i++] = data[k];

            for (int k = l; k <= r; k++)
            {
                data[k] = temp[k];
                if (Debugger.IsAttached)
                    System.Diagnostics.Debug.WriteLine(" " + data[k]);

            }
            if (Debugger.IsAttached) System.Diagnostics.Debug.WriteLine(" m " + m + System.Environment.NewLine);
            return data;
        }
        /// <summary>
        /// Sorts a list ascending using quick sort
        /// </summary>
        /// <remarks>
        /// best practice is to use quicksort for sorting random collections
        /// By default the A[r] element will be used as pivot element 
        /// Stable: No
        /// </remarks>
        /// <typeparam name="T">Compareable data</typeparam>
        /// <param name="A"></param>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns>Sorted list</returns>
        private static IEnumerable<T> Quick_Sort<T>(this IList<T> A, int l, int r) where T : IComparable<T>
        {
            if (l < r)
            {
                //if (Debugger.IsAttached)
                //  A.Print();
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
                    while ((A[i].CompareTo(x) == -1 || A[i].CompareTo(x) == 0) && i < r)
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
                        if (Debugger.IsAttached)
                            A.Print();
                    }
                }
                while (i < j);
                tmp = A[i];
                A[i] = A[r];
                A[r] = tmp;
                if (Debugger.IsAttached)
                    A.Print();

                int p = i;

                Quick_Sort(A, l, p - 1);
                Quick_Sort(A, p + 1, r);
            }
            return A;
        }
        public static IEnumerable<T> Quick_Sort<T>(this IList<T> A) where T : IComparable<T>
        {
            return Quick_Sort<T>(A, 0, A.Count - 1);
        }

        #region todo
        public static IEnumerable<T> HeapSort<T>(this IEnumerable<T> A) where T : IComparable<T>
        {
            CreateHeap(A, A.Count());
            for (int i = A.Count(); i > 2; i--)
            {
                Seep(A, 1, i - 1);
            }
            return A;

        }
        private static IEnumerable<T> Seep<T>(this IEnumerable<T> A, int i, int m) where T : IComparable<T>
        {
            int b = 2 * i;
            while (b <= m)
            {
                throw new NotImplementedException();
            }
            return A;
        }
        /// <summary>
        /// Create heap
        /// </summary>
        /// <typeparam name="T">Compareable data</typeparam>
        /// <param name="data">data collection to sort</param>
        /// <param name="n">maximum index n</param>
        /// <returns>The sorted liset</returns>
        public static IEnumerable<T> CreateHeap<T>(this IEnumerable<T> A, int n) where T : IComparable<T>
        {
            int m = (n) / 2;
            for (int i = m; i < 1; i--)
            {
                Seep(A, i, n);
            }
            return A;
        }
        #endregion


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
            Debug.WriteLine("");

            foreach (T v in A)
            {
                Debug.WriteLine(v + ", ");
            }

        }

    }
}
