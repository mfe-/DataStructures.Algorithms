using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure.Test.Extensions
{
    public static class Extensions
    {
        public static INode<D> Travers<D>(this INode<D> node) 
        {
            //I do nothing
            return node;
        }
        public static ISingleNode<D> Travers<D>(this ISingleNode<D> node) 
        {
            //I do nothing
            return node;
        }
    }
}
