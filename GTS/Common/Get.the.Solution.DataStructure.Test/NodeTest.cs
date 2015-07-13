using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Get.the.Solution.DataStructure.Test
{
    using Get.the.Solution.DataStructure.Test.ExtendedModel;
    using Get.the.Solution.DataStructure.Test.Extensions;
    using System.Collections.Generic;

    [TestClass]
    public class Node
    {
        [TestMethod]
        public void TestNodeSetLeft()
        {
          

        }
        [TestMethod]
        public void TestNodeSetRight()
        {
            Node<int> node1 = new Node<int>() { Value = 1 };
            Node<int> node2 = new Node<int>() { Value = 2 };

            //test get Left node
            node1.Left = (node2);
            var testResult1 = node1.Left;
            Assert.AreEqual(node2, testResult1);

            //test call extension on base node - should compileable
            node1.Left.Travers();

            //assign node to singlenode
            SingleNode<int> singlenode = new SingleNode<int>(3);
            singlenode.Right = node1;
            var testResult2 = singlenode.Right;
            Assert.AreEqual(node1, testResult2);

            //assign singlenode to singleNodeExtended
            SingleNodeExtended<int> node3Extended = new SingleNodeExtended<int>() { Value = 3 };
            singlenode.Right = node3Extended;
            var testResult3 = singlenode.Right;
            Assert.AreEqual(node3Extended, testResult3);

            SingleNode<int> singlenode1 = new SingleNode<int>(3);
            node3Extended.Right = singlenode1;
            var testResult4 = node3Extended.Right;
            Assert.AreEqual(singlenode1, testResult4);

            //assign node to singlenode
            singlenode1.Right = node1;
            var testResult5 = singlenode1.Right;
            Assert.AreEqual(node1, testResult5);

            //check for types
            //SingleNode<int> start = singlenode;
            //List<String> typeList = new List<string>();

            //while (start != null)
            //{
            //    typeList.Add(start.GetType().ToString());
            //    start.Right = start;
            //}

            //test get Left node for extended class
            node1.Right = node2;
            var testResult6 = node1.Right;
            Assert.AreEqual(node2, testResult6);

            NodeExtended<int> nodeExtended = new NodeExtended<int>() { Value = 3 };
            node2.Right = nodeExtended;
            var testResult7 = node2.Right;
            Assert.AreEqual(nodeExtended, testResult7);

            Node<int> node3 = new Node<int>();
            nodeExtended.Right = node3;
            var testResult8 = nodeExtended.Right;
            Assert.AreEqual(node3, testResult8);

        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
    }
}
