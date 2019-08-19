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
        public void TestDataICovariant()
        {
            //this shold compile
            //ISingleNode<FooBar> node1 = new SingleNode<FooBar>(new FooBar());
            //ISingleNode<Foo> node2 = new SingleNode<Foo>(new Foo());
   
            //new SingleNode<FooBar>(new FooBar()).Right = new SingleNode<Foo>(null);
            //node1.Right = node2;

            //Assert.AreEqual("FooBar", node2.Value.ToString());


            List<Foo> bla = new List<Foo>();
            bla.Add(new FooBar());

            //http://stackoverflow.com/questions/10956993/out-t-vs-t-in-generics
            //The classic example is IEnumerable<out T>.Since IEnumerable <out T > is covariant, you're allowed to do the following:
            IEnumerable<string> strings = new List<string>();
            IEnumerable<object> objects = strings;

            List<object> ieobject = new List<object>();
            ieobject.Add("");
            ieobject.Add(2);

            DataStructure.LinkedList<object> list = new DataStructure.LinkedList<object>();
            list.Add("");
            list.Add(2);

            DataStructure.LinkedList<Foo> listfoo = new DataStructure.LinkedList<Foo>();
            list.Add(new FooBar());
            list.Add(new Foo());

            //this should compile
            //ISingleNode<FooBar> n1 = new SingleNode<FooBar>(null);
            //ISingleNode<Foo> n2 = node1;





        }
        [TestMethod]
        public void TestNodeSetLeft()
        {
            Node<int> node1 = new Node<int>(1);
            Node<int> node2 = new Node<int>(2);
            //test get Left node for extended class 1->2
            node1.Left = node2;
            var testResult6 = node1.Left;
            Assert.AreEqual(node2, testResult6);

            //assign node extended 1->2->3
            NodeExtended<int> nodeExtended = new NodeExtended<int>(3);
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
            Node<int> node1 = new Node<int>(1);
            Node<int> node2 = new Node<int>(2);

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
            SingleNodeExtended<int> node3Extended = new SingleNodeExtended<int>(4);
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
            NodeExtended<int> nodeExtended = new NodeExtended<int>(3);
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
        [TestMethod]
        public void TestInOutData()
        {
            Foo foo = new Foo();

            FooBar bar = new FooBar();

            ISingleNode<Foo> s1 = new SingleNode<Foo>(foo);
            ISingleNode<Foo> s2 = new SingleNode<Foo>(bar);


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
