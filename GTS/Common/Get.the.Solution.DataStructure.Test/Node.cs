using System;
using Get.the.Solution.DataStructure.Test.ExtendedModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Get.the.Solution.DataStructure.Test
{
    [TestClass]
    public class Node
    {
        [TestMethod]
        public void TestNodeSetLeft()
        {
            Node<int> node1 = new Node<int>();
            Node<int> node2 = new Node<int>();

            node1.SetLeft(node2);

            var testResult = node1.GetLeft();

            Assert.AreEqual(node2, testResult);

            //extended node class test
            SingleNodeExtended<int> node3extended = new SingleNodeExtended<int>();

            node2.SetRight(new SingleNode<int>());

        }
        [TestMethod]
        public void TestNodeSetRight()
        {
            Node<int> node1 = new Node<int>();
            Node<int> node2 = new Node<int>();

            node1.SetRight(node2);

            var testResult = node1.GetRight();

            Assert.AreEqual(node2, testResult);

        }
    }
}
