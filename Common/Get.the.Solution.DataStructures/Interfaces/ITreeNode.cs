
namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Represents a node in a binary tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITreeNode<T> : INode<T>
    {
        /// <summary>
        /// Get or sets the parent node
        /// </summary>
        ITreeNode<T> Parent { get; set; }
        /// <summary>
        /// Get or sets the left node
        /// </summary>
        new ITreeNode<T> Left { get; set; }
        /// <summary>
        /// Get or sets the right node
        /// </summary>
        new ITreeNode<T> Right { get; set; }
    }
}
