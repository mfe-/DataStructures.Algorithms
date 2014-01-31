using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.DataStructure;

namespace Get.DataStructure
{
    public interface ILNode<T> : INode<T>
    {
        INode<T> Parent { get; set; }
    }
}
