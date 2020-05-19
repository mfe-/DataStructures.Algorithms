using DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Algorithms.Test
{
    public class AvlTreeTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AvlTreeTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Theory]
        [InlineData("0000.txt")]
        [InlineData("0001.txt")]
        [InlineData("0002.txt")]
        [InlineData("0003.txt")]
        [InlineData("0004.txt")]
        public void Avltree_should_be_balanced_after_add(string sampleFile)
        {
            long start = DateTime.Now.Millisecond;
            long end = DateTime.Now.Millisecond;
            long offs = end - start;
            AvlTree<int> avl = new AvlTree<int>();
            //duplicates are forbidden
            HashSet<int> set = new HashSet<int>();

            IEnumerable<string> testinstance = EmbeddedResourceLoader.GetFileContents(sampleFile);
            var enumerator = testinstance.GetEnumerator();
            bool insert = true;
            start = DateTime.Now.Millisecond;
            while (enumerator.MoveNext())
            {
                string s = enumerator.Current;
                if (s == "#insert")
                {
                    insert = true;
                }
                else if (s == "#remove")
                {
                    insert = false;
                }
                int val = 0;
                if (int.TryParse(s, out val))
                {
                    if (insert)
                    {
                        set.Add(val);
                        try
                        {
                            avl.Add(val, val);
                        }
                        catch (ArgumentException e)
                        {
                            _testOutputHelper.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        set.Remove(val);
                        avl.Remove(val);
                    }
                }
                int items = avl.Inorder().Count();
                int expected = set.Count;

                Assert.Equal(expected, items);
                // check balance
                int a = CheckHeight(avl.RootNode);
            }
            end = DateTime.Now.Millisecond;

            // Inorder
            var inorder = avl.Inorder().ToList();

            // check amount of nodes (is everything stored)
            int n = inorder.Count();

            Assert.True(set.Count() == n, "amount of nodes not correct: " + n + " (setpoint: "
                    + set.Count() + ")");

            CheckParent(avl.RootNode);

            // check sort order
            int?[] parents = new int?[n];
            if (n > 0)
            {
                // safe parentnode
                parents[0] = (inorder[0].P == null ? null : (int?)inorder[0].P.Key);
            }
            for (int i = 1; i < n; i++)
            {
                var v = inorder[i];

                Assert.False((int)inorder[i - 1].Key >= (int)v.Key,
                    "node not sorted: " + inorder[i - 1].Key + " vs. "
                    + v.Key + " (position: " + (i - 1) + " vs. " + i + ")");

                // check Key (leafenode!)
                Assert.False(!set.Contains((int)v.Key), "wrong node in tree: " + v.Key);
                // save parent node
                parents[i] = (v.P == null ? null : (int?)v.P.Key);
            }

            // print results
            long sum = end - start - offs;
            _testOutputHelper.WriteLine($"sampleFile:{sampleFile}");
            _testOutputHelper.WriteLine($"time:{sum} ms");

        }
        [Theory]
        [InlineData("0000.txt")]
        [InlineData("0001.txt")]
        [InlineData("0002.txt")]
        [InlineData("0003.txt")]
        [InlineData("0004.txt")]
        public void GetMin_and_GetMax_should_return_min_and_max_of_current_Tree(string sampleFile)
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
            AvlTree<int> avlTree = new AvlTree<int>();
            foreach (int key in ints)
            {
                avlTree.Add(key, key);
            }
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"avlTree Add:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            stopwatch.Start();
            var leafemin = avlTree.GetMinimum().Key;
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"avlTree Min:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            stopwatch.Start();
            var hasmin = hasSet.Min();
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"hasSet Min:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            Assert.Equal(hasmin, leafemin);

            stopwatch.Start();
            var leafemax = avlTree.GetMaximum().Key;
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"avlTree Min:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            stopwatch.Start();
            var hasmax = hasSet.Max();
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"hasSet Min:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            Assert.Equal(hasmax, leafemax);

            //remove hasmin

            stopwatch.Start();
            avlTree.Remove(hasmin);
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"avlTree Remove:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            stopwatch.Start();
            hasSet.Remove(hasmin);
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"hasSet Remove:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            //get again the minimum

            stopwatch.Start();
             leafemin = avlTree.GetMinimum().Key;
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"avlTree Min:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            stopwatch.Start();
            hasmin = hasSet.Min();
            stopwatch.Stop();
            _testOutputHelper.WriteLine($"hasSet Min:{stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            Assert.Equal(hasmin, leafemin);

            //[InlineData("0007.txt")]
            //HashSet Add:29
            //avlTree Add:3148207
            //avlTree Min:0
            //hasSet Min:7
            //avlTree Min:0
            //hasSet Min:2
            //avlTree Remove:82
            //hasSet Remove:0
            //avlTree Min:0
            //hasSet Min:2

    }
        private static void CheckParent<TNode>(TNode v) 
            where TNode : class, INodeParent<TNode>, INode<TNode>
        {
            if (v == null)
                return;
            if (v.V != null)
            {

                Assert.False(v.V.P != v, "parent node not correct: " + v.Key);
                CheckParent(v.V);

            }
            if (v.U != null)
            {

                Assert.False(v.U.P != v, "parent node not correct: " + v.Key);
                CheckParent(v.U);
            }

        }
        protected static int CheckHeight<TNode>(TNode root) 
            where TNode : class, INodeParent<TNode>, INode<TNode>
        {
            if (root == null)
                return 0;

            int h1 = CheckHeight(root.V);
            int h2 = CheckHeight(root.U);
            int bal = h2 - h1;
            int h = Math.Max(h1, h2) + 1;

            if ((bal * bal) > 1)
                throw new ArithmeticException($"The node with the key " + root.Key
                    + " is not balanced! (" + h1 + " vs " + h2 + ")");
            return h;
        }
    }
}
