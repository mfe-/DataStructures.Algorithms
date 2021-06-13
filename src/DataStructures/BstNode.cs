using System;

namespace DataStructures
{
    /// <summary>
    /// Node which can be used for a binary search tree
    /// </summary>
    /// <typeparam name="TData">The value typ which is used for storing data</typeparam>
    public class BstNode<TData> : INodeTree<TData>
    {
        /// <summary>
        /// Initializes a new node
        /// </summary>
        public BstNode(IComparable comparer, TData value)
        {
            Key = comparer;
            Value = value;
        }
        /// <inheritdoc/>
        public TData Value { get; set; }
        /// <inheritdoc/>
        public INodeTree<TData>? P { get; set; }
        /// <inheritdoc/>
        public IComparable Key { get; set; }
        /// <inheritdoc/>
        public INodeTree<TData>? V { get; set; }
        /// <inheritdoc/>
        public INodeTree<TData>? U { get; set; }
        /// <summary>
        /// Get or sets the amount of child nodes from the current node
        /// </summary>
        public int AmountofNode { get; set; }
    }
}
