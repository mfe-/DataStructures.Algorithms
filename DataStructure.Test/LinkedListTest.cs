using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Get.the.Solution.DataStructure.Test
{
    [TestClass]
    public class LinkedListTest
    {
        [TestMethod]
        public void TestRemove()
        {
            LinkedList<int> linkedlist = new LinkedList<int>();
            //remove none existing element
            var result = linkedlist.Remove(1);

            Assert.AreEqual(result, false);
            Assert.AreEqual(linkedlist.Count, 0);
            Assert.AreEqual(linkedlist.First, null);


            linkedlist.Add(1);

            Assert.AreEqual(linkedlist.First.Value, 1);
            Assert.AreEqual(linkedlist.Count, 1);

            //remove the only one existing first element 
            result = linkedlist.Remove(1);

            Assert.AreEqual(result, true);
            Assert.AreEqual(linkedlist.Count, 0);
            Assert.AreEqual(linkedlist.First, null);

            linkedlist.Add(1);
            linkedlist.Add(2);

            //remove the last element 
            result = linkedlist.Remove(2);

            Assert.AreEqual(result, true);
            Assert.AreEqual(linkedlist.Count, 1);
            Assert.AreEqual(linkedlist.First.Value, 1);
            Assert.AreEqual(linkedlist.Last.Value, 1);

            linkedlist.Add(3);
            linkedlist.Add(4);

            //remove a element in the middle
            result = linkedlist.Remove(3);

            Assert.AreEqual(result, true);
            Assert.AreEqual(linkedlist.Count, 2);
            Assert.AreEqual(linkedlist.First.Value, 1);
            Assert.AreEqual(linkedlist.Last.Value, 4);



        }
        [TestMethod]
        public void TestAdd()
        {
            LinkedList<int> linkedlist = new LinkedList<int>();
            linkedlist.Add(1);
            linkedlist.Add(2);
            linkedlist.Add(3);

            LinkedList<int> linkedlist1 = GenerateList(100, true);
            LinkedList<int> linkedlist2 = GenerateList(999, true);


            LinkedList<String> linkedListString = new LinkedList<String>();

            var testStrings = new String[]{ "Safe", "and", "Simple", null, "Life's", "a", "Game", null, 
										"Pumping", "Out", "the", "Same", "Old", "Same", null};

            foreach (String s in testStrings)
                linkedListString.Add(s);

            Assert.AreEqual(linkedListString.Count,testStrings.Length);

            //requries CopyTo
            //var linkedListResultList = linkedListString.ToList();
            //for (int i = 0; i < linkedListString.Count; i++)
            //    Assert.AreEqual(((string)linkedListResultList[i]) ,testStrings[i], "TestLinkedListAdd: Failed - Objects in list are not same.");


            //linkedListString.Add("Safe");
            //linkedListString.Add("and");
            //linkedListString.Add("Simple");
            //linkedListString.Add(null);
            //linkedListString.Add("Life's");
            //linkedListString.Add("a");
            //linkedListString.Add("Game");
            //linkedListString.Add(null);
            //linkedListString.Add("Pumping");
            //linkedListString.Add("Out");
            //linkedListString.Add("the");
            //linkedListString.Add("Same");
            //linkedListString.Add("Old");
            //linkedListString.Add("Same");
            //linkedListString.Add(null);

        }
        [TestMethod]
        public void TestGetEnumerator()
        {
            List<int> expectedList = new List<int>()
            {
                1,
                2,
                3
            };

            LinkedList<int> linkedlist = new LinkedList<int>();
            linkedlist.Add(1);
            linkedlist.Add(2);
            linkedlist.Add(3);

            List<int> result = new List<int>();
            //foreach requires IEnumerable to be implemented
            foreach (var item in linkedlist)
            {
                result.Add(item);
            }

            CollectionAssert.AreEqual(expectedList, result);

            linkedlist = new LinkedList<int>();
            result = new List<int>();
            expectedList = GenerateArray(541, true).ToList();

            foreach (int value in expectedList)
            {
                linkedlist.Add(value);
            }

            foreach (int value in linkedlist)
            {
                result.Add(value);
            }

            CollectionAssert.AreEqual(expectedList, result);
        }
        private int[] GenerateArray(int maxItems, bool randomNumbers = true)
        {
            int[] array = new int[maxItems];
            for (int i = 0; i < maxItems; i++)
            {
                if (randomNumbers == true)
                {
                    Random rnd = new Random();
                    array[i] = rnd.Next(0, maxItems);
                }
                else
                {
                    array[i] = i;
                }

            }
            return array;
        }
        private LinkedList<int> GenerateList(int maxItems, bool randomNumbers = true)
        {
            LinkedList<int> linkedlist = new LinkedList<int>();
            int[] values = GenerateArray(maxItems, randomNumbers);
            for (int i = 0; i < maxItems; i++)
            {
                linkedlist.Add(values[i]);
            }
            return linkedlist;
        }
    }
}
