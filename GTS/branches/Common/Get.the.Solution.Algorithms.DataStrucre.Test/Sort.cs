using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Get.the.Solution.Algorithms.Test
{
    [TestClass]
    public class Sort
    {
        [TestMethod]
        public void Quick_Sort()
        {
            var a = new int[] { 4, 1, 2, 3, 7, 8, 6, 5 }.Quick_Sort();
            var b = new int[] { 10, 11, 1, 0, 101, 111, 01, 00 }.Quick_Sort();

            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }.ToList(), a.ToList());
            CollectionAssert.AreEqual(new int[] { 0, 0, 1, 1, 10,11, 101, 111 }.ToList(), b.ToList());

        }
    }
}
