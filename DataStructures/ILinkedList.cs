using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.DataStructure;

namespace Get.the.Solution.DataStructure
{
    public interface ILinkedList<T>
        where T : Node<T>
    {
        T Root { get; set; }
    }
}
