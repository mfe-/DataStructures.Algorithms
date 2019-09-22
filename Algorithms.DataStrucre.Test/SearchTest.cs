//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Get.the.Solution.Algorithms.Test
//{
//    [TestClass]
//    public class SearchTest : BaseTest
//    {

//        [TestMethod]
//        public void TestBinarySearch()
//        {
//            var expected = 91;
//            var result = new int[] { 1, 6, 14, 21, 26, 31, 46, 76, 81, 86, 91, 95, 98 }.BinarySearch(91);
//            Assert.AreEqual(expected, result);

//            //var data = base.RandomList<int>();
//            //expected = data[3];
//            ////binary search expects a ascending sorted list
//            //result = data.OrderBy(a => a).BinarySearch(expected);

//            Assert.AreEqual(result, expected);
//        }
//        [TestMethod]
//        public void TestFibonacciSearch()
//        {
//            var expected = 59;
//            var list = new int[] { 2, 4, 5, 13, 14, 20, 27, 28, 33, 44, 45, 50, 51, 57, 59, 60, 64, 65, 78, 80 };
//            var result = list.FibonacciSearch(59, 12, 7, 4);
//            Assert.AreEqual(expected, list[result]);

//            result = list.FibonacciSearch(59);
//            Assert.AreEqual(expected, list[result]);

//            //list = base.RandomList<int>().ToArray();
//            //expected = list[list.Count() - 1];
//            //result = list.FibonacciSearch(expected);
//            //Assert.AreEqual(expected, list[result]);

//        }
//    }
//}
