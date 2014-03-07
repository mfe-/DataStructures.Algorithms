using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Get.DataStructure;

namespace Get.Algorithms.DataStrucre.Test
{
    /// <summary>
    /// Summary description for DataStructure
    /// </summary>
    [TestClass]
    public class DataStructure
    {
        public DataStructure()
        {
            Node<int> n1 = new Node<int>();
            Node<int> n2 = new Node<int>();
            n1.Left = n2;
            n1.Left.Right = n1;

            Vertex<int, Object> v1 = new Vertex<int, object>();

            Vertex<int, Object> v2 = new Vertex<int, Object>();

            Edge<int, Object> e1 = new Edge<int, Object>(null, null);
            e1.U = v1;
            e1.Value = new Object();


            v1.AddEdge(v2, 2, true);

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
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

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //


            
        }
    }
}
