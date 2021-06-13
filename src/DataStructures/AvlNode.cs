using System;

namespace DataStructures
{
    /// <summary>
    /// Node class used in avl tree
    /// </summary>
    /// <typeparam name="TData">The value typ which is used for storing data</typeparam>
    public class AvlNode<TData> : INodeTree<TData>
    {
        /// <summary>
        /// Initializes a new node
        /// </summary>
        public AvlNode(IComparable comparer, TData value)
        {
            Key = comparer;
            Value = value;
        }
        /// <summary>
        /// Get or sets whether the current node is balanced.
        /// </summary>
        /// <remarks>Can be used for an avl tree. <see href="https://en.wikipedia.org/wiki/AVL_tree#Balance_factor"/></remarks>
        public int Balance { get; set; }
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
    }
}
