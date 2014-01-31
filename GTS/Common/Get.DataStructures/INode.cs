using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.DataStructure
{
    public interface IData<T>
    {
        T Data { get; set; }
    }
    public interface INode<T> : IData<T>
    {
        INode<T> Left { get; set; }
        INode<T> Right { get; set; }
    }
}
