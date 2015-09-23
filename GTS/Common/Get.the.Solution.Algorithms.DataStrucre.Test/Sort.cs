using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.Algorithms.Test
{

    [TestClass]
    public class Sort
    {
        public IList<int> RandomList<T>() where T : IComparable<T>
        {
            Random rand = new Random();
            List<int> result = new List<int>();
            HashSet<int> check = new HashSet<int>();
            for (Int32 i = 0; i < 300; i++)
            {
                int curValue = rand.Next(1, 100000);
                while (check.Contains(curValue))
                {
                    curValue = rand.Next(1, 100000);
                }
                result.Add(curValue);
                check.Add(curValue);
            }
            return result;
        }
        [TestMethod]
        public void TestInsertion_Sort()
        {
            var expected = new int[] { 1, 2, 3, 4, 5, 6 };
            var result = new int[] { 5, 2, 4, 6, 1, 3 }.Insertion_Sort();
            CollectionAssert.AreEqual(expected, result.ToArray());

            expected = new int[] { 2, 5, 6, 9, 11, 12 };
            result = new int[] { 6, 9, 12, 2, 11, 5 }.Insertion_Sort();
            CollectionAssert.AreEqual(expected, result.ToArray());

            expected = new int[] { 0, 0, 1, 2, 5, 6, 7 };
            result = new int[] { 6, 1, 2, 0, 0, 5, 7 }.Insertion_Sort();
            CollectionAssert.AreEqual(expected, result.ToArray());
        }
        [TestMethod]
        public void TestSelection_Sort()
        {
            var expected = new int[] { 1, 2, 3, 4, 5, 6 };
            var result = new int[] { 5, 2, 4, 6, 1, 3 }.Selection_Sort();
            CollectionAssert.AreEqual(expected, result.ToArray());

            expected = new int[] { 0, 2, 2, 5, 12, 34, 59, 87 };
            result = new int[] { 59, 5, 12, 34, 87, 0, 2, 2 }.Selection_Sort();
            CollectionAssert.AreEqual(expected, result.ToArray());

            expected = new int[] { 2, 5, 6, 9, 11, 12 };
            result = new int[] { 6, 9, 12, 2, 11, 5 }.Selection_Sort();
            CollectionAssert.AreEqual(expected, result.ToArray());

            expected = new int[] { 0, 0, 1, 2, 5, 6, 7 };
            result = new int[] { 6, 1, 2, 0, 0, 5, 7 }.Selection_Sort();
            CollectionAssert.AreEqual(expected, result.ToArray());
        }
        [TestMethod]
        public void TestQuick_Sort()
        {
            var expected = new int[] { 1, 1, 1, 11, 110, 111, 111 };
            var result = new int[] { 111, 11, 1, 1, 110, 111, 1 }.Quick_Sort();
            CollectionAssert.AreEqual(expected, result.ToArray());
        }
        [TestMethod]
        public void TestMerge_Srot()
        {
            var expected = new int[] { 1, 2, 2, 3, 4, 5, 6, 6 };
            var result = new int[] { 5, 2, 4, 6, 1, 3, 2, 6 }.MergeSort();
            CollectionAssert.AreEqual(expected, result.ToArray());

            expected = new int[] { 1, 7, 11, 14, 26, 33, 45, 65, 81 };
            result = new int[] { 1, 14, 81, 45, 65, 33, 11, 26, 7 }.MergeSort();
            CollectionAssert.AreEqual(expected, result.ToArray());
        }
        [TestMethod]
        public void TestSub_Sort()
        {
            var expected = new int[] { 1, 1, 5, 5, 9 };
            var result = new int[] { 1, 9, 5, 5, 1 }.Sub_Sort();

            CollectionAssert.AreEqual(expected, result.ToArray());
        }

        [TestMethod]
        public void TestTranspose_Sort()
        {
            var expected = new int[] { 1, 1, 5, 5, 9 };
            var result = new int[] { 1, 9, 5, 5, 1 }.Transpose_Sort();

            CollectionAssert.AreEqual(expected, result.ToArray());

            expected = this.RandomList<int>().ToArray();
            result = expected.Transpose_Sort();

            expected.ToList().Sort();

            CollectionAssert.AreEqual(expected, result.ToArray());


        }
    }
}
