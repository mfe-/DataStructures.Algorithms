using System;

namespace DataStructures
{
    /// <summary>
    /// Node for tree
    /// </summary>
    /// <typeparam name="TNode">The tree node typ.</typeparam>
    public interface INodeParent<TNode> : INode<TNode> 
        where TNode : class, INodeParent<TNode>, INode<TNode>
    {
        /// <summary>
        /// Parent
        /// </summary>
        TNode? P { get; set; }
        /// <summary>
        /// The identifier of <seealso cref="INodeParent{TNode}"/>
        /// </summary>
        IComparable Key { get; set; }
    }
}
