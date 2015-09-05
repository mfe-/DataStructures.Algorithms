using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Get.the.Solution.DataStructure.Test
{
    [TestClass]
    public class TreeTest
    {
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

            IEnumerable<String> values = GetInputFile("0000");
            List<int> castedList = new List<int>();
            Tree<int> tree = new Tree<int>();
            //there should be a second tree and treenode structure for comparison
            System.Windows.Forms.TreeNode input = new System.Windows.Forms.TreeNode(); 

            int val;
            foreach (String s in values)
            {
                val = Int32.Parse(s);
                tree.Add(val);
                castedList.Add(val);
            }
            if (values.Count() == 0)
            {
                throw new ArgumentException("file contains no values");
            }

            InitialTest();
            
             System.Diagnostics.Debug.WriteLine("After initial test: tree size = " + tree.Length + " tree height = " + tree.Height);

            //long startTime = System.nanoTime();
            //testInsert(tree, input);
            //long estimatedTime = System.nanoTime() - startTime;
            //debugInformation(estimatedTime, "Insertion test took ", "After insertion test:", tree);

            //startTime = System.nanoTime();
            //testDelete(tree, input);
            //estimatedTime = System.nanoTime() - startTime;
            //debugInformation(estimatedTime, "Deletion test took ", "After deletion test:", tree);

            //startTime = System.nanoTime();
            //testPositions(tree, input);
            //estimatedTime = System.nanoTime() - startTime;
            //debugInformation(estimatedTime, "Testing positions took ", "After testing positions:", tree);

            //startTime = System.nanoTime();
            //testValues(tree, input);
            //estimatedTime = System.nanoTime() - startTime;
            //debugInformation(estimatedTime, "Testing value lists took ", "After creating value lists:", tree);

            //startTime = System.nanoTime();
            //tree.simpleBalance();
            //testBalance(tree, input);
            //estimatedTime = System.nanoTime() - startTime;
            //debugInformation(estimatedTime, "Testing tree balance took ", "After balancing:", tree);

            //StringBuffer msg = new StringBuffer(test ? choppedFileName + ": " : "");
            //msg.append("OK");
            //System.out.println("");
            //System.out.println(msg.toString());




        }

    }
}
