namespace DataStructures
{
    /// <summary>
    /// Can be used for linked lists
    /// </summary>
    /// <typeparam name="TNode">The typ of nodes which can be connected</typeparam>
    public interface INode<TNode> where TNode : class, INode<TNode>
    {
        /// <summary>
        /// Left
        /// </summary>
        TNode? V { get; set; }
        /// <summary>
        /// Right
        /// </summary>
        TNode? U { get; set; }
    }
}
