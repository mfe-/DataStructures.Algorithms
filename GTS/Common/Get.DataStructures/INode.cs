using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public interface IData<T>
    {
        T Value { get; set; }
    }
    public interface ISNode<T>
    {
        INode<T> Right { get; set; }
    }
    public interface INode<T> : IData<T>, ISNode<T>
    {
        
        INode<T> Left { get; set; }
    }
}
