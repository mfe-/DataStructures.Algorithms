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
        where T : ISNode<T>
    {
        T Right { get; set; }
    }
    public interface INode<T,D> : IData<D>, ISNode<T>
        where T : INode<T,D>
    {
        
        T Left { get; set; }
    }
}
