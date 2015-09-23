using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure.Test
{
    [TestClass]
    public class TreeTest
    {
        /// <summary>
        /// Factor used during tests
        /// </summary>
        private static int factor = 10;

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void InitialTest()
        {
            Tree<int> t = new Tree<int>();

            int[] arr = new int[] { 30, 20, 45, 10, 25, 38, 50, 5, 13, 22, 29, 12, 14 };


            if (!t.Empty || t.Length != 0 || t.Height != -1)
            {
                new AssertFailedException("Error when testing empty tree!");
            }
            foreach (int i in arr)
            {
                t.Add(i);
            }
            if (t.Empty)
            {
                new AssertFailedException("Check the implementation of isEmpty()!");
            }
            if (t.Length != arr.Length)
            {
                new AssertFailedException("Check the implementation of size()!");
            }
            if (!t.Exists(20) || t.Exists(23))
            {
                new AssertFailedException("Check the implementation of exists()!");
            }
            if (t.FindIndex(3) != 13 || t.FindIndex(0) != 5)
            {
                new AssertFailedException("Check the implementation of valueAtPosition()!");
            }
            if (t.IndexOf(10) != 1 || t.IndexOf(29) != 8 || t.IndexOf(31) != 10)
            {
                new AssertFailedException("Check the implementation of position()!");
            }
            if (t.Height != 4)
            {
                new AssertFailedException("Check the implementation of height()!");
            }
            try
            {
                t.FindIndex(1000);
            }
            catch (ArgumentException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
        internal static IEnumerable<String> GetInputFile(string filename)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            string path = "Get.the.Solution.DataStructure.Test.instanzen";

            using (TextReader reader = new StreamReader(thisAssembly.GetManifestResourceStream(path + "." + filename)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
        public IEnumerable<String> TreeTestValues { get; set; }

        [TestMethod]
        public void TestTree()
        {
            TreeSet input = new TreeSet();

            IEnumerable<String> values = GetInputFile("0002");
            Tree<int> tree = new Tree<int>();


            int val;
            foreach (String s in values)
            {
                val = Int32.Parse(s);
                //tree data strucutre to test
                tree.Add(val);
                input.insert(val);
            }
            if (values.Count() == 0)
            {
                throw new ArgumentException("file contains no values");
            }

            InitialTest();

            System.Diagnostics.Debug.WriteLine("After initial test: tree size = " + tree.Length + " tree height = " + tree.Height);

            long startTime = DateTime.Now.Ticks;
            TestInsert(tree, input);
            long estimatedTime = DateTime.Now.Ticks - startTime;
            DebugInformation(estimatedTime, "Insertion test took ", "After insertion test:", tree);

            startTime = DateTime.Now.Ticks;
            TestDelete(tree, input);
            estimatedTime = DateTime.Now.Ticks - startTime;
            DebugInformation(estimatedTime, "Deletion test took ", "After deletion test:", tree);

            startTime = DateTime.Now.Ticks;
            TestPositions(tree, input);
            estimatedTime = DateTime.Now.Ticks - startTime;
            DebugInformation(estimatedTime, "Testing positions took ", "After testing positions:", tree);

            startTime = DateTime.Now.Ticks;
            //testValues(tree, input); - Method not implemented by our tree data strucutre
            estimatedTime = DateTime.Now.Ticks - startTime;
            DebugInformation(estimatedTime, "Testing value lists took ", "After creating value lists:", tree);

            startTime = DateTime.Now.Ticks;
            //tree.simpleBalance(); - Method not implemented by our tree data strucutre
            //testBalance(tree, input); - Method not implemented by our tree data strucutre
            estimatedTime = DateTime.Now.Ticks - startTime;
            DebugInformation(estimatedTime, "Testing tree balance took ", "After balancing:", tree);

            StringBuilder msg = new StringBuilder("");
            msg.Append("OK");
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine(msg.ToString());

        }
        /// <summary>
        /// Compares the input of a given tree with a tree set build from the original data source
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="input"></param>
        [TestMethod]
        private static void TestContent(Tree<int> tree, TreeSet input)
        {
            if (tree.Empty != input.isEmpty())
            {
                new AssertFailedException("Failure when calling isEmpty()!");
            }
            if (tree.Length != input.size())
            {
                new AssertFailedException("Incorrect size - " + " tree: " + tree.Length + " tree set: " + input.size());
            }
            int counter = 0;
            for (int i = 0; i < input.size(); i++)
            {
                int val = input.position(i);
                if (tree.Exists(val) == false)
                {
                    new AssertFailedException("Element " + i + " is missing!");
                }
                if (tree.FindIndex(counter++) != i)
                {
                    new AssertFailedException("Element " + i + " is at the wrong position!");
                }
            }
        }
        /// <summary> 
        /// Tests the implementation of the method values in the tree implementation.
        /// Results from a tree set are used for comparison.
        /// </summary>
        /// <param name="tree">tree  tree build from original data source</param>
        /// <param name="input">input tree set build from original data source</param>
        [TestMethod]
        private static void TestValues(Tree<int> tree, TreeSet input)
        {
            TestContent(tree, input);
            int[] testarr = input.InOrder(input.Root, new List<TreeSet.Node>()).Select(a => a.Key).ToArray();
            int mark1, mark2, run;
            int size = input.size();
            int counter = size / factor;
            for (int i = 0; i < counter; i = (i + 1) * 2)
            {
                mark1 = i;
                mark2 = size - 1 - i;
                //for (int e : tree.values(testarr[mark1], testarr[mark2])) {
                //    if (e != testarr[mark1++]) {
                //        new AssertFailedException(("Failure when calling method values()!");
                //    }
                //}
                mark1 = i;
                mark2 = size - 1 - i;
                run = 0;
                //for (int e : tree.values(testarr[mark2], testarr[mark1])) {
                //    if (e != testarr[run <= mark1 ? run++ : mark2++]) {
                //        new AssertFailedException(("Failure when calling method values()!");
                //    }
                //}
            }
            TestContent(tree, input);
        }
        /// <summary>
        /// Tests the implementation of the method position in the tree implementation.
        /// Results from a tree set are used for comparison.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="input"></param>
        [TestMethod]
        private static void TestPositions(Tree<int> tree, TreeSet input)
        {
            TestContent(tree, input);
            int[] testarr = input.InOrder(input.Root, new List<TreeSet.Node>()).Select(a => a.Key).ToArray();
            int counter = input.size();
            for (int i = 0; i < counter; i += factor)
            {
                if (tree.IndexOf(testarr[i]) != i)
                {
                    new AssertFailedException("Position mismatch!");
                }
            }
            TestContent(tree, input);
        }

        /// <summary>
        /// Tries to insert integers in a given tree and a given tree set. This method proceeds as follows: (1) Compare the
        /// structure of the given tree and the given tree set to ensure a proper initial situation. (2) Insertion of values.
        /// If a value is already present in both data structures no insertion should take place. (3) Compare the structure
        /// of the given tree and the given tree set to ensure that the insertion procedure worked properly and both data
        /// structures include the same values.
        /// </summary>
        /// <param name="tree">tree  tree build from original data source</param>
        /// <param name="input">input tree set build from original data source</param>
        [TestMethod]
        private static void TestInsert(Tree<int> tree, TreeSet input)
        {
            TestContent(tree, input);
            int first = input.InOrder(input.Root, new List<TreeSet.Node>()).First().Key;
            int last = input.InOrder(input.Root, new List<TreeSet.Node>()).Last().Key;
            for (int i = first; i <= last; i++)
            {
                input.insert(i);
                tree.Add(i);
            }
            TestContent(tree, input);
        }
        /// <summary>
        /// Tries to delete integers in a given tree and a given tree set. This method proceeds as follows: (1) Compare the
        /// structure of the given tree and the given tree set to ensure a proper initial situation. (2) Deletion of values.
        /// If a value is not present in both data structures no deletion should take place. (3) Compare the structure of the
        /// given tree and the given tree set to ensure that the deletion procedure worked properly and both data structures
        /// include the same values.
        /// </summary>
        /// <param name="tree">tree  tree build from original data source</param>
        /// <param name="input">input tree set build from original data source</param>
        [TestMethod]
        private static void TestDelete(Tree<int> tree, TreeSet input)
        {
            TestContent(tree, input);
            int first = input.InOrder(input.Root, new List<TreeSet.Node>()).First().Key;
            int last = input.InOrder(input.Root, new List<TreeSet.Node>()).Last().Key;
            for (int i = first; i <= last; i += factor)
            {
                input.delete(i);
                tree.Remove(i);
            }
            TestContent(tree, input);
        }

        private static void DebugInformation(long estimatedTime, String first, String second, Tree<int> tree)
        {
            System.Diagnostics.Debug.WriteLine(first + estimatedTime * 1.0 / 1000000000 + " seconds");
            System.Diagnostics.Debug.WriteLine(second + " tree size = " + tree.Length + ", tree height = " + tree.Height);
        }

    }
}
