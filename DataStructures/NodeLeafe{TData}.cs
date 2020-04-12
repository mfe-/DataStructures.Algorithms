using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public interface INodeLeafe<TData> : IData<TData>, INodeTree<INodeLeafe<TData>> 
    {

    }
    public class NodeLeafe<TData> : INodeLeafe<TData>
    {
        public NodeLeafe(IComparable comparer,TData Value)
        {
            Key = comparer;
        }
        public TData Value { get; set; }
        public INodeLeafe<TData> P { get; set; }
        public IComparable Key { get; set; }
        public INodeLeafe<TData> V { get; set; }
        public INodeLeafe<TData> U { get; set; }
    }
}
