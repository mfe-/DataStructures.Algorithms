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
            Node<int> node1 = new Node<int>() { Value = 1 };
            Node<int> node2 = new Node<int>() { Value = 2 };
            //test get Left node for extended class 1->2
            node1.Left = node2;
            var testResult6 = node1.Left;
            Assert.AreEqual(node2, testResult6);

            //assign node extended 1->2->3
            NodeExtended<int> nodeExtended = new NodeExtended<int>() { Value = 3 };
            node2.Left = nodeExtended;
            var testResult7 = node2.Left;
            Assert.AreEqual(nodeExtended, testResult7);

            //assign node 1->2->3->4
            Node<int> node3 = new Node<int>(4);
            nodeExtended.Left = node3;
            var testResult8 = nodeExtended.Left;
            Assert.AreEqual(node3, testResult8);

            //check references
            INode<int> startNode = node1;
            List<Type> typeList = new List<Type>();
            List<int> valueResult = new List<int>();
            typeList = new List<Type>();
            valueResult = new List<int>();

            while (startNode != null)
            {
                typeList.Add(startNode.GetType());
                valueResult.Add(startNode.Value);
                startNode = startNode.Left;
            }

            var expectedTypeValues = new List<Type>()
            {
                typeof(Node<int>),
                typeof(Node<int>),
                typeof(NodeExtended<int>),
                typeof(Node<int>)
            };
            var expectedValueValues = new List<int>()
            {
                1,
                2,
                3,
                4
            };

            CollectionAssert.AreEqual(typeList, expectedTypeValues);
            CollectionAssert.AreEqual(valueResult, expectedValueValues);

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

            //assign node to singlenode 3->1
            SingleNode<int> singlenode = new SingleNode<int>(3);
            singlenode.Right = node1;
            var testResult2 = singlenode.Right;
            Assert.AreEqual(node1, testResult2);

            //assign singlenode to singleNodeExtended 3->1->4
            SingleNodeExtended<int> node3Extended = new SingleNodeExtended<int>() { Value = 4 };
            singlenode.Right.Right = node3Extended;
            var testResult3 = singlenode.Right.Right;
            Assert.AreEqual(node3Extended, testResult3);

            //assign node to singlenode 3->1->4->5
            SingleNode<int> singlenode1 = new SingleNode<int>(5);
            node3Extended.Right = singlenode1;
            var testResult4 = node3Extended.Right;
            Assert.AreEqual(singlenode1, testResult4);

            //assign node to singlenode 3->1->4->5->2
            singlenode1.Right = node2;
            var testResult5 = singlenode1.Right;
            Assert.AreEqual(node2, testResult5);

            //check references
            ISingleNode<int> start = singlenode;
            List<Type> typeList = new List<Type>();
            List<int> valueResult = new List<int>();

            while (start != null)
            {
                typeList.Add(start.GetType());
                valueResult.Add(start.Value);
                start = start.Right;
            }

            List<Type> expectedTypeValues = new List<Type>()
            {
                typeof(SingleNode<int>),
                typeof(Node<int>),
                typeof(SingleNodeExtended<int>),
                typeof(SingleNode<int>),
                typeof(Node<int>)
            };
            List<int> expectedValueValues = new List<int>()
            {
                3,
                1,
                4,
                5,
                2
            };

            CollectionAssert.AreEqual(typeList, expectedTypeValues);
            CollectionAssert.AreEqual(valueResult, expectedValueValues);

            //test get Left node for extended class 1->2
            node1.Right = node2;
            var testResult6 = node1.Right;
            Assert.AreEqual(node2, testResult6);

            //assign node extended 1->2->3
            NodeExtended<int> nodeExtended = new NodeExtended<int>() { Value = 3 };
            node2.Right = nodeExtended;
            var testResult7 = node2.Right;
            Assert.AreEqual(nodeExtended, testResult7);

            //assign node 1->2->3->4
            Node<int> node3 = new Node<int>(4);
            nodeExtended.Right = node3;
            var testResult8 = nodeExtended.Right;
            Assert.AreEqual(node3, testResult8);

            //check references
            INode<int> startNode = node1;
            typeList = new List<Type>();
            valueResult = new List<int>();

            while (startNode != null)
            {
                typeList.Add(startNode.GetType());
                valueResult.Add(startNode.Value);
                startNode = startNode.Right;
            }

            expectedTypeValues = new List<Type>()
            {
                typeof(Node<int>),
                typeof(Node<int>),
                typeof(NodeExtended<int>),
                typeof(Node<int>)
            };
            expectedValueValues = new List<int>()
            {
                1,
                2,
                3,
                4
            };

            CollectionAssert.AreEqual(typeList, expectedTypeValues);
            CollectionAssert.AreEqual(valueResult, expectedValueValues);

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
