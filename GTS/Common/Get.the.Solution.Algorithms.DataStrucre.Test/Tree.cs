using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Get.the.Solution.Algorithms.Test
{
    [TestClass]
    public class Tree : BaseTest
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

            root = new TreeNode<int>(30);

            root.Left = new TreeNode<int>(10);
            root.Right = new TreeNode<int>(40);

            root.Left.Left = new TreeNode<int>(12);
            root.Left.Right = new TreeNode<int>(20);

            root.Right.Left = new TreeNode<int>(60);
            root.Right.Right = new TreeNode<int>(80);

            nodes = root.PostOrder();
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
