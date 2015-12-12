using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.Algorithms.Test
{
    public class BaseTest
    {
        public IList<int> RandomList<T>() where T : IComparable<T>
        {
            Random rand = new Random();
            List<int> result = new List<int>();
            HashSet<int> check = new HashSet<int>();
            for (Int32 i = 0; i < 300; i++)
            {
                int curValue = rand.Next(1, 100000);
                while (check.Contains(curValue))
                {
                    curValue = rand.Next(1, 100000);
                }
                result.Add(curValue);
                check.Add(curValue);
            }
            return result;
        }
        public ITreeNode<int> GenerateTree()
        {
            TreeNode<int> root = new TreeNode<int>(10);

            //create left tree
            TreeNode<int> l7 = new TreeNode<int>(7);
            TreeNode<int> l2 = new TreeNode<int>(2);
            TreeNode<int> l9 = new TreeNode<int>(9);
            TreeNode<int> l1 = new TreeNode<int>(1);
            TreeNode<int> l4 = new TreeNode<int>(4);
            TreeNode<int> l3 = new TreeNode<int>(3);
            TreeNode<int> l8 = new TreeNode<int>(8);

            l3.Parent = l4;
            l4.Right = l3;

            l4.Parent = l2;
            l2.Right = l4;

            l1.Parent = l2;
            l2.Left = l1;

            l2.Parent = l7;
            l7.Left = l2;

            l8.Parent = l9;
            l9.Left = l8;

            l9.Parent = l7;
            l7.Right = l9;

            l7.Parent = root;
            root.Left = l7;

            //create right tree
            TreeNode<int> r11 = new TreeNode<int>(11);
            TreeNode<int> r12 = new TreeNode<int>(12);
            TreeNode<int> r18 = new TreeNode<int>(18);
            TreeNode<int> r15 = new TreeNode<int>(15);

            r11.Parent = r12;
            r12.Left = r11;

            r12.Parent = r15;
            r15.Left = r12;

            r18.Parent = r15;
            r15.Right = r18;

            r15.Parent = root;
            root.Right = r15;

            return root;
        }

    }
}
