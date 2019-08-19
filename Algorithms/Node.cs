using Get.the.Solution.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.Algorithms
{
    public static class Node
    {
        public static IEnumerable<TSource> SetData<TSource>(this ISingleNode<TSource> source, TSource data)
        {
            if (source == null) throw new ArgumentException("source");

            if (source is ITreeNode<TSource>) return SetDataTreeNode<TSource>(source as ITreeNode<TSource>, data);
            if (source is INode<TSource>) return SetDataNode<TSource>(source as INode<TSource>,data);

            return SetDataSingleNode(source, data);
        }
        private static ISingleNode<TSource> SetDataSingleNode<TSource>(this ISingleNode<TSource> source, TSource data)
        {
            SingleNode<TSource> newnode = new SingleNode<TSource>(data);
            newnode.Right = source.Right;
            return newnode;
        }
        private static INode<TSource> SetDataNode<TSource>(this INode<TSource> source, TSource data)
        {
            return new Node<TSource>(data,source.Left,source.Right);
        }
        private static ITreeNode<TSource> SetDataTreeNode<TSource>(this ITreeNode<TSource> source, TSource data)
        {
            return new TreeNode<TSource>(data, source.Left, source.Right);
        }
    }
}
