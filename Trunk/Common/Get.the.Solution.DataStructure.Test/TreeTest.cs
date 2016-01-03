using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Diagnostics;

namespace Get.the.Solution.DataStructure.Test
{
    /// <summary>
    /// Provides test methods for tree implementations
    /// </summary>
    [TestClass]
    public class TreeTest
    {
        private CompositionContainer _Container = null;
        /// <summary>
        /// Factor used during tests
        /// </summary>
        private static int factor = 10;

        /// <summary>
        /// Searches for classes which implements ITree in the
        /// Get.the.Solution.DataStructure.Test and Get.the.Solution.DataStructure project
        /// </summary>
        [TestInitialize()]
        public void InitialTest()
        {
            //http://blogs.msdn.com/b/hammett/archive/2011/03/08/mef-s-convention-model.aspx

            if (_Container == null)
            {
                var catalog = new AggregateCatalog(
                    new AssemblyCatalog(Assembly.GetExecutingAssembly()),
                    new AssemblyCatalog(typeof(ITree<>).Assembly)
                    );

                //Create the current composition container to create the parts
                _Container = new CompositionContainer(catalog);

                //bind import/ exports
                _Container.ComposeParts(this);

                TreeInstances.Remove(ComparisonTreeTestInstance);
            }
            
        }

        /// <summary>
        /// Instance used to compare the tested tree
        /// </summary>
        [Import("TreeTest")]
        protected ITree<int> ComparisonTreeTestInstance;
        /// <summary>
        /// This instances must pass the test
        /// </summary>
        [ImportMany(typeof(ITree<>))]
        protected List<ITree<int>> TreeInstances;

        /// <summary>
        /// Reads the overgiven filename and creates for every line a list element
        /// </summary>
        /// <remarks>
        /// The file have to be located in the project folder instanzen 
        /// </remarks>
        /// <param name="filename">The name of the file</param>
        /// <returns>The created list</returns>
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

        /// <summary>
        /// Tests all tree classes which implements ITree. 
        /// The following test methods will be called <see cref="TestContent"/>, <see cref="TestValues"/>,
        /// <see cref="TestPositions"/>, <see cref="TestInsert"/> and <see cref="TestDelete"/>
        /// </summary>
        [TestMethod]
        public void TestTree()
        {
            String testinstanz = "0001";
            foreach (var t in TreeInstances)
            {
                t.Clear();

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
                //comparison tree instance
                ITree<int> input = ComparisonTreeTestInstance;

                IEnumerable<String> values = GetInputFile(testinstanz);
                //tree instance to test
                ITree<int> tree = t;
                tree.Clear();


                int val;
                foreach (String s in values)
                {
                    val = Int32.Parse(s);
                    //tree data strucutre to test
                    tree.Add(val);
                    input.Add(val);
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
        }
        /// <summary>
        /// Compares the input of a given tree with a tree set build from the original data source
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="input"></param>
        private static void TestContent(ITree<int> tree, ITree<int> input)
        {
            if (tree.Empty != input.Empty)
            {
                new AssertFailedException("Failure when calling isEmpty()!");
            }
            if (tree.Length != input.Length)
            {
                new AssertFailedException("Incorrect size - " + " tree: " + tree.Length + " tree set: " + input.Length);
            }
            int counter = 0;
            for (int i = 0; i < input.Length; i++)
            {
                int val = input.FindIndex(i);
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
        private static void TestValues(ITree<int> tree, ITree<int> input)
        {
            TestContent(tree, input);
            //int[] testarr = input.InOrder(input.Root, new List<TreeSet.Node>()).Select(a => a.Key).ToArray();
            int[] testarr = new int[input.Length]; 
            input.CopyTo(testarr,0);
            
            int mark1, mark2, run;
            int size = input.Length;
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
        private static void TestPositions(ITree<int> tree, ITree<int> input)
        {
            TestContent(tree, input);
            int[] testarr = new int[input.Length];
            input.CopyTo(testarr, 0);
            int counter = input.Length; //Length
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
        private static void TestInsert(ITree<int> tree, ITree<int> input)
        {
            TestContent(tree, input);
            int[] arr = new int[input.Length];
            input.CopyTo(arr, 0);

            int first = arr.First();
            int last = arr.Last();
            for (int i = first; i <= last; i++)
            {
                input.Add(i); //insert
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
        private static void TestDelete(ITree<int> tree, ITree<int> input)
        {
            TestContent(tree, input);
            int[] arr = new int[input.Length];
            input.CopyTo(arr, 0);

            int first = arr.First();
            int last = arr.Last();

            for (int i = first; i <= last; i += factor)
            {
                input.Remove(i); //delete
                tree.Remove(i);
            }
            TestContent(tree, input);
        }
        /// <summary>
        /// Performs a run time test for all tree instances
        /// </summary>
        [TestMethod]
        public void RunTimeTest()
        {
            //get test values from file
            IList<int> values = new List<int>(200812);
            IEnumerable<String> rawvalues = GetInputFile("0002");
            IEnumerable<String> rawvalues1 = GetInputFile("0007");

            int val;
            foreach (String s in rawvalues)
            {
                val = Int32.Parse(s);
                values.Add(val);
            }
            foreach (String s in rawvalues1)
            {
                val = Int32.Parse(s);
                values.Add(val);
            }
            values = values.Distinct().ToList();

            TreeInstances.Add(ComparisonTreeTestInstance);

            foreach (var t in TreeInstances)
            {
                t.Clear();
                //add
                DateTime start = DateTime.Now;
                System.Diagnostics.Debug.WriteLine(String.Format("Test for {0} with {1} element(s) to add starting", t.GetType(), values.Count));
                foreach (int i in values)
                {
                    t.Add(i);
                }
                TimeSpan end = DateTime.Now - start;
                System.Diagnostics.Debug.WriteLine(String.Format("Test for {0} with {1} element(s) to add took {2}.", t.GetType(), values.Count, end));
                Assert.AreEqual(t.Length, values.Count);
                Assert.IsTrue(end.TotalSeconds < 1.5);

                //IndexOf
                start = DateTime.Now;
                System.Diagnostics.Debug.WriteLine(String.Format("Test for {0} with 1 element(s) to IndexOf starting", t.GetType(), values.Count));
                foreach (int i in values)
                {
                    t.IndexOf(i);
                    Assert.IsTrue((DateTime.Now - start).TotalSeconds < 0.4);
                    //comment break; when executing test on localmachine, workaround for appveyor
                    break;
                }
                end = DateTime.Now - start;
                System.Diagnostics.Debug.WriteLine(String.Format("Test for {0} with 1 element(s) to IndexOf took {2}.", t.GetType(), values.Count, end));

                //remove
                start = DateTime.Now;
                System.Diagnostics.Debug.WriteLine(String.Format("Test for {0} with {1} element(s) to remove starting", t.GetType(), values.Count));
                foreach (int i in values)
                {
                    t.Remove(i);
                }
                end = DateTime.Now - start;
                System.Diagnostics.Debug.WriteLine(String.Format("Test for {0} with {1} element(s) to remove took {2}.", t.GetType(), values.Count, end));
                Assert.IsTrue(end.TotalSeconds < 5.0);
            }
        }
        private static void DebugInformation(long estimatedTime, String first, String second, ITree<int> tree)
        {
            System.Diagnostics.Debug.WriteLine(first + estimatedTime * 1.0 / 1000000000 + " seconds");
            System.Diagnostics.Debug.WriteLine(second + " tree size = " + tree.Length + ", tree height = " + tree.Height);
        }

    }
}
