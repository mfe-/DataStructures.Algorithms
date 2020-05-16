using DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Algorithms.Test
{
    public class BinarySearchTreeTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BinarySearchTreeTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Tests the implementation with an empty tree and the example tree from the exercise sheet ss15.
        /// </summary>
        [Fact]
        public void InitialTest()
        {
            BinarySearchTree<int> t = new BinarySearchTree<int>();
            int[] arr = new int[] { 30, 20, 45, 10, 25, 38, 50, 5, 13, 22, 29, 12, 14 };

            Assert.False(!t.Empty || t.Count != 0 || t.Height != -1, "Error when testing empty tree!");

            foreach (var i in arr)
            {
                t.Add(i, i);
            }

            Assert.False(t.Empty, "Check the implementation of Empty!");

            Assert.False(t.Count != arr.Length, "Check the implementation of Length!");

            Assert.False(!t.ContainsKey(20) || t.ContainsKey(23), "Check the implementation of ContainsKey()!");

            //Assert.False(t.GetElementAt(3) != 13 || t.GetElementAt(0) != 5), "Check the implementation of GetElementAt()!");

            //Assert.False(t.position(10) != 1 || t.position(29) != 8 || t.position(31) != 10), "Check the implementation of position()!");

            Assert.False(t.Height != 4, "Check the implementation of height!");

            try
            {
                //t.GetElementAt(1000);
            }
            catch (ArgumentException e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
        }

        [Theory]
        [InlineData("0011.txt")]
        public void Some_basic_tree_tests_like_add_remove(string sampleFile)
        {
            IEnumerable<string> testinstance = EmbeddedResourceLoader.GetFileContents(sampleFile);
            List<int> ints = new List<int>();
            testinstance = testinstance.Distinct();
            foreach (var line in testinstance)
            {
                int val = 0;
                if (int.TryParse(line, out val))
                {
                    ints.Add(val);
                }
            }

            AvlTree<int> input = new AvlTree<int>(); //use AVL tree to check against the BinarySearchTree
            BinarySearchTree<int> tree = new BinarySearchTree<int>();

            foreach (int i in ints)
            {
                tree.Add(i, i);
                input.Add(i, i);
            }

            TestContent(tree, input);
        }

        /// <summary>
        /// Compares the input of a given tree with a tree set build from the original data source
        /// </summary>
        /// <param name="tree">tree to compare to</param>
        /// <param name="input">tree set build from original data source</param>
        private void TestContent<TNode>(AbstractTree<TNode,int> tree, AbstractTree<TNode,int> input) where TNode : class, INodeParent<TNode>, INode<TNode>, IData<int>
        {
            Assert.False(tree.Empty != input.Empty, "Failure when calling isEmpty()!");

            Assert.False(tree.Count != input.Count, "Incorrect size - " + " tree: " + tree.Count + " tree set: " + input.Count);

            //int counter = 0;
            //foreach (var i in input)
            //{
            //    if (tree.exists(i) == false)
            //    {
            //        bailOut("Element " + i + " is missing!");
            //    }
            //    if (tree.GetElementAt(counter++) != i)
            //    {
            //        bailOut("Element " + i + " is at the wrong position!");
            //    }
            //}
        }

        [Fact]
        public void TestInOrder()
        {
            BinarySearchTree<int> binarySearchTree = new BinarySearchTree<int>();

            binarySearchTree.Add(8, 1);
            binarySearchTree.Add(3, 1);
            binarySearchTree.Add(1, 1);
            binarySearchTree.Add(6, 1);
            binarySearchTree.Add(4, 1);
            binarySearchTree.Add(7, 1);

            binarySearchTree.Add(10, 1);
            binarySearchTree.Add(14, 1);
            binarySearchTree.Add(13, 1);

            var inorderResult = binarySearchTree.Inorder().ToArray();

            Assert.Equal(new int[] { 1, 3, 4, 6, 7, 8, 10, 13, 14 }, inorderResult.Select(a => (int)a.Key).ToArray());


            for (int i = 0; i < binarySearchTree.Count; i++)
            {
                int indextest = i;
                var expected = inorderResult[indextest];
                var result = binarySearchTree.GetElementAt(indextest);
                Assert.Equal(expected, result);
            }

            binarySearchTree = new BinarySearchTree<int>();

            for(int i=0;i<10;i++)
            {
                binarySearchTree.Add(i, i);
            }

            inorderResult = binarySearchTree.Inorder().ToArray();
            for (int i = 0; i < binarySearchTree.Count; i++)
            {
                int indextest = i;
                var expected = inorderResult[indextest];
                var result = binarySearchTree.GetElementAt(indextest);
                Assert.Equal(expected, result);
            }
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
        }
        [Theory]
        [InlineData("0011.txt")]
        [InlineData("0013.txt")]
        [InlineData("0014.txt")]
        [InlineData("0021.txt")]
        [InlineData("0100.txt")]
        [InlineData("0007.txt")]
        public void Add_test_compared_to_hashSet(string sampleFile)
        {
            IEnumerable<string> testinstance = EmbeddedResourceLoader.GetFileContents(sampleFile);
            List<int> ints = new List<int>();
            testinstance = testinstance.Distinct();
            foreach (var line in testinstance)
            {
                int val = 0;
                if (int.TryParse(line, out val))
                {
                    ints.Add(val);
                }
            }
            _testOutputHelper.WriteLine($"Adding:{ints.Count} items");
            HashSet<int> hasSet = new HashSet<int>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (int key in ints)
            {
                hasSet.Add(key);
            }
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"HashSet Add:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();
            stopwatch.Start();
            BinarySearchTree<int> binarySearchTree = new BinarySearchTree<int>();
            int i = 0;
            foreach (int key in ints)
            {
                binarySearchTree.Add(key, key);
                i++;
            }
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"BinarySearchTree Add:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            Assert.Equal(hasSet.Count, binarySearchTree.Count);

            stopwatch.Start();
            var leafemin = binarySearchTree.GetMinimum().Key;
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"binarySearchTree Min:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            stopwatch.Start();
            var hasmin = hasSet.Min();
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"hasSet Min:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            Assert.Equal(hasmin, leafemin);

            stopwatch.Start();
            var leafemax = binarySearchTree.GetMaximum().Key;
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"binarySearchTree max:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            stopwatch.Start();
            var hasmax = hasSet.Max();
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"hasSet max:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            Assert.Equal(hasmax, leafemax);
        }
    }
}
