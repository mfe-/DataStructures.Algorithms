using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get.the.Solution.DataStructure;

namespace Get.the.Solution.DataStructure
{
    //covariant for properties are not supported 
    //http://stackoverflow.com/questions/4348760/c-sharp-covariant-return-types-utilizing-generics

    public interface INode<D, T> : ISingleNode<D, T>
        where T : INode<D, T>
    {
        T GetLeft();
        void SetLeft(T t);
    }


}
