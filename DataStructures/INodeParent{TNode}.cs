using System;

namespace DataStructures
{
    /// <summary>
    /// Node for tree
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public interface INodeParent<TNode> : INode<TNode> 
        where TNode : class, INodeParent<TNode>, INode<TNode>
    {
        /// <summary>
        /// Parent
        /// </summary>
        TNode? P { get; set; }

        IComparable Key { get; set; }
    }
}
