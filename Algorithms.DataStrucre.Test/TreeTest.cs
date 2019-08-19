using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Get.the.Solution.Algorithms.Test
{
    [TestClass]
    public class TreeTest : BaseTest
    {
        [TestMethod]
        public void TestInOrder()
        {
            TreeNode<int> root = GenerateTree() as TreeNode<int>;

            IEnumerable<INode<int>> nodes = root.InOrder();

            nodes.Print();

            root = new TreeNode<int>(30);

            root.Left = new TreeNode<int>(10);
            root.Right = new TreeNode<int>(40);

            root.Left.Left = new TreeNode<int>(12);
            root.Left.Right = new TreeNode<int>(20);

            root.Right.Left = new TreeNode<int>(60);
            root.Right.Right = new TreeNode<int>(80);

            nodes = root.InOrder();

            nodes.Print();


            //T1 test - nach algodat skriptum

            root = new TreeNode<int>(5);
            
            root.Left = new TreeNode<int>(3);
            root.Left.Left = new TreeNode<int>(2);
            root.Left.Right = new TreeNode<int>(5);
            test(root.Left);
            root.Right = new TreeNode<int>(7);
            root.Right.Right = new TreeNode<int>(8);

            IEnumerable<int> expected = new List<int>()
            {
                2,3,5,5,7,8
            };

            IEnumerable<int> result = root.InOrder().Select(a=>a.Value);

            CollectionAssert.AreEqual(expected.ToList(), result.ToList());

        }
        public INode<int> test(INode<int> test)
        {
            return test;
        }
        [TestMethod]
        public void TestPreOrder()
        {
            TreeNode<int> root = GenerateTree() as TreeNode<int>;

            IEnumerable<INode<int>> nodes = root.PreOrder();
            nodes.Print();

            root = new TreeNode<int>(30);

            root.Left = new TreeNode<int>(10);
            root.Right = new TreeNode<int>(40);

            root.Left.Left = new TreeNode<int>(12);
            root.Left.Right = new TreeNode<int>(20);

            root.Right.Left = new TreeNode<int>(60);
            root.Right.Right = new TreeNode<int>(80);

            nodes = root.PreOrder();
        }
        [TestMethod]
        public void TestPostOrder()
        {
            Node<int> root = GenerateTree() as  TreeNode<int>;

            IEnumerable<INode<int>> nodes = root.PostOrder();
            nodes.Print();

            root = new TreeNode<int>(55);
            root.Left = new TreeNode<int>(40);
            root.Left.Left = new TreeNode<int>(33);
            root.Left.Right = new TreeNode<int>(50);

            root.Right = new TreeNode<int>(65);
            root.Right.Left = new TreeNode<int>(60);
            root.Right.Right = new TreeNode<int>(70);

            nodes = root.InOrder();

            //new tree
            root = new TreeNode<int>(30);
            root.Left = new TreeNode<int>(10);
            root.Right = new TreeNode<int>(40);

            root.Left.Left = new TreeNode<int>(12);
            root.Left.Right = new TreeNode<int>(20);

            root.Right.Left = new TreeNode<int>(60);
            root.Right.Right = new TreeNode<int>(80);

            nodes = root.PostOrder();

            //new tree
            root = new TreeNode<int>(6);
            root.Left = new TreeNode<int>(4);
            root.Right = new TreeNode<int>(5);

            root.Left.Left = new TreeNode<int>(8);
            root.Left.Left.Left = new TreeNode<int>(1);

            root.Left.Right = new TreeNode<int>(7);
            root.Left.Right.Left = new TreeNode<int>(2);
            root.Left.Right.Left.Right = new TreeNode<int>(3);

            root.Right.Right = new TreeNode<int>(9);

            nodes = root.PreOrder();

        }
        [TestMethod]
        public void TestCreatePostOrderList()
        {
            TreeNode<int> root = GenerateTree() as TreeNode<int>;
            IEnumerable<INode<int>> nodes = root.PostOrder();
            nodes.Print();

            INode<int> list = root.CreatePostOrderList();

            INode<int> temp = list;
            do
            {
                System.Diagnostics.Debug.WriteLine(temp.Value.ToString());
                temp = temp.Left;
            }
            while (temp != null);
        }
    }
}
